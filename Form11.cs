using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form11 : Form
    {
        private string? inputPath;
        private string? outputPath;
        private Bitmap? inputBitmap;
        private Bitmap? outputBitmap;
        private CancellationTokenSource? cts;

        public Form11()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            ApplyTheme();
            HookEvents();

            btnRun.Enabled = false;
            btnSaveAs.Enabled = false;
            lblStatus.Text = "Spreman.";
            progress.Style = ProgressBarStyle.Blocks;
            progress.Value = 0;

            chkAllowExposure.Checked = true;
            chkAllowSharpen.Checked = true;
            chkAllowFaceRestore.Checked = true;
            chkAllowColorize.Checked = true;
            chkAllowUpscale.Checked = true;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;

            inputBitmap?.Dispose();
            outputBitmap?.Dispose();
            base.OnFormClosed(e);
        }

        private void HookEvents()
        {
            btnLoad.Click += (_, __) => LoadImageViaDialog();
            btnRun.Click += async (_, __) => await RunAutoFixAsync();
            btnSaveAs.Click += (_, __) => SaveOutputAs();

            picInput.DragEnter += PicInput_DragEnter;
            picInput.DragDrop += PicInput_DragDrop;
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelTop.BackColor = Theme.PanelAlt;
            body.BackColor = Theme.Background;
            panelInput.BackColor = Theme.PanelAlt;
            panelOutput.BackColor = Theme.PanelAlt;
            panelLog.BackColor = Theme.PanelAlt;
            panelStatus.BackColor = Theme.PanelAlt;

            picInput.BackColor = Color.Black;
            picOutput.BackColor = Color.Black;

            Theme.StyleButton(btnLoad, Theme.BtnPrimary);
            Theme.StyleButton(btnRun, Theme.BtnAction);
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);

            Theme.StyleLabel(lblInputTitle);
            Theme.StyleLabel(lblOutputTitle);
            Theme.StyleLabel(lblStatus);
            Theme.StyleLabel(lblTitle);

            chkAllowExposure.ForeColor = Theme.Text;
            chkAllowSharpen.ForeColor = Theme.Text;
            chkAllowColorize.ForeColor = Theme.Text;
            chkAllowUpscale.ForeColor = Theme.Text;
            chkAllowFaceRestore.ForeColor = Theme.Text;
            chkAllowExposure.BackColor = Theme.PanelAlt;
            chkAllowSharpen.BackColor = Theme.PanelAlt;
            chkAllowColorize.BackColor = Theme.PanelAlt;
            chkAllowUpscale.BackColor = Theme.PanelAlt;
            chkAllowFaceRestore.BackColor = Theme.PanelAlt;

            txtLog.BackColor = Theme.Panel;
            txtLog.ForeColor = Theme.Text;
            progress.BackColor = Theme.Panel;
        }

        private void PicInput_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Any(IsImageFile))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void PicInput_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
                return;

            var files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
            string? first = files?.FirstOrDefault(IsImageFile);
            if (first == null)
                return;

            LoadInput(first);
        }

        private static bool IsImageFile(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".jpg" or ".jpeg" or ".png" or ".bmp";
        }

        private void LoadImageViaDialog()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
                LoadInput(ofd.FileName);
        }

        private void LoadInput(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Datoteka ne postoji.");
                return;
            }

            inputPath = path;
            btnRun.Enabled = true;
            btnSaveAs.Enabled = false;
            outputPath = null;

            inputBitmap?.Dispose();
            outputBitmap?.Dispose();
            outputBitmap = null;

            try
            {
                inputBitmap = LoadUnlockedBitmap(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mogu ucitati sliku: " + ex.Message);
                inputPath = null;
                btnRun.Enabled = false;
                return;
            }

            picInput.Image?.Dispose();
            picInput.Image = new Bitmap(inputBitmap);
            picOutput.Image?.Dispose();
            picOutput.Image = null;

            txtLog.Clear();
            AppendLog($"[INPUT] {path}");
            SetStatus("Input ucitan.");
        }

        private void SaveOutputAs()
        {
            if (outputBitmap == null)
            {
                MessageBox.Show("Nema output slike.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp",
                FileName = $"autofix_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                outputBitmap.Save(sfd.FileName);
                SetStatus("Spremljeno.");
            }
        }

        private void SetStatus(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetStatus(text)));
                return;
            }
            lblStatus.Text = text;
        }

        private void AppendLog(string text)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new Action(() => AppendLog(text)));
                return;
            }
            txtLog.AppendText(text + Environment.NewLine);
        }

        private async Task RunAutoFixAsync()
        {
            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                MessageBox.Show("Ucitaj sliku prvo.");
                return;
            }

            btnRun.Enabled = false;
            btnSaveAs.Enabled = false;
            btnLoad.Enabled = false;

            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();
            var token = cts.Token;

            progress.Style = ProgressBarStyle.Marquee;
            progress.Value = 0;

            try
            {
                SetStatus("Analiziram sliku...");
                var analysis = await Task.Run(() => AnalyzeImage(inputPath!), token);
                AppendLog($"[ANALYSIS] {analysis}");

                var steps = BuildPipeline(analysis);
                if (steps.Count == 0)
                {
                    SetStatus("Nema potrebnih popravaka (po heuristikama).");
                    return;
                }

                string current = inputPath!;
                int stepIndex = 0;
                foreach (var step in steps)
                {
                    token.ThrowIfCancellationRequested();
                    stepIndex++;

                    SetStatus($"AutoFix: {stepIndex}/{steps.Count} - {step.Name}");
                    AppendLog($"[STEP {stepIndex}/{steps.Count}] {step.Name}");
                    current = await step.RunAsync(current, token);
                    AppendLog($"  -> {current}");
                }

                outputPath = current;
                outputBitmap?.Dispose();
                outputBitmap = LoadUnlockedBitmap(outputPath);

                picOutput.Image?.Dispose();
                picOutput.Image = new Bitmap(outputBitmap);

                btnSaveAs.Enabled = true;
                SetStatus("Gotovo.");
            }
            catch (OperationCanceledException)
            {
                SetStatus("Prekinuto.");
                AppendLog("[CANCEL] Prekinuto od korisnika.");
            }
            catch (Exception ex)
            {
                SetStatus("Greska.");
                AppendLog("[ERROR] " + ex.Message);
                MessageBox.Show(ex.Message, "AutoFix greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progress.Style = ProgressBarStyle.Blocks;
                btnRun.Enabled = inputPath != null;
                btnLoad.Enabled = true;
            }
        }

        private sealed class PipelineStep
        {
            public string Name { get; }
            public Func<string, CancellationToken, Task<string>> RunAsync { get; }
            public PipelineStep(string name, Func<string, CancellationToken, Task<string>> runAsync)
            {
                Name = name;
                RunAsync = runAsync;
            }
        }

        private List<PipelineStep> BuildPipeline(ImageAnalysis a)
        {
            var steps = new System.Collections.Generic.List<PipelineStep>();
            bool willTryFaceRestore = false;

            if (chkAllowExposure.Checked)
            {
                const double targetLuma = 0.46;
                const double lumaDeadZone = 0.07;
                double safeMeanLuma = Math.Clamp(a.MeanLuma, 0.05, 0.95);
                double brightnessFactor = targetLuma / safeMeanLuma;
                double maxBright = a.LaplacianVariance < 20.0 ? 1.22 : 1.70;
                brightnessFactor = Math.Clamp(brightnessFactor, 0.65, maxBright);

                if (Math.Abs(a.MeanLuma - targetLuma) > lumaDeadZone &&
                    Math.Abs(brightnessFactor - 1.0) >= 0.06)
                {
                    steps.Add(new PipelineStep(
                        $"Brightness x{brightnessFactor:0.00}",
                        (p, t) => RunTransformAsync(p, "brightness", brightnessFactor, t)));
                }

                if (a.StdLuma < 0.11)
                {
                    double factor = a.StdLuma < 0.05 ? 1.45 : a.StdLuma < 0.08 ? 1.30 : 1.15;
                    steps.Add(new PipelineStep(
                        $"Contrast x{factor:0.00}",
                        (p, t) => RunTransformAsync(p, "contrast", factor, t)));
                }
            }

            // Grain/scratch cleanup for noisy legacy photos. This is conservative and soft-skips on failure.
            if (chkAllowSharpen.Checked)
            {
                bool grainLikely = a.LaplacianVariance > 420.0 && a.StdLuma > 0.11;
                bool scratchLikely = a.LaplacianVariance > 300.0 && a.StdLuma > 0.16;

                if (grainLikely || scratchLikely)
                {
                    double denoise = Math.Clamp(18.0 + (a.LaplacianVariance - 260.0) / 14.0, 12.0, 48.0);
                    double scratch = Math.Clamp(38.0 + (a.StdLuma * 220.0), 35.0, 75.0);
                    steps.Add(new PipelineStep(
                        $"Grain/Scratch clean (d={denoise:0}, s={scratch:0})",
                        (p, t) => RunGrainScratchMaybeAsync(p, denoise, scratch, t)));
                }
            }

            // If image is very blurry and face restore is enabled, try GFPGAN first.
            // GFPGAN will fail gracefully when no faces are detected (we treat that as a soft-skip).
            if (chkAllowFaceRestore.Checked)
            {
                int shortSide = Math.Min(a.Width, a.Height);
                bool veryBlurry = a.LaplacianVariance < 85.0;
                bool lowRes = shortSide < 720;
                bool lowContrastFaceRegionLikely = a.StdLuma < 0.10;
                bool canHaveFace = shortSide >= 180;

                if (canHaveFace && (veryBlurry || lowRes || lowContrastFaceRegionLikely))
                {
                    willTryFaceRestore = true;
                    int upscale = shortSide < 420 ? 2 : 1;
                    steps.Add(new PipelineStep($"GFPGAN face restore (upscale x{upscale})", (p, t) => RunGfpganMaybeAsync(p, upscale, t)));

                    double fidelity = veryBlurry ? 0.62 : 0.72;
                    int cfUpscale = shortSide < 520 ? 2 : 1;
                    steps.Add(new PipelineStep(
                        $"CodeFormer face restore (w={fidelity:0.00}, up x{cfUpscale})",
                        (p, t) => RunCodeFormerMaybeAsync(p, fidelity, cfUpscale, t)));
                }
            }

            if (chkAllowSharpen.Checked && a.LaplacianVariance >= 35.0 && a.LaplacianVariance < 120.0 && !willTryFaceRestore)
            {
                // Keep sharpening conservative; strong sharpening on blurred images can create halos.
                double factor = a.LaplacianVariance < 60.0 ? 1.55 : 1.35;
                steps.Add(new PipelineStep($"Sharpness x{factor:0.00}", (p, t) => RunTransformAsync(p, "sharpness", factor, t)));
            }

            // DDColor is powerful; keep it mild and only when image is close to grayscale.
            if (chkAllowColorize.Checked && a.MeanSaturation < 0.035)
            {
                double strength = a.MeanSaturation < 0.015 ? 0.70 : 0.60;
                steps.Add(new PipelineStep($"DDColor strength {strength:0.00}", (p, t) => RunDdcolorAsync(p, strength, t)));
                steps.Add(new PipelineStep("Saturation soften x0.92", (p, t) => RunTransformAsync(p, "saturation", 0.92, t)));
            }

            if (chkAllowUpscale.Checked)
            {
                int shortSide = Math.Min(a.Width, a.Height);
                int scale = shortSide < 360 ? 4 : shortSide < 720 ? 2 : 1;
                if (a.LaplacianVariance < 20.0)
                    scale = Math.Min(scale, 2);
                if (scale > 1)
                    steps.Add(new PipelineStep($"ESRGAN upscale x{scale}", (p, t) => RunEsrganAsync(p, scale, t)));
            }

            return steps;
        }

        private sealed record ImageAnalysis(
            int Width,
            int Height,
            double MeanLuma,
            double StdLuma,
            double MeanSaturation,
            double LaplacianVariance
        )
        {
            public override string ToString()
            {
                return
                    $"size={Width}x{Height}, " +
                    $"luma(mean={MeanLuma:0.000}, std={StdLuma:0.000}), " +
                    $"sat(mean={MeanSaturation:0.000}), " +
                    $"sharp(varLap={LaplacianVariance:0.0})";
            }
        }

        private static ImageAnalysis AnalyzeImage(string path)
        {
            using var bmp = LoadUnlockedBitmap(path);
            int w = bmp.Width;
            int h = bmp.Height;
            int step = Math.Max(1, Math.Min(w, h) / 200);

            double sumL = 0, sumL2 = 0, sumS = 0;
            double sumLap = 0, sumLap2 = 0;
            long count = 0, lapCount = 0;

            using var rgb24 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(rgb24))
                g.DrawImage(bmp, 0, 0, w, h);

            var rect = new Rectangle(0, 0, w, h);
            var data = rgb24.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                int stride = data.Stride;
                int bytes = stride * h;
                var buffer = new byte[bytes];
                Marshal.Copy(data.Scan0, buffer, 0, bytes);

                for (int y = 0; y < h; y += step)
                {
                    int rowOffset = y * stride;
                    for (int x = 0; x < w; x += step)
                    {
                        int i = rowOffset + (x * 3);
                        byte b = buffer[i + 0], g2 = buffer[i + 1], r2 = buffer[i + 2];

                        double luma = (0.2126 * r2 + 0.7152 * g2 + 0.0722 * b) / 255.0;
                        sumL += luma;
                        sumL2 += luma * luma;

                        double max = Math.Max(r2, Math.Max(g2, b));
                        double min = Math.Min(r2, Math.Min(g2, b));
                        double sat = max <= 0 ? 0 : (max - min) / max;
                        sumS += sat;
                        count++;
                    }
                }

                int lapStep = Math.Max(1, step);
                for (int y = 1; y < h - 1; y += lapStep)
                {
                    int rowOffset = y * stride;
                    int rowUp = (y - 1) * stride;
                    int rowDn = (y + 1) * stride;

                    for (int x = 1; x < w - 1; x += lapStep)
                    {
                        int idx = x * 3;

                        double c = Luma255(buffer, rowOffset + idx);
                        double u = Luma255(buffer, rowUp + idx);
                        double d = Luma255(buffer, rowDn + idx);
                        double l = Luma255(buffer, rowOffset + idx - 3);
                        double r = Luma255(buffer, rowOffset + idx + 3);

                        double lap = (4 * c) - u - d - l - r;
                        sumLap += lap;
                        sumLap2 += lap * lap;
                        lapCount++;
                    }
                }
            }
            finally
            {
                rgb24.UnlockBits(data);
            }

            double meanL = count > 0 ? sumL / count : 0;
            double varL = count > 0 ? (sumL2 / count) - (meanL * meanL) : 0;
            double stdL = Math.Sqrt(Math.Max(0, varL));
            double meanS = count > 0 ? sumS / count : 0;

            double meanLap = lapCount > 0 ? sumLap / lapCount : 0;
            double varLap = lapCount > 0 ? (sumLap2 / lapCount) - (meanLap * meanLap) : 0;

            return new ImageAnalysis(w, h, meanL, stdL, meanS, Math.Max(0, varLap));
        }

        private static double Luma255(byte[] buffer, int offset)
        {
            byte b = buffer[offset + 0];
            byte g = buffer[offset + 1];
            byte r = buffer[offset + 2];
            return (0.2126 * r + 0.7152 * g + 0.0722 * b);
        }

        private static Bitmap LoadUnlockedBitmap(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var img = Image.FromStream(fs, useEmbeddedColorManagement: false, validateImageData: false);
            return new Bitmap(img);
        }

        private string GetProjectRoot()
        {
            return AppPaths.FindAppRoot();
        }

        private string GetPythonExe()
        {
            return AppPaths.FindPythonExe();
        }

        private async Task<string> RunTransformAsync(string input, string operation, double param, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string python = GetPythonExe();
            string script = Path.Combine(projectRoot, "python", "transform.py");

            if (!File.Exists(script))
                throw new FileNotFoundException("Nedostaje python/transform.py", script);

            string outFile = Path.Combine(Path.GetTempPath(), $"autofix_{operation}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png");

            var psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments = $"\"{script}\" \"{input}\" \"{operation}\" \"{param.ToString(CultureInfo.InvariantCulture)}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            var stdOutTask = proc.StandardOutput.ReadToEndAsync();
            var stdErrTask = proc.StandardError.ReadToEndAsync();

            await Task.WhenAll(stdOutTask, stdErrTask);
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"transform.py failed ({operation}).\n{stdErrTask.Result}");

            string base64 = (stdOutTask.Result ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(base64))
                throw new Exception("transform.py nije vratio base64 output.");

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                throw new Exception($"transform.py output nije valjani base64.\nSTDERR:\n{stdErrTask.Result}\n\nSTDOUT:\n{base64}");
            }

            await File.WriteAllBytesAsync(outFile, bytes, token);
            return outFile;
        }

        private async Task<string> RunDdcolorAsync(string input, double strength, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string python = GetPythonExe();
            string script = Path.Combine(projectRoot, "DDCOLOR", "ddcolor_run.py");

            if (!File.Exists(script))
                throw new FileNotFoundException("Nedostaje DDCOLOR/ddcolor_run.py", script);

            string outFile = Path.Combine(Path.GetTempPath(), $"autofix_ddcolor_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png");

            var psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments = $"\"{script}\" \"{input}\" \"{outFile}\" {strength.ToString("0.00", CultureInfo.InvariantCulture)}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(script) ?? projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            string stdOut = await proc.StandardOutput.ReadToEndAsync();
            string stdErr = await proc.StandardError.ReadToEndAsync();
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"DDColor nije uspio.\nSTDOUT:\n{stdOut}\n\nSTDERR:\n{stdErr}");

            if (!File.Exists(outFile))
                throw new Exception("DDColor nije generirao output datoteku.");

            return outFile;
        }

        private async Task<string> RunEsrganAsync(string input, int scale, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string exe = Path.Combine(projectRoot, "ESRGAN", "realesrgan-ncnn-vulkan.exe");
            if (!File.Exists(exe))
                throw new FileNotFoundException("Nedostaje ESRGAN/realesrgan-ncnn-vulkan.exe", exe);

            string outFile = Path.Combine(Path.GetTempPath(), $"autofix_esrgan_x{scale}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png");

            var psi = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = $"-i \"{input}\" -o \"{outFile}\" -s {scale}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(exe) ?? projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            string stdOut = await proc.StandardOutput.ReadToEndAsync();
            string stdErr = await proc.StandardError.ReadToEndAsync();
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"ESRGAN nije uspio.\nSTDOUT:\n{stdOut}\n\nSTDERR:\n{stdErr}");

            if (!File.Exists(outFile))
                throw new Exception("ESRGAN nije generirao output datoteku.");

            return outFile;
        }

        private async Task<string> RunGfpganMaybeAsync(string input, int upscale, CancellationToken token)
        {
            try
            {
                return await RunGfpganAsync(input, upscale, token);
            }
            catch (Exception ex)
            {
                // Most common: "no faces detected" (exit code 6) - treat as soft skip.
                AppendLog("[GFPGAN] preskacem: " + ex.Message.Split('\n').FirstOrDefault());
                return input;
            }
        }

        private async Task<string> RunGfpganAsync(string input, int upscale, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string python = GetPythonExe();
            string script = Path.Combine(projectRoot, "python", "gfpgan", "run_gfpgan.py");
            if (!File.Exists(script))
                throw new FileNotFoundException("Nedostaje python/gfpgan/run_gfpgan.py", script);

            string outFile = Path.Combine(Path.GetTempPath(), $"autofix_gfpgan_x{upscale}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png");

            var psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments = $"\"{script}\" \"{input}\" \"{outFile}\" --upscale {upscale} --only-center-face",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(script) ?? projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            string stdOut = await proc.StandardOutput.ReadToEndAsync();
            string stdErr = await proc.StandardError.ReadToEndAsync();
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"GFPGAN nije uspio.\nSTDOUT:\n{stdOut}\n\nSTDERR:\n{stdErr}");

            if (!File.Exists(outFile))
                throw new Exception("GFPGAN nije generirao output datoteku.");

            return outFile;
        }

        private async Task<string> RunCodeFormerMaybeAsync(string input, double fidelityWeight, int upscale, CancellationToken token)
        {
            try
            {
                return await RunCodeFormerAsync(input, fidelityWeight, upscale, token);
            }
            catch (Exception ex)
            {
                AppendLog("[CodeFormer] preskacem: " + ex.Message.Split('\n').FirstOrDefault());
                return input;
            }
        }

        private async Task<string> RunCodeFormerAsync(string input, double fidelityWeight, int upscale, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string python = GetPythonExe();
            string script = Path.Combine(projectRoot, "CODEFORMER", "inference_codeformer.py");
            if (!File.Exists(script))
                throw new FileNotFoundException("Nedostaje CODEFORMER/inference_codeformer.py", script);

            string codeformerWeight = Path.Combine(projectRoot, "CODEFORMER", "weights", "CodeFormer", "codeformer.pth");
            if (!File.Exists(codeformerWeight))
                throw new FileNotFoundException("Nedostaje CodeFormer tezina", codeformerWeight);

            string outputRoot = Path.Combine(
                Path.GetTempPath(),
                "WindowsFormsApp",
                "AutoFix",
                $"codeformer_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}");
            Directory.CreateDirectory(outputRoot);

            string fidelity = fidelityWeight.ToString("0.00", CultureInfo.InvariantCulture);

            var psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments =
                    $"\"{script}\" -i \"{input}\" -o \"{outputRoot}\" -w {fidelity} -s {upscale} " +
                    "--detection_model retinaface_resnet50 --only_center_face --bg_upsampler None",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(script) ?? projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            string stdOut = await proc.StandardOutput.ReadToEndAsync();
            string stdErr = await proc.StandardError.ReadToEndAsync();
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"CodeFormer nije uspio.\nSTDOUT:\n{stdOut}\n\nSTDERR:\n{stdErr}");

            string finalResultsDir = Path.Combine(outputRoot, "final_results");
            string? outFile = GetNewestImage(finalResultsDir);

            if (string.IsNullOrWhiteSpace(outFile))
                outFile = GetNewestImage(outputRoot);

            if (string.IsNullOrWhiteSpace(outFile) || !File.Exists(outFile))
                throw new Exception("CodeFormer nije generirao output datoteku.");

            return outFile;
        }

        private async Task<string> RunGrainScratchMaybeAsync(string input, double denoise, double scratch, CancellationToken token)
        {
            try
            {
                return await RunGrainScratchAsync(input, denoise, scratch, token);
            }
            catch (Exception ex)
            {
                AppendLog("[Grain/Scratch] preskacem: " + ex.Message.Split('\n').FirstOrDefault());
                return input;
            }
        }

        private async Task<string> RunGrainScratchAsync(string input, double denoise, double scratch, CancellationToken token)
        {
            string projectRoot = GetProjectRoot();
            string python = GetPythonExe();
            string script = Path.Combine(projectRoot, "BCKG_REMOVAL", "run_grain_scratch.py");
            if (!File.Exists(script))
                throw new FileNotFoundException("Nedostaje BCKG_REMOVAL/run_grain_scratch.py", script);

            string outFile = Path.Combine(Path.GetTempPath(), $"autofix_grain_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png");
            string denoiseArg = denoise.ToString("0.0", CultureInfo.InvariantCulture);
            string scratchArg = scratch.ToString("0.0", CultureInfo.InvariantCulture);

            var psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments =
                    $"\"{script}\" --input \"{input}\" --output \"{outFile}\" " +
                    $"--denoise {denoiseArg} --scratch {scratchArg} --no-lama --device cpu",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(script) ?? projectRoot
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();

            string stdOut = await proc.StandardOutput.ReadToEndAsync();
            string stdErr = await proc.StandardError.ReadToEndAsync();
            proc.WaitForExit();

            token.ThrowIfCancellationRequested();

            if (proc.ExitCode != 0)
                throw new Exception($"Grain/scratch obrada nije uspjela.\nSTDOUT:\n{stdOut}\n\nSTDERR:\n{stdErr}");

            if (!File.Exists(outFile))
                throw new Exception("Grain/scratch skripta nije generirala output datoteku.");

            return outFile;
        }

        private static string? GetNewestImage(string folder)
        {
            if (!Directory.Exists(folder))
                return null;

            var candidates = Directory
                .EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(p =>
                {
                    string ext = Path.GetExtension(p).ToLowerInvariant();
                    return ext is ".png" or ".jpg" or ".jpeg" or ".bmp";
                })
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .ToList();

            return candidates.FirstOrDefault();
        }
    }
}
