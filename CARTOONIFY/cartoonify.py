import os
import sys
import subprocess
import shutil
import tempfile
import argparse
import cv2
import numpy as np


def log(msg: str) -> None:
    print("[LOG] " + str(msg))


# ---------------------------------------------------------
#  AnimeGAN v2 + optional denoise + blend
# ---------------------------------------------------------
def run_animegan_v2(input_path: str, output_path: str, style: str, blend: str, denoise: str) -> None:
    """
    Pokreće AnimeGAN v2 nad ulaznom slikom i sprema rezultat.

    Parametri:
    - style: v1|v2|paprika|celeba (odabir težina)
    - blend: 0..1 (1 = potpuno stilizirano, 0 = original)
    - denoise: 0..1 (0 = bez denoise-a, 1 = jači denoise prije modela)

    Napomena: `blend` i `denoise` dolaze iz CLI argumenata pa se parsiraju u float.
    """
    log(f"Style: {style}, Blend: {blend}, Denoise: {denoise}")

    style_map = {
        "v1": "face_paint_512_v1.pt",
        "v2": "face_paint_512_v2.pt",
        "paprika": "paprika.pt",
        "celeba": "celeba_distill.pt",
    }

    if style not in style_map:
        log(f"Unknown style '{style}', defaulting to v2.")
        style = "v2"

    model_name = style_map[style]

    base_dir = os.path.dirname(os.path.abspath(__file__))
    model_dir = os.path.join(base_dir, "animegan_v2")
    weights_path = os.path.join(model_dir, "weights", model_name)
    test_script = os.path.join(model_dir, "test.py")

    if not os.path.exists(weights_path):
        raise FileNotFoundError(f"Model weights not found: {weights_path}")
    if not os.path.exists(test_script):
        raise FileNotFoundError(f"test.py not found at {test_script}")

    temp_output = output_path + "_temp.png"
    temp_denoised = output_path + "_denoise.png"

    # ---------------------------------------------------------
    # STEP 1: Denoise (optional)
    # ---------------------------------------------------------
    img = cv2.imread(input_path)
    if img is None:
        raise FileNotFoundError(f"Could not read input image: {input_path}")

    try:
        denoise_val = float(denoise)
    except (TypeError, ValueError) as exc:
        raise ValueError(f"Invalid --denoise value: {denoise!r} (expected 0..1)") from exc

    denoise_val = max(0.0, min(1.0, denoise_val))
    h_val = denoise_val * 20  # convert to 0-20 strength

    if denoise_val > 0.01:
        log("Applying denoise...")
        denoised = cv2.fastNlMeansDenoisingColored(img, None, h_val, h_val, 7, 21)
        cv2.imwrite(temp_denoised, denoised)
        input_for_model = temp_denoised
    else:
        input_for_model = input_path

    # ---------------------------------------------------------
    # STEP 2: AnimeGAN v2 process
    # ---------------------------------------------------------
    # prepare temp input/output dirs for test.py (expects directories)
    work_root = tempfile.mkdtemp(prefix="animegan_", dir=os.path.dirname(output_path))
    temp_input_dir = os.path.join(work_root, "in")
    temp_output_dir = os.path.join(work_root, "out")
    os.makedirs(temp_input_dir, exist_ok=True)
    os.makedirs(temp_output_dir, exist_ok=True)

    temp_input_name = os.path.basename(input_path)
    temp_input_path = os.path.join(temp_input_dir, temp_input_name)
    shutil.copy(input_for_model, temp_input_path)

    cmd = [
        sys.executable,
        test_script,
        "--checkpoint",
        weights_path,
        "--input_dir",
        temp_input_dir,
        "--output_dir",
        temp_output_dir,
        "--device",
        "cpu",
        "--x32",
    ]

    log("Running AnimeGAN v2 model...")
    env = os.environ.copy()
    env["PYTHONPATH"] = model_dir + os.pathsep + env.get("PYTHONPATH", "")
    result = subprocess.run(
        cmd,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True,
        cwd=model_dir,
        env=env,
    )

    if result.stdout:
        log(result.stdout)
    if result.stderr:
        log(result.stderr)

    # ---------------------------------------------------------
    # STEP 3: Blend original and stylized image
    # ---------------------------------------------------------
    # locate stylized file
    stylized_files = [f for f in os.listdir(temp_output_dir) if f.lower().endswith((".png", ".jpg", ".jpeg", ".bmp"))]
    if not stylized_files:
        raise FileNotFoundError(f"Stylized output not produced in {temp_output_dir}")
    temp_output = os.path.join(temp_output_dir, stylized_files[0])

    log("Blending result...")

    original = cv2.imread(input_path)
    stylized = cv2.imread(temp_output)

    if original is None:
        raise FileNotFoundError(f"Could not read original image: {input_path}")
    if stylized is None:
        raise FileNotFoundError(f"Could not read stylized image: {temp_output}")

    # Ensure same size for blending (test.py may resize to x32)
    if stylized.shape[:2] != original.shape[:2]:
        stylized = cv2.resize(stylized, (original.shape[1], original.shape[0]), interpolation=cv2.INTER_CUBIC)

    original = original.astype(np.float32)
    stylized = stylized.astype(np.float32)

    try:
        alpha = float(blend)
    except (TypeError, ValueError) as exc:
        raise ValueError(f"Invalid --blend value: {blend!r} (expected 0..1)") from exc

    alpha = max(0.0, min(alpha, 1.0))

    blended = alpha * stylized + (1 - alpha) * original
    blended = np.clip(blended, 0, 255).astype(np.uint8)

    cv2.imwrite(output_path, blended)

    # Clean-up
    for f in (temp_output, temp_denoised):
        try:
            os.remove(f)
        except OSError:
            pass
    try:
        shutil.rmtree(work_root, ignore_errors=True)
    except OSError:
        pass

    log("Finished.")


# ---------------------------------------------------------
# MAIN ARGS
# ---------------------------------------------------------
def main() -> None:
    parser = argparse.ArgumentParser()

    parser.add_argument("--input", required=True)
    parser.add_argument("--output", required=True)
    parser.add_argument("--style", default="v2")
    parser.add_argument("--blend", default="1.0")
    parser.add_argument("--denoise", default="0.0")

    args = parser.parse_args()

    run_animegan_v2(
        input_path=args.input,
        output_path=args.output,
        style=args.style,
        blend=args.blend,
        denoise=args.denoise,
    )


if __name__ == "__main__":
    main()
