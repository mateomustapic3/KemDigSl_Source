import argparse
import os
import sys

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
if SCRIPT_DIR not in sys.path:
    sys.path.insert(0, SCRIPT_DIR)

from cartoonify import run_animegan_v2


def main():
    parser = argparse.ArgumentParser(description="Run AnimeGAN v2 cartoonify")
    parser.add_argument("--input", required=True, help="Input image path")
    parser.add_argument("--output", required=True, help="Output image path")
    parser.add_argument("--style", default="v2", choices=["v1", "v2", "paprika", "celeba"], help="Model style")
    parser.add_argument("--blend", default="1.0", help="Blend ratio (0-1, 1 = full cartoon)")
    parser.add_argument("--denoise", default="0.0", help="Denoise strength (0-1)")
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
