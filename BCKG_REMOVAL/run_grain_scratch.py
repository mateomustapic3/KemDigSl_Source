"""
Film grain and scratch remover for old photos.

Workflow:
- optional grain removal using OpenCV fastNlMeansDenoisingColored
- automatic thin-line mask extraction (bright/dark scratches)
- inpainting with LaMa (if available), otherwise OpenCV Telea inpaint

Usage example:
python run_grain_scratch.py --input input.jpg --output cleaned.png --denoise 35 --scratch 55
"""

import argparse
import os
import sys
from pathlib import Path

import cv2
import numpy as np

ROOT = Path(__file__).resolve().parent
LAMA_DIR = ROOT / "lama"
LAMA_VERBOSE = os.environ.get("LAMA_VERBOSE", "").strip() == "1"


def log_warn(message: str) -> None:
    if LAMA_VERBOSE:
        print(message)


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Film grain & scratch remover using LaMa")
    parser.add_argument("--input", required=True, help="Input image path")
    parser.add_argument("--output", required=True, help="Output image path")
    parser.add_argument(
        "--denoise",
        type=float,
        default=35,
        help="Grain removal strength (0-60 recommended, higher = stronger)",
    )
    parser.add_argument(
        "--scratch",
        type=float,
        default=55,
        help="Scratch detection sensitivity (0-100, higher detects more)",
    )
    parser.add_argument(
        "--no-lama",
        action="store_true",
        help="Force fallback to OpenCV inpaint instead of LaMa",
    )
    parser.add_argument(
        "--device",
        default="cpu",
        choices=["cpu", "cuda"],
        help="Device hint for LaMa (defaults to CPU)",
    )
    return parser.parse_args()


def load_image(path: str) -> np.ndarray:
    img = cv2.imread(path, cv2.IMREAD_COLOR)
    if img is None:
        raise FileNotFoundError(f"Cannot read image: {path}")
    return img


def remove_grain(img: np.ndarray, strength: float) -> np.ndarray:
    strength = max(0.0, min(strength, 80.0))
    if strength < 1e-3:
        return img

    # tune fastNlMeansDenoisingColored parameters from slider value
    h_color = 5 + strength * 0.8
    h_luma = 4 + strength * 0.6
    template = 7
    search = 21
    return cv2.fastNlMeansDenoisingColored(img, None, h_luma, h_color, template, search)


def build_scratch_mask(img: np.ndarray, sensitivity: float) -> np.ndarray:
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (3, 3), 0)

    kernel3 = cv2.getStructuringElement(cv2.MORPH_RECT, (3, 3))
    kernel5 = cv2.getStructuringElement(cv2.MORPH_RECT, (5, 5))

    # capture bright and dark thin structures
    tophat = cv2.morphologyEx(blurred, cv2.MORPH_TOPHAT, kernel5)
    blackhat = cv2.morphologyEx(blurred, cv2.MORPH_BLACKHAT, kernel5)
    bandpassed = cv2.max(tophat, blackhat)

    # optional Laplacian to boost faint scratches
    lap = cv2.Laplacian(blurred, cv2.CV_8U, ksize=3)
    bandpassed = cv2.max(bandpassed, lap)

    # dynamic threshold based on sensitivity
    sensitivity = max(0.0, min(sensitivity, 100.0))
    thr_value = np.interp(sensitivity, [0, 100], [80, 18])
    _, mask = cv2.threshold(bandpassed, thr_value, 255, cv2.THRESH_BINARY)

    mask = cv2.medianBlur(mask, 3)
    mask = cv2.dilate(mask, kernel3, iterations=2)
    mask = cv2.morphologyEx(mask, cv2.MORPH_CLOSE, kernel5, iterations=1)

    # light cleanup: ignore extremely small blobs
    num_labels, labels, stats, _ = cv2.connectedComponentsWithStats(mask, connectivity=8)
    cleaned = np.zeros_like(mask)
    for i in range(1, num_labels):
        area = stats[i, cv2.CC_STAT_AREA]
        if area < 8:
            continue
        cleaned[labels == i] = 255

    return cleaned


def _prepare_lama_batch(image_bgr: np.ndarray, mask: np.ndarray, pad_modulo: int, device):
    from saicinpainting.evaluation.data import pad_img_to_modulo
    import torch

    image_rgb = cv2.cvtColor(image_bgr, cv2.COLOR_BGR2RGB).astype("float32") / 255.0
    image_chw = np.transpose(image_rgb, (2, 0, 1))
    mask = (mask.astype("float32") / 255.0)[None, ...]

    h, w = image_chw.shape[1:]
    if pad_modulo and pad_modulo > 1:
        image_chw = pad_img_to_modulo(image_chw, pad_modulo)
        mask = pad_img_to_modulo(mask, pad_modulo)

    batch = {
        "image": torch.from_numpy(image_chw).unsqueeze(0).to(device),
        "mask": torch.from_numpy(mask).unsqueeze(0).to(device),
        "unpad_to_size": (h, w),
    }
    return batch


def inpaint_with_lama(image_bgr: np.ndarray, mask: np.ndarray, device_hint: str = "cpu") -> np.ndarray:
    sys.path.insert(0, str(LAMA_DIR))
    try:
        import torch
        import yaml
        from omegaconf import OmegaConf
        from saicinpainting.evaluation.utils import move_to_device
        from saicinpainting.training.trainers import load_checkpoint
    except Exception as exc:  # noqa: BLE001
        log_warn(f"[WARN] LaMa dependencies unavailable, falling back to OpenCV: {exc}")
        return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

    device = torch.device(device_hint if device_hint == "cuda" and torch.cuda.is_available() else "cpu")

    model_dir = LAMA_DIR / "models" / "big-lama"
    config_path = model_dir / "config.yaml"
    checkpoint_candidates = [
        model_dir / "models" / "best.ckpt",
        model_dir / "best.ckpt",
    ]
    checkpoint_path = next((p for p in checkpoint_candidates if p.exists()), None)

    if not config_path.exists() or checkpoint_path is None:
        log_warn("[WARN] LaMa model files not found; using OpenCV fallback.")
        return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

    with open(config_path, "r") as f:
        train_config = OmegaConf.create(yaml.safe_load(f))

    # disable training-only features
    train_config.training_model.predict_only = True
    train_config.visualizer.kind = "noop"

    try:
        model = load_checkpoint(train_config, str(checkpoint_path), map_location=device, strict=False)
    except Exception as exc:  # noqa: BLE001
        log_warn(f"[WARN] Failed to load LaMa checkpoint ({checkpoint_path}): {exc}")
        return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

    model.freeze()
    model.to(device)
    model.eval()

    batch = _prepare_lama_batch(image_bgr, mask, pad_modulo=8, device=device)

    with torch.no_grad():
        batch = move_to_device(batch, device)
        batch["mask"] = (batch["mask"] > 0.5).float()
        batch = model(batch)
        res = batch.get("inpainted") or batch.get("predicted_image")
        if res is None:
            log_warn("[WARN] LaMa output missing; fallback to OpenCV inpaint.")
            return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

        h, w = batch.get("unpad_to_size", (res.shape[2], res.shape[3]))
        res = res[0, :, :h, :w].permute(1, 2, 0).detach().cpu().numpy()
        res = np.clip(res * 255.0, 0, 255).astype("uint8")
        return cv2.cvtColor(res, cv2.COLOR_RGB2BGR)


def process_image(inp: str, out: str, denoise: float, scratch: float, use_lama: bool, device: str) -> None:
    img = load_image(inp)
    print(f"[INFO] Loaded image: {inp}")

    denoised = remove_grain(img, denoise)
    if denoise > 0:
        print(f"[INFO] Grain removed with strength {denoise:.1f}")

    mask = build_scratch_mask(denoised, scratch)
    non_zero = int(np.count_nonzero(mask))
    print(f"[INFO] Scratch mask pixels: {non_zero}")

    if non_zero < 50:
        print("[INFO] Mask too small; skipping inpaint.")
        result = denoised
    else:
        if use_lama:
            result = inpaint_with_lama(denoised, mask, device_hint=device)
        else:
            result = cv2.inpaint(denoised, mask, 3, cv2.INPAINT_TELEA)

    os.makedirs(os.path.dirname(out) or ".", exist_ok=True)
    cv2.imwrite(out, result)
    print(f"[INFO] Saved: {out}")


def main() -> None:
    args = parse_args()
    process_image(
        inp=args.input,
        out=args.output,
        denoise=args.denoise,
        scratch=args.scratch,
        use_lama=not args.no_lama,
        device=args.device,
    )


if __name__ == "__main__":
    main()
