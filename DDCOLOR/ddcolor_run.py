import os
import sys
import torch
import json
import cv2
import numpy as np
import torch.nn.functional as F
from PIL import Image
import torchvision.transforms as transforms

# Ensure we use the local basicsr/ddcolor modules shipped with the app
BASE = os.path.dirname(os.path.abspath(__file__))
LOCAL_BASICS = os.path.join(BASE, "basicsr")
sys.path.insert(0, os.path.join(LOCAL_BASICS, "archs"))
sys.path.insert(0, LOCAL_BASICS)
sys.path.insert(0, BASE)

from ddcolor_model import DDColor

def load_image(image_path):
    img = Image.open(image_path).convert("RGB")
    return img


def save_image(tensor, out_path):
    img = transforms.ToPILImage()(tensor.squeeze().cpu().clamp(0, 1))
    img.save(out_path)


def main():
    if len(sys.argv) < 3:
        print("Usage: python ddcolor_run.py input.jpg output.jpg [strength 0-1]")
        return

    input_path = sys.argv[1]
    output_path = sys.argv[2]

    strength = 1.0
    if len(sys.argv) >= 4:
        try:
            strength = float(sys.argv[3])
        except ValueError:
            print(f"Invalid strength value '{sys.argv[3]}', using default 1.0")
            strength = 1.0

    strength = max(0.0, min(1.0, strength))

    pretrain_dir = os.path.join(BASE, "pretrain")
    model_path = os.path.join(pretrain_dir, "pytorch_model.bin")
    config_path = os.path.join(pretrain_dir, "config.json")

    if not os.path.exists(model_path):
        print(f"Model weights not found: {model_path}")
        return
    if not os.path.exists(config_path):
        print(f"Config file not found: {config_path}")
        return

    with open(config_path, "r", encoding="utf-8") as f:
        cfg = json.load(f)

    input_size = cfg.get("input_size", [512, 512])
    if isinstance(input_size, (list, tuple)) and len(input_size) == 2:
        input_size_tuple = (int(input_size[0]), int(input_size[1]))
    else:
        input_size_tuple = (512, 512)

    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

    model = DDColor(
        encoder_name=cfg.get("encoder_name", "convnext-l"),
        decoder_name=cfg.get("decoder_name", "MultiScaleColorDecoder"),
        input_size=input_size_tuple,
        num_output_channels=cfg.get("num_output_channels", 2),
        last_norm=cfg.get("last_norm", "Spectral"),
        do_normalize=cfg.get("do_normalize", False),
        num_queries=cfg.get("num_queries", 100),
        num_scales=cfg.get("num_scales", 3),
        dec_layers=cfg.get("dec_layers", 9),
    ).to(device)

    state = torch.load(model_path, map_location="cpu")
    state_dict = state["params"] if "params" in state else state
    model.load_state_dict(state_dict, strict=False)
    model.eval()

    img_bgr = cv2.imread(input_path, cv2.IMREAD_COLOR)
    if img_bgr is None:
        print(f"Could not read input image: {input_path}")
        return

    height, width = img_bgr.shape[:2]
    img_float = img_bgr.astype(np.float32) / 255.0
    orig_l = cv2.cvtColor(img_float, cv2.COLOR_BGR2Lab)[:, :, :1]

    resized = cv2.resize(img_float, input_size_tuple)
    img_l = cv2.cvtColor(resized, cv2.COLOR_BGR2Lab)[:, :, :1]
    img_gray_lab = np.concatenate((img_l, np.zeros_like(img_l), np.zeros_like(img_l)), axis=-1)
    img_gray_rgb = cv2.cvtColor(img_gray_lab, cv2.COLOR_LAB2RGB)

    tensor_gray_rgb = torch.from_numpy(img_gray_rgb.transpose((2, 0, 1))).float().unsqueeze(0).to(device)

    with torch.no_grad():
        output_ab = model(tensor_gray_rgb).cpu()

    output_ab_resized = F.interpolate(output_ab, size=(height, width), mode="bilinear", align_corners=False)[0].float().numpy().transpose(1, 2, 0)
    output_lab = np.concatenate((orig_l, output_ab_resized), axis=-1)
    output_bgr = cv2.cvtColor(output_lab, cv2.COLOR_LAB2BGR)

    gray_lab_full = np.concatenate((orig_l, np.zeros_like(orig_l), np.zeros_like(orig_l)), axis=-1)
    gray_bgr = cv2.cvtColor(gray_lab_full, cv2.COLOR_LAB2BGR)

    blended_bgr = gray_bgr * (1.0 - strength) + output_bgr * strength
    output_img = np.clip(blended_bgr * 255.0, 0, 255).round().astype(np.uint8)

    os.makedirs(os.path.dirname(output_path) or ".", exist_ok=True)
    cv2.imwrite(output_path, output_img)

    print("OK")


if __name__ == "__main__":
    main()
