using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Project
{
    public partial class Form2 : Form
    {
        private string selectedImage = "";
        private string lastOutputImage = "";
        private readonly string tempWorkDir;
        private void UseOutputAsInput()
        {
            if (string.IsNullOrEmpty(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("No output image available yet.");
                return;
            }

            try
            {
                LoadSelectedImage(lastOutputImage);
                lblStatus.Text = "Output postavljen kao novi input.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mogu učitati izlaznu sliku: " + ex.Message);
            }
        }

        public Form2()
        {
            InitializeComponent();

            cbScale.Enabled = false; // unlock after choosing mode

            btnSaveAs.Enabled = false;
            AllowDrop = true;
            DragEnter += Form2_DragEnter;
            DragDrop += Form2_DragDrop;

            // Svi privremeni outputi (ESRGAN lanac + downscale + log) idu u %TEMP%.
            tempWorkDir = Path.Combine(Path.GetTempPath(), "WindowsFormsApp", "ESRGAN");
            Directory.CreateDirectory(tempWorkDir);

            ApplyTheme();
        }

        private void ConfigureScaleOptions(bool upscale)
        {
            cbScale.Items.Clear();
            if (upscale)
            {
                cbScale.Items.AddRange(new object[] { "x2", "x4", "x8", "x16" });
            }
            else
            {
                cbScale.Items.AddRange(new object[] { "x2", "x4", "x8", "x16" });
            }

            if (cbScale.Items.Count > 0)
                cbScale.SelectedIndex = 0;

            cbScale.Enabled = true;
        }

        // -------------------------
        // Load image WITHOUT locking file
        // -------------------------
        private Image LoadUnlockedImage(string path)
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

        private static bool IsImageFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".png" or ".jpg" or ".jpeg" or ".bmp";
        }

        private void LoadSelectedImage(string path)
        {
            selectedImage = path;
            pbOriginal.Image = LoadUnlockedImage(selectedImage);
            lblStatus.Text = "Image loaded.";
            btnSaveAs.Enabled = false;
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadSelectedImage(ofd.FileName);
                }
            }
        }

        private async void btnEnhance_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedImage))
            {
                MessageBox.Show("Please select an image first.");
                return;
            }

            if (!radioUpscale.Checked && !radioDownscale.Checked)
            {
                MessageBox.Show("Odaberite upscale ili downscale.");
                return;
            }

            if (cbScale.SelectedItem == null || !cbScale.Enabled)
            {
                MessageBox.Show("Please select scale factor.");
                return;
            }

            string rawScale = cbScale.SelectedItem.ToString()?.Trim() ?? string.Empty;
            // Accept formats like "2", "0.5", "x2", "2x"
            if (rawScale.StartsWith("x", StringComparison.OrdinalIgnoreCase))
                rawScale = rawScale[1..];
            if (rawScale.EndsWith("x", StringComparison.OrdinalIgnoreCase))
                rawScale = rawScale[..^1];

            if (!double.TryParse(rawScale, NumberStyles.Float, CultureInfo.InvariantCulture, out double targetScale))
            {
                MessageBox.Show("Neispravan faktor skaliranja.");
                return;
            }

            bool isDownscale = radioDownscale.Checked;
            double effectiveScale = targetScale;
            if (isDownscale && targetScale >= 1.0)
                effectiveScale = 1.0 / targetScale;

            string exePath = Path.Combine(Application.StartupPath, "ESRGAN", "realesrgan-ncnn-vulkan.exe");
            string esrganDir = Path.Combine(Application.StartupPath, "ESRGAN");
            string finalOutputPath = Path.Combine(
                tempWorkDir,
                isDownscale
                    ? $"output_downscale_x{targetScale:0.##}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
                    : $"output_x{targetScale:0.##}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            );

            if (!Directory.Exists(esrganDir))
            {
                MessageBox.Show("ESRGAN folder not found next to the application.");
                return;
            }

            if (isDownscale)
            {
                try
                {
                    lblStatus.Text = $"Downscaling x{targetScale:0.##}...";
                    progressBar1.Value = 10;

                    string downscaled = DownscaleImage(selectedImage, finalOutputPath, effectiveScale);

                    pbResult.Image = LoadUnlockedImage(downscaled);
                    lastOutputImage = downscaled;
                    lblStatus.Text = $"Downscale complete (x{targetScale:0.##}).";
                    progressBar1.Value = 100;
                    btnSaveAs.Enabled = true;
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Downscale failed: " + ex.Message;
                    progressBar1.Value = 0;
                }

                return;
            }

            if (!File.Exists(exePath))
            {
                MessageBox.Show("realesrgan-ncnn-vulkan.exe not found in ESRGAN folder.");
                return;
            }

            // Odredi sekvencu prolaza (2x/4x) za zadani target scale
            int[] passes;
            int targetInt = (int)targetScale;
            switch (targetInt)
            {
                case 2:
                    passes = new[] { 2 };
                    break;
                case 4:
                    passes = new[] { 4 };
                    break;
                case 8:
                    passes = new[] { 4, 2 };      // 4 * 2 = 8
                    break;
                case 16:
                    passes = new[] { 4, 4 };      // 4 * 4 = 16
                    break;
                default:
                    MessageBox.Show("Unsupported upscale factor. Use 2, 4, 8, or 16 for upscaling, or pick a downscale value.");
                    return;
            }

            // Počisti stari finalni output
            if (File.Exists(finalOutputPath))
                File.Delete(finalOutputPath);

            // pripremi log u %TEMP%
            string logPath = Path.Combine(tempWorkDir, "ncnn_output.txt");
            File.WriteAllText(logPath,
                $"=== NCNN SUPER RESOLUTION OUTPUT ({DateTime.Now}) ==={Environment.NewLine}" +
                $"TARGET SCALE: x{targetScale}{Environment.NewLine}" +
                $"INPUT: {selectedImage}{Environment.NewLine}{Environment.NewLine}");

            lblStatus.Text = $"Enhancing x{targetScale}...";
            progressBar1.Value = 5;

            string? resultPath = null;

            await Task.Run(() =>
            {
                resultPath = RunNcnnChain(exePath, selectedImage, finalOutputPath, passes, logPath, tempWorkDir);
            });

            if (resultPath != null && File.Exists(resultPath))
            {
                try
                {
                    pbResult.Image = LoadUnlockedImage(resultPath);
                    lblStatus.Text = $"Enhancement complete (x{targetScale})!";
                    lastOutputImage = resultPath;
                    progressBar1.Value = 100;
                    btnSaveAs.Enabled = true;
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error displaying result: " + ex.Message;
                }
            }
            else
            {
                lblStatus.Text = $"Failed to enhance image. Check log: {logPath}";
                progressBar1.Value = 0;
            }
        }

        /// <summary>
        /// Downscale image locally using high quality resizing.
        /// </summary>
        private string DownscaleImage(string inputPath, string outputPath, double factor)
        {
            if (factor <= 0 || factor >= 1)
                throw new ArgumentException("Factor must be between 0 and 1 for downscale.", nameof(factor));

            using (var src = LoadUnlockedImage(inputPath))
            {
                int newWidth = Math.Max(1, (int)Math.Round(src.Width * factor));
                int newHeight = Math.Max(1, (int)Math.Round(src.Height * factor));

                using (var bmp = new Bitmap(newWidth, newHeight))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    g.DrawImage(src, new Rectangle(0, 0, newWidth, newHeight));
                    bmp.Save(outputPath, ImageFormat.Png);
                }
            }

            return outputPath;
        }

        /// <summary>
        /// Pokreće lanac NCNN poziva (više prolaza 2x/4x) dok se ne postigne zadani faktor.
        /// </summary>
        private string? RunNcnnChain(string exePath, string inputPath, string finalOutputPath, int[] passes, string logPath, string workDir)
        {
            string currentInput = inputPath;
            var intermediates = new System.Collections.Generic.List<string>();

            for (int i = 0; i < passes.Length; i++)
            {
                int s = passes[i];

                // privremeni output za ovaj pass
                string tempOut = Path.Combine(workDir, $"tmp_pass{i + 1}_x{s}_{Guid.NewGuid():N}.png");
                if (File.Exists(tempOut))
                    File.Delete(tempOut);

                bool ok = RunNcnnSingle(exePath, currentInput, tempOut, s, logPath, i + 1, passes.Length);

                if (!ok || !File.Exists(tempOut))
                {
                    File.AppendAllText(logPath, $"PASS {i + 1} FAILED. Aborting chain.{Environment.NewLine}");
                    return null;
                }

                intermediates.Add(tempOut);
                currentInput = tempOut;
            }

            // Zadnji privremeni output -> finalni output_xX.png
            try
            {
                if (File.Exists(finalOutputPath))
                    File.Delete(finalOutputPath);

                File.Copy(currentInput, finalOutputPath, true);
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"ERROR copying final output: {ex}{Environment.NewLine}");
                return null;
            }

            // Očisti intermediate slike (ostavi samo finalni output).
            foreach (var tmp in intermediates)
            {
                try { File.Delete(tmp); } catch { /* ignore */ }
            }

            return finalOutputPath;
        }

        /// <summary>
        /// Jedan NCNN poziv (jedan prolaz, scale = 2 ili 4).
        /// </summary>
        private bool RunNcnnSingle(
            string exePath,
            string inputPath,
            string outputPath,
            int scale,
            string logPath,
            int passIndex,
            int passCount)
        {
            try
            {
                string args = $"-i \"{inputPath}\" -o \"{outputPath}\" -s {scale}";

                using (Process proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    proc.Start();

                    string stdout = proc.StandardOutput.ReadToEnd();
                    string stderr = proc.StandardError.ReadToEnd();

                    proc.WaitForExit();

                    File.AppendAllText(logPath,
                        $"--- PASS {passIndex}/{passCount} (x{scale}) ---{Environment.NewLine}" +
                        $"COMMAND: {exePath} {args}{Environment.NewLine}{Environment.NewLine}" +
                        $"STDOUT:{Environment.NewLine}{stdout}{Environment.NewLine}" +
                        $"STDERR:{Environment.NewLine}{stderr}{Environment.NewLine}{Environment.NewLine}");

                    if (!File.Exists(outputPath))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath,
                    $"ERROR in pass {passIndex}: {ex}{Environment.NewLine}");
                return false;
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("No enhanced image available.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp";
                sfd.FileName = $"enhanced_{DateTime.Now:yyyyMMdd_HHmmss}.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(lastOutputImage, sfd.FileName, true);
                        MessageBox.Show("Saved!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file: " + ex.Message);
                    }
                }
            }
        }

        private void btnUseOutput_Click(object sender, EventArgs e)
        {
            UseOutputAsInput();
        }

        private void Form2_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any(IsImageFile))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void Form2_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string? first = files.FirstOrDefault(IsImageFile);
            if (first == null)
                return;

            try
            {
                LoadSelectedImage(first);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image: " + ex.Message);
            }
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelTop.BackColor = Theme.PanelAlt;
            panelBottom.BackColor = Theme.PanelAlt;
            panelCenter.BackColor = Theme.Panel;
            tableImages.BackColor = Theme.Panel;

            pbOriginal.BackColor = Color.Black;
            pbResult.BackColor = Color.Black;

            Theme.StyleButton(btnSelectImage, Theme.BtnPrimary);
            Theme.StyleButton(btnEnhance, Theme.BtnAction);
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);
            Theme.StyleButton(btnUseOutput, Theme.BtnAccent);

            Theme.StyleComboBox(cbScale);
            radioUpscale.ForeColor = Theme.Text;
            radioDownscale.ForeColor = Theme.Text;
            radioUpscale.BackColor = Theme.PanelAlt;
            radioDownscale.BackColor = Theme.PanelAlt;

            Theme.StyleLabel(lblStatus);
            Theme.StyleLabel(lblTitle);
            Theme.StyleLabel(lblOriginalTitle);
            Theme.StyleLabel(lblResultTitle);

            progressBar1.BackColor = Theme.Panel;
        }

        private void radioUpscale_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioUpscale.Checked)
            {
                ConfigureScaleOptions(true);
                lblStatus.Text = "Mode: upscale";
            }
        }

        private void radioDownscale_CheckedChanged(object? sender, EventArgs e)
        {
            if (radioDownscale.Checked)
            {
                ConfigureScaleOptions(false);
                lblStatus.Text = "Mode: downscale";
            }
        }
    }
}
