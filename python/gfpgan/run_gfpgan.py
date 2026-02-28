import os
import sys
import cv2
import torch
import argparse

from gfpgan import GFPGANer  # dolazi iz GFPGAN repo-a


def main():
    parser = argparse.ArgumentParser(description="GFPGAN wrapper za WinForms app.")
    parser.add_argument("input_path", help="Ulazna slika")
    parser.add_argument("output_path", help="Izlazna slika")
    parser.add_argument(
        "--upscale",
        type=int,
        default=2,
        choices=[1, 2, 4],
        help="Faktor povećanja (1, 2 ili 4)",
    )
    parser.add_argument(
        "--only-center-face",
        action="store_true",
        help="Obrađuj samo centralno lice",
    )
    parser.add_argument(
        "--no-paste-back",
        action="store_true",
        help="Nemoj lijepiti lice natrag u originalnu sliku",
    )

    args = parser.parse_args()

    input_path = args.input_path
    output_path = args.output_path

    if not os.path.exists(input_path):
        print(f"Input file does not exist: {input_path}", file=sys.stderr)
        sys.exit(2)

    # putanja do modela (prilagodi po potrebi)
    script_dir = os.path.dirname(os.path.abspath(__file__))
    model_path = os.path.join(
        script_dir, "experiments", "pretrained_models", "GFPGANv1.4.pth"
    )

    if not os.path.exists(model_path):
        print(f"Model not found: {model_path}", file=sys.stderr)
        sys.exit(3)

    # info o uređaju
    device = "cuda" if torch.cuda.is_available() else "cpu"
    print(f"Using device: {device}")

    restorer = GFPGANer(
        model_path=model_path,
        upscale=args.upscale,
        arch="clean",
        channel_multiplier=2,
        bg_upsampler=None,  # kasnije se može spojiti s Real-ESRGAN
    )

    img = cv2.imread(input_path, cv2.IMREAD_COLOR)
    if img is None:
        print(f"Failed to read image: {input_path}", file=sys.stderr)
        sys.exit(4)

    try:
        _, restored_faces, restored_img = restorer.enhance(
            img,
            has_aligned=False,
            only_center_face=args.only_center_face,
            paste_back=not args.no_paste_back,
        )
    except Exception as e:
        print(f"GFPGAN error: {e}", file=sys.stderr)
        sys.exit(5)

    # When paste_back is disabled, GFPGAN returns faces but restored_img is None.
    # Also guard against failed detection (no faces).
    if restored_img is None or restored_img.size == 0:
        if restored_faces and len(restored_faces) > 0:
            restored_img = restored_faces[0]
        else:
            print(
                "GFPGAN returned empty result (no faces detected or paste_back off).",
                file=sys.stderr,
            )
            sys.exit(6)

    # spremi izlaz – radi i kad nema folder u pathu
    output_dir = os.path.dirname(output_path)
    if output_dir and not os.path.exists(output_dir):
        os.makedirs(output_dir, exist_ok=True)

    cv2.imwrite(output_path, restored_img)

    print("OK")
    sys.exit(0)


if __name__ == "__main__":
    main()
