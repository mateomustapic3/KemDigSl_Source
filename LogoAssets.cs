using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Project
{
    internal static class LogoAssets
    {
        private const int MiniLogoMaxSize = 44;
        private const int MiniLogoMargin = 8;

        private static readonly byte[]? MainLogoBytes = LoadBytes("images", "mainLogo.png");
        private static readonly byte[]? MiniLogoBytes = LoadBytes("images", "miniLogo.png");
        private static readonly Size? MainLogoSize = GetImageSize(MainLogoBytes);
        private static readonly Size? MiniLogoSize = GetImageSize(MiniLogoBytes);

        internal static Image? CreateMainLogo()
        {
            return CreateImage(MainLogoBytes);
        }

        internal static Image? CreateMiniLogo()
        {
            return CreateImage(MiniLogoBytes);
        }

        internal static Size GetMainLogoFitSize(int maxWidth, int maxHeight)
        {
            return FitSize(MainLogoSize, maxWidth, maxHeight);
        }

        internal static void AttachMiniLogo(Panel host)
        {
            if (host == null)
                return;

            var image = CreateMiniLogo();
            if (image == null)
                return;

            var picture = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                TabStop = false
            };

            host.Controls.Add(picture);
            picture.BringToFront();

            void Position()
            {
                int maxSide = Math.Min(MiniLogoMaxSize, Math.Max(1, host.ClientSize.Height));
                Size size = FitSize(MiniLogoSize, maxSide, maxSide);
                picture.Size = size;

                int rightMargin = Math.Max(MiniLogoMargin, host.Padding.Right);
                int x = host.ClientSize.Width - picture.Width - rightMargin;
                int y = Math.Max(0, (host.ClientSize.Height - picture.Height) / 2);
                picture.Location = new Point(Math.Max(0, x), y);
            }

            Position();
            host.Resize += (_, __) => Position();
        }

        private static byte[]? LoadBytes(params string[] relativeParts)
        {
            try
            {
                string path = AppPaths.ResolveFile(relativeParts);
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    return null;

                return File.ReadAllBytes(path);
            }
            catch
            {
                return null;
            }
        }

        private static Image? CreateImage(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using var ms = new MemoryStream(bytes);
            using var img = Image.FromStream(ms);
            return new Bitmap(img);
        }

        private static Size? GetImageSize(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            try
            {
                using var ms = new MemoryStream(bytes);
                using var img = Image.FromStream(ms);
                return img.Size;
            }
            catch
            {
                return null;
            }
        }

        private static Size FitSize(Size? original, int maxWidth, int maxHeight)
        {
            if (original == null || maxWidth <= 0 || maxHeight <= 0)
                return new Size(Math.Max(1, maxWidth), Math.Max(1, maxHeight));

            double ratio = Math.Min(maxWidth / (double)original.Value.Width, maxHeight / (double)original.Value.Height);
            ratio = Math.Min(ratio, 1.0);

            int width = Math.Max(1, (int)Math.Round(original.Value.Width * ratio));
            int height = Math.Max(1, (int)Math.Round(original.Value.Height * ratio));
            return new Size(width, height);
        }
    }
}
