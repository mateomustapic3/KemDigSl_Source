import sys
import os

try:
    from ultralytics import YOLO
except Exception as e:  # pragma: no cover - runtime guard
    sys.stderr.write(f"Failed to import ultralytics.YOLO: {e}\n")
    sys.exit(1)

def main():
    if len(sys.argv) < 4:
        print("Usage: python detect_yolo.py <image_path> <output_txt> <conf>", file=sys.stderr)
        sys.exit(1)

    image_path = sys.argv[1]
    output_txt = sys.argv[2]
    conf = float(sys.argv[3])

    script_dir = os.path.dirname(os.path.abspath(__file__))
    model_path = os.path.join(script_dir, "models", "yolov8n.pt")

    if not os.path.exists(image_path):
        print(f"Input image not found: {image_path}", file=sys.stderr)
        sys.exit(1)

    if not os.path.exists(model_path):
        print(f"Model not found at {model_path}", file=sys.stderr)
        sys.exit(1)

    try:
        model = YOLO(model_path)
    except Exception as e:
        print(f"Failed to load model: {e}", file=sys.stderr)
        sys.exit(1)

    try:
        results = model(
            image_path,
            conf=conf,
            verbose=False,
            device="cuda:0" if os.environ.get("CUDA_VISIBLE_DEVICES") else "cpu",
        )[0]
    except Exception as e:
        print(f"Inference failed: {e}", file=sys.stderr)
        sys.exit(1)

    lines = []
    for box in results.boxes:
        cls_id = int(box.cls[0].item())
        label = model.names.get(cls_id, f"class_{cls_id}")
        score = float(box.conf[0].item())
        x1, y1, x2, y2 = box.xyxy[0].tolist()
        line = f"{label};{score:.4f};{x1:.2f};{y1:.2f};{x2:.2f};{y2:.2f}"
        lines.append(line)

    out_dir = os.path.dirname(output_txt)
    if out_dir and not os.path.exists(out_dir):
        os.makedirs(out_dir, exist_ok=True)

    with open(output_txt, "w", encoding="utf-8") as f:
        for line in lines:
            f.write(line + "\n")

if __name__ == "__main__":
    main()
