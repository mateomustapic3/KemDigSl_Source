import torch
import torch.nn as nn
from PIL import Image
import torchvision.transforms as transforms
import argparse
import os
import sys

ROOT = os.path.dirname(os.path.abspath(__file__))
if ROOT not in sys.path:
    sys.path.insert(0, ROOT)

from net import decoder, vgg
from function import adaptive_instance_normalization


def test_transform(size):
    # Resize shorter side to given size (0 = keep original), keep aspect ratio.
    return transforms.Compose([
        transforms.Resize(size) if size > 0 else transforms.Lambda(lambda x: x),
        transforms.ToTensor()
    ])


def load_image(path, tfm):
    img = Image.open(path).convert('RGB')
    return tfm(img).unsqueeze(0)


def to_image(tensor):
    tensor = tensor.squeeze().cpu().clamp(0, 1)
    transform = transforms.ToPILImage()
    return transform(tensor)

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--content", required=True)
    parser.add_argument("--style", required=True)
    parser.add_argument("--alpha", type=float, default=1.0)
    parser.add_argument("--output", required=True)
    parser.add_argument("--modelpath", required=True)
    parser.add_argument("--content_size", type=int, default=512,
                        help="Resize shorter side of content (0 = no resize)")
    parser.add_argument("--style_size", type=int, default=512,
                        help="Resize shorter side of style (0 = no resize)")
    args = parser.parse_args()

    print(f"[AdaIN] Ucitavanje slika: content={args.content}, style={args.style}", flush=True)
    content_tf = test_transform(args.content_size)
    style_tf = test_transform(args.style_size)
    content = load_image(args.content, content_tf)
    style = load_image(args.style, style_tf)

    device = torch.device("cpu")
    print(f"[AdaIN] Device: {device}", flush=True)
    content = content.to(device)
    style = style.to(device)

    # Load models
    decoder_path = os.path.join(args.modelpath, "decoder.pth")
    vgg_path = os.path.join(args.modelpath, "vgg_normalised.pth")
    print(f"[AdaIN] Ucitavanje modela iz {args.modelpath}", flush=True)

    dec = decoder
    vgg_net = vgg

    vgg_net.load_state_dict(torch.load(vgg_path, map_location=device))
    dec.load_state_dict(torch.load(decoder_path, map_location=device))
    # Use VGG up to relu4_1 (layer 31) as in official AdaIN inference to avoid artifacts
    vgg_net = nn.Sequential(*list(vgg_net.children())[:31])

    # Freeze
    vgg_net.eval()
    dec.eval()

    # Extract features
    print("[AdaIN] Ekstrakcija feature-a...", flush=True)
    content_f = vgg_net(content)
    style_f = vgg_net(style)

    # AdaIN
    print(f"[AdaIN] Primjena AdaIN (alpha={args.alpha})...", flush=True)
    t = adaptive_instance_normalization(content_f, style_f)
    t = args.alpha * t + (1 - args.alpha) * content_f

    # Decode
    print("[AdaIN] Dekodiranje...", flush=True)
    output = dec(t)

    print(f"[AdaIN] Spremanje u {args.output}", flush=True)
    out_img = to_image(output)
    out_img.save(args.output)
    print("[AdaIN] Gotovo.", flush=True)

if __name__ == "__main__":
    main()
