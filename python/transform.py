import sys
import base64
from io import BytesIO
from PIL import Image, ImageOps, ImageFilter, ImageEnhance


def transform(image_path: str, operation: str, param: float | None = None):
    img = Image.open(image_path)

    if operation == "invert":
        img = ImageOps.invert(img.convert("RGB"))
    elif operation == "grayscale":
        img = ImageOps.grayscale(img)
    elif operation == "blur":
        radius = param if param is not None else 2.0
        img = img.filter(ImageFilter.GaussianBlur(radius))
    elif operation == "rotate_90":
        img = img.rotate(90, expand=True)
    elif operation == "rotate_custom":
        angle = param if param is not None else 0
        img = img.rotate(angle, expand=True)
    elif operation == "mirror":
        img = ImageOps.mirror(img)
    elif operation == "flip":
        img = ImageOps.flip(img)
    elif operation == "sepia":
        sepia = Image.open(image_path).convert("RGB")
        w, h = sepia.size
        pixels = sepia.load()
        for y in range(h):
            for x in range(w):
                r, g, b = pixels[x, y]
                tr = int(0.393 * r + 0.769 * g + 0.189 * b)
                tg = int(0.349 * r + 0.686 * g + 0.168 * b)
                tb = int(0.272 * r + 0.534 * g + 0.131 * b)
                pixels[x, y] = (min(tr, 255), min(tg, 255), min(tb, 255))
        img = sepia
    elif operation == "brightness":
        factor = param if param is not None else 1.0
        img = img.convert("RGB")
        lut = [
            max(0, min(255, int(p * factor)))
            for p in range(256)
        ]
        img = img.point(lut * 3)
    elif operation == "contrast":
        factor = param if param is not None else 1.0
        img = img.convert("RGB")
        lut = [
            max(0, min(255, int(128 + (p - 128) * factor)))
            for p in range(256)
        ]
        img = img.point(lut * 3)
    elif operation == "saturation":
        factor = param if param is not None else 1.0
        img = ImageEnhance.Color(img).enhance(factor)
    elif operation == "sharpness":
        factor = param if param is not None else 1.0
        img = ImageEnhance.Sharpness(img).enhance(factor)

    buffer = BytesIO()
    img.save(buffer, format="PNG")
    encoded = base64.b64encode(buffer.getvalue()).decode("utf-8")
    print(encoded)


if __name__ == "__main__":
    input_image = sys.argv[1]
    operation = sys.argv[2]
    param = float(sys.argv[3]) if len(sys.argv) > 3 else None
    transform(input_image, operation, param)
