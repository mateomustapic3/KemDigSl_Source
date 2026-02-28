using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form10 : Form
    {
        private Bitmap? originalImage;
        private Bitmap? previewImage;
        private Bitmap? maskImage;
        private bool isDrawing;
        private Point lastPoint;
        private int brushSize = 18;
        private string inputPath = string.Empty;
        private string outputImagePath = string.Empty;
        private bool isProcessing;
        private bool outputZoomed;
        private Form? outputZoomForm;
        private PictureBox? outputZoomPicture;
        private Label? overlayLabel;

        public Form10()
        {
            InitializeComponent();
            ApplyTheme();
            trackBrush.Value = brushSize;
            lblBrushValue.Text = $"{brushSize}px";
            btnRun.Enabled = false;
            btnSave.Enabled = false;
            lblStatus.Text = "Status: spreman";
            InitOverlay();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputPath = ofd.FileName;
                LoadImage(inputPath);
                txtLog.AppendText($"Loaded: {inputPath}{Environment.NewLine}");
                lblStatus.Text = "Nacrtaj masku (crveno).";
                btnRun.Enabled = true;
                btnSave.Enabled = false;
            }
        }

        private void LoadImage(string path)
        {
            originalImage?.Dispose();
            previewImage?.Dispose();
            maskImage?.Dispose();

            originalImage = new Bitmap(LoadUnlockedImage(path));
            maskImage = new Bitmap(originalImage.Width, originalImage.Height);
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (originalImage == null || maskImage == null)
                return;

            previewImage?.Dispose();
            previewImage = new Bitmap(originalImage.Width, originalImage.Height);
            using (var g = Graphics.FromImage(previewImage))
            {
                g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);

                // Fast overlay: tint mask red with transparency via ColorMatrix (avoids per-pixel loops).
                using var ia = new System.Drawing.Imaging.ImageAttributes();
                var cm = new System.Drawing.Imaging.ColorMatrix(new[]
                {
                    new float[] {1f, 0f, 0f, 0f, 0f}, // R from mask
                    new float[] {0f, 0f, 0f, 0f, 0f}, // G zeroed
                    new float[] {0f, 0f, 0f, 0f, 0f}, // B zeroed
                    new float[] {0f, 0f, 0f, 0.45f, 0f}, // alpha scaled
                    new float[] {0f, 0f, 0f, 0f, 0f},
                });
                ia.SetColorMatrix(cm);
                var destRect = new Rectangle(0, 0, maskImage.Width, maskImage.Height);
                g.DrawImage(maskImage, destRect, 0, 0, maskImage.Width, maskImage.Height, GraphicsUnit.Pixel, ia);
            }

            picImage.Image?.Dispose();
            picImage.Image = new Bitmap(previewImage);
        }

        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (originalImage == null || maskImage == null)
                return;
            isDrawing = true;
            lastPoint = e.Location;
            DrawAt(e.Location);
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || originalImage == null || maskImage == null)
                return;
            DrawAt(e.Location);
            lastPoint = e.Location;
        }

        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void DrawAt(Point mousePoint)
        {
            if (maskImage == null || originalImage == null || picImage.Image == null)
                return;

            var imgRect = GetImageDisplayRectangle(picImage, originalImage.Size);
            if (!imgRect.Contains(mousePoint))
                return; // cursor outside the drawn image

            // Map mouse (control space) to image space accounting for zoom/letterboxing.
            float relX = (mousePoint.X - imgRect.X) / imgRect.Width;
            float relY = (mousePoint.Y - imgRect.Y) / imgRect.Height;
            int cx = (int)(relX * maskImage.Width);
            int cy = (int)(relY * maskImage.Height);

            using (var g = Graphics.FromImage(maskImage))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, cx - brushSize / 2, cy - brushSize / 2, brushSize, brushSize);
            }
            UpdatePreview();
        }

        /// <summary>
        /// Returns the rectangle (in control coordinates) where the image is drawn when SizeMode=Zoom.
        /// </summary>
        private static RectangleF GetImageDisplayRectangle(PictureBox pb, Size imgSize)
        {
            if (pb.Width == 0 || pb.Height == 0 || imgSize.Width == 0 || imgSize.Height == 0)
                return RectangleF.Empty;

            float imageAspect = (float)imgSize.Width / imgSize.Height;
            float boxAspect = (float)pb.Width / pb.Height;

            float drawWidth, drawHeight;
            if (boxAspect > imageAspect)
            {
                drawHeight = pb.Height;
                drawWidth = imageAspect * drawHeight;
            }
            else
            {
                drawWidth = pb.Width;
                drawHeight = drawWidth / imageAspect;
            }

            float offsetX = (pb.Width - drawWidth) / 2f;
            float offsetY = (pb.Height - drawHeight) / 2f;
            return new RectangleF(offsetX, offsetY, drawWidth, drawHeight);
        }

        private void btnClearMask_Click(object sender, EventArgs e)
        {
            if (maskImage == null || originalImage == null)
                return;
            using (var g = Graphics.FromImage(maskImage))
            {
                g.Clear(Color.Black);
            }
            UpdatePreview();
            lblStatus.Text = "Maska obrisana.";
        }

        private void trackBrush_Scroll(object sender, EventArgs e)
        {
            brushSize = trackBrush.Value;
            lblBrushValue.Text = $"{brushSize}px";
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (originalImage == null || maskImage == null || string.IsNullOrEmpty(inputPath))
            {
                MessageBox.Show("Prvo učitaj sliku i nacrtaj masku.");
                return;
            }

            if (isProcessing)
                return;

            string pythonExe = GetPythonExe();
            if (!File.Exists(pythonExe))
            {
                MessageBox.Show("Python runtime nije pronađen.");
                return;
            }

            string scriptPath = ResolvePath("BCKG_REMOVAL", "run_object_removal.py");
            if (string.IsNullOrWhiteSpace(scriptPath) || !File.Exists(scriptPath))
            {
                MessageBox.Show("run_object_removal.py nije pronađen.");
                return;
            }

            string scriptDir = Path.GetDirectoryName(scriptPath)!;
            string outputDir = Path.Combine(scriptDir, "outputs");
            Directory.CreateDirectory(outputDir);
            outputImagePath = Path.Combine(outputDir, $"object_removed_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            // save temp mask
            string tempMask = Path.Combine(Path.GetTempPath(), $"objmask_{Guid.NewGuid():N}.png");
            maskImage.Save(tempMask);

            lblStatus.Text = "Pokrećem LaMa...";
            progressBar.Style = ProgressBarStyle.Marquee;
            btnRun.Enabled = false;
            btnSave.Enabled = false;
            ShowOverlay(true);

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{scriptPath}\" --input \"{inputPath}\" --mask \"{tempMask}\" --output \"{outputImagePath}\" --device cpu",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = scriptDir
            };

            var outputBuffer = new StringBuilder();

            isProcessing = true;
            ToggleUi(false);

            await Task.Run(() =>
            {
                using var proc = new Process { StartInfo = psi };
                proc.OutputDataReceived += (_, ea) =>
                {
                    if (ea.Data != null)
                    {
                        lock (outputBuffer)
                        {
                            outputBuffer.AppendLine(ea.Data);
                        }
                    }
                };
                proc.ErrorDataReceived += (_, ea) =>
                {
                    if (ea.Data != null)
                    {
                        lock (outputBuffer)
                        {
                            outputBuffer.AppendLine("ERR: " + ea.Data);
                        }
                    }
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            });

            progressBar.Style = ProgressBarStyle.Blocks;
            if (outputBuffer.Length > 0)
                txtLog.AppendText(outputBuffer.ToString());

            if (File.Exists(outputImagePath))
            {
                picOutput.Image?.Dispose();
                picOutput.Image = LoadUnlockedImage(outputImagePath);
                lblStatus.Text = "Gotovo.";
                btnSave.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Output nije pronađen.";
            }

            ToggleUi(true);
            isProcessing = false;
            ShowOverlay(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (picOutput.Image == null)
            {
                MessageBox.Show("Nema output slike.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"object_removed_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                picOutput.Image.Save(sfd.FileName);
                lblStatus.Text = "Saved.";
                txtLog.AppendText("Slika spremljena.\r\n");
            }
        }

        private void picOutput_Click(object sender, EventArgs e)
        {
            if (picOutput.Image == null)
                return;

            if (!outputZoomed)
            {
                // Pop-out fullscreen viewer
                outputZoomForm = new Form
                {
                    Text = "Output preview (click to close)",
                    BackColor = Color.Black,
                    WindowState = FormWindowState.Maximized,
                    StartPosition = FormStartPosition.CenterScreen
                };

                outputZoomPicture = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Black,
                    Image = new Bitmap(picOutput.Image)
                };

                outputZoomPicture.Click += (_, __) => CloseZoomForm();
                outputZoomForm.FormClosed += (_, __) => CloseZoomForm();
                outputZoomForm.Controls.Add(outputZoomPicture);
                outputZoomForm.Show();

                outputZoomed = true;
                lblStatus.Text = "Zoom: fullscreen (click output to close)";
            }
            else
            {
                CloseZoomForm();
                lblStatus.Text = "Zoom reset.";
            }
        }

        private void CloseZoomForm()
        {
            outputZoomed = false;

            var zoomPic = outputZoomPicture;
            var zoomFrm = outputZoomForm;

            outputZoomPicture = null;
            outputZoomForm = null;

            if (zoomPic != null)
            {
                zoomPic.Image?.Dispose();
                zoomPic.Dispose();
            }

            if (zoomFrm != null && !zoomFrm.IsDisposed)
            {
                // Close may trigger FormClosed again, guard via nulling references above.
                try { zoomFrm.Close(); } catch { /* ignore */ }
                try { zoomFrm.Dispose(); } catch { /* ignore */ }
            }
        }
        

        private string GetPythonExe()
        {
            string[] candidateRoots =
            {
                Application.StartupPath,
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", ".."))
            };

            foreach (var root in candidateRoots)
            {
                string candidate = Path.Combine(root, "PythonRuntime", "python.exe");
                if (File.Exists(candidate))
                    return candidate;

                candidate = Path.Combine(root, "python", "python.exe");
                if (File.Exists(candidate))
                    return candidate;
            }

            return "python.exe";
        }

        private string ResolvePath(params string[] parts)
        {
            string[] candidateRoots =
            {
                Application.StartupPath,
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", ".."))
            };

            foreach (var root in candidateRoots)
            {
                var combined = Path.Combine(root, Path.Combine(parts));
                if (File.Exists(combined))
                    return combined;
            }

            return string.Empty;
        }

        private static Image LoadUnlockedImage(string path)
        {
            const int maxAttempts = 3;
            const int delayMs = 100;
            Exception? lastError = null;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(path);
                    if (data.Length == 0)
                        throw new InvalidDataException("Image file is empty.");

                    using var ms = new MemoryStream(data);
                    using var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: false);
                    return new Bitmap(img);
                }
                catch (Exception ex) when (ex is IOException or ArgumentException)
                {
                    lastError = ex;
                    if (attempt < maxAttempts)
                        System.Threading.Thread.Sleep(delayMs);
                }
            }

            throw new InvalidOperationException("Failed to load image data.", lastError);
        }

        private void ToggleUi(bool enabled)
        {
            btnLoad.Enabled = enabled;
            btnRun.Enabled = enabled && originalImage != null;
            btnSave.Enabled = enabled && picOutput.Image != null;
            btnClearMask.Enabled = enabled && maskImage != null;
            trackBrush.Enabled = enabled;
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelTop.BackColor = Theme.PanelAlt;
            panelCenter.BackColor = Theme.Panel;
            panelBottom.BackColor = Theme.PanelAlt;
            panelRight.BackColor = Theme.PanelAlt;

            Theme.StyleButton(btnLoad, Theme.BtnPrimary);
            Theme.StyleButton(btnRun, Theme.BtnAction);
            Theme.StyleButton(btnSave, Theme.BtnSave);
            Theme.StyleButton(btnClearMask, Theme.BtnClear);

            Theme.StyleLabel(lblTitle);
            Theme.StyleLabel(lblBrush);
            Theme.StyleLabel(lblBrushValue);
            Theme.StyleLabel(lblStatus);
            Theme.StyleLabel(lblOutputTitle);

            Theme.StyleTrackBar(trackBrush);

            txtLog.BackColor = Theme.Panel;
            txtLog.ForeColor = Theme.Text;
            progressBar.BackColor = Theme.Panel;

            picImage.BackColor = Color.Black;
            picOutput.BackColor = Color.Black;
        }

        private void InitOverlay()
        {
            overlayLabel = new Label
            {
                Text = "Slika se transformira, molimo pričekajte!",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(140, 0, 0, 0),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            picOutput.Controls.Add(overlayLabel);
            overlayLabel.BringToFront();
        }

        private void ShowOverlay(bool show)
        {
            if (overlayLabel != null)
                overlayLabel.Visible = show;
        }
    }
}
