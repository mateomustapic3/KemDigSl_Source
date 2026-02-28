"""
Object removal using LaMa.

Usage:
python run_object_removal.py --input img.png --mask mask.png --output cleaned.png [--device cpu|cuda] [--no-lama]

Mask: white pixels mark regions to remove.
"""

import argparse
import os
import sys
from pathlib import Path

import cv2
import numpy as np

ROOT = Path(__file__).resolve().parent
LAMA_DIR = ROOT / "lama"


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Object removal with LaMa inpainting")
    parser.add_argument("--input", required=True, help="Input image path")
    parser.add_argument("--mask", required=True, help="Binary mask path (white = remove)")
    parser.add_argument("--output", required=True, help="Output image path")
    parser.add_argument("--device", default="cpu", choices=["cpu", "cuda"], help="Device hint")
    parser.add_argument("--no-lama", action="store_true", help="Force OpenCV Telea fallback")
    return parser.parse_args()


def load_image(path: str) -> np.ndarray:
    img = cv2.imread(path, cv2.IMREAD_COLOR)
    if img is None:
        raise FileNotFoundError(f"Cannot read image: {path}")
    return img


def load_mask(path: str, shape) -> np.ndarray:
    mask = cv2.imread(path, cv2.IMREAD_GRAYSCALE)
    if mask is None:
        raise FileNotFoundError(f"Cannot read mask: {path}")
    mask = cv2.resize(mask, (shape[1], shape[0]), interpolation=cv2.INTER_NEAREST)
    _, mask_bin = cv2.threshold(mask, 127, 255, cv2.THRESH_BINARY)
    return mask_bin


def inpaint_with_lama(image_bgr: np.ndarray, mask: np.ndarray, device_hint: str = "cpu") -> np.ndarray:
    sys.path.insert(0, str(LAMA_DIR))
    try:
        import torch
        import yaml
        from omegaconf import OmegaConf
        from saicinpainting.evaluation.utils import move_to_device
        from saicinpainting.training.trainers import load_checkpoint
        from saicinpainting.evaluation.data import pad_img_to_modulo
    except Exception as exc:  # noqa: BLE001
        print(f"[WARN] LaMa dependencies unavailable, fallback to OpenCV: {exc}")
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
        print("[WARN] LaMa model files missing; using OpenCV fallback.")
        return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

    with open(config_path, "r") as f:
        train_config = OmegaConf.create(yaml.safe_load(f))
    train_config.training_model.predict_only = True
    train_config.visualizer.kind = "noop"

    try:
        model = load_checkpoint(train_config, str(checkpoint_path), map_location=device, strict=False)
    except Exception as exc:  # noqa: BLE001
        print(f"[WARN] Failed to load LaMa checkpoint: {exc}")
        return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

    model.freeze()
    model.to(device)
    model.eval()

    image_rgb = cv2.cvtColor(image_bgr, cv2.COLOR_BGR2RGB).astype("float32") / 255.0
    image_chw = np.transpose(image_rgb, (2, 0, 1))
    mask_f = (mask.astype("float32") / 255.0)[None, ...]

    h, w = image_chw.shape[1:]
    pad_mod = 8
    image_chw = pad_img_to_modulo(image_chw, pad_mod)
    mask_f = pad_img_to_modulo(mask_f, pad_mod)

    import torch

    batch = {
        "image": torch.from_numpy(image_chw).unsqueeze(0).to(device),
        "mask": torch.from_numpy(mask_f).unsqueeze(0).to(device),
        "unpad_to_size": (h, w),
    }

    with torch.no_grad():
        batch = move_to_device(batch, device)
        batch["mask"] = (batch["mask"] > 0.5).float()
        batch = model(batch)
        res = batch.get("inpainted") or batch.get("predicted_image")
        if res is None:
            print("[WARN] LaMa output missing; using OpenCV fallback.")
            return cv2.inpaint(image_bgr, mask, 3, cv2.INPAINT_TELEA)

        uh, uw = batch.get("unpad_to_size", (res.shape[2], res.shape[3]))
        res = res[0, :, :uh, :uw].permute(1, 2, 0).detach().cpu().numpy()
        res = np.clip(res * 255.0, 0, 255).astype("uint8")
        return cv2.cvtColor(res, cv2.COLOR_RGB2BGR)


def main() -> None:
    args = parse_args()
    img = load_image(args.input)
    mask = load_mask(args.mask, img.shape[:2])
    non_zero = int(np.count_nonzero(mask))
    if non_zero < 10:
        print("[WARN] Mask is empty; skipping inpaint.")
        out_img = img
    else:
        if args.no_lama:
            out_img = cv2.inpaint(img, mask, 3, cv2.INPAINT_TELEA)
        else:
            out_img = inpaint_with_lama(img, mask, device_hint=args.device)

    os.makedirs(os.path.dirname(args.output) or ".", exist_ok=True)
    cv2.imwrite(args.output, out_img)
    print(f"[INFO] Saved: {args.output}")


if __name__ == "__main__":
    main()
