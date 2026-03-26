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
    public partial class Form9 : Form
    {
        private string inputImagePath = string.Empty;
        private string outputImagePath = string.Empty;
        private bool isProcessing;
        private Label? overlayLabel;

        public Form9()
        {
            InitializeComponent();
            ApplyTheme();
            lblDenoiseValue.Text = $"{trackDenoise.Value}";
            lblScratchValue.Text = $"{trackScratch.Value}%";
            btnRun.Enabled = false;
            btnSave.Enabled = false;
            lblStatus.Text = "Status: spreman";
            progressBar.Style = ProgressBarStyle.Blocks;
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
                inputImagePath = ofd.FileName;
                picInput.Image = LoadUnlockedImage(inputImagePath);
                txtLog.AppendText($"Loaded: {inputImagePath}{Environment.NewLine}");
                lblStatus.Text = "Image ready.";
                btnRun.Enabled = true;
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputImagePath))
            {
                MessageBox.Show("Prvo ucitaj sliku.");
                return;
            }

            string pythonExe = GetPythonExe();
            if (!File.Exists(pythonExe))
            {
                MessageBox.Show("Python runtime nije pronađen.");
                return;
            }

            string scriptPath = ResolvePath("BCKG_REMOVAL", "run_grain_scratch.py");
            if (string.IsNullOrWhiteSpace(scriptPath) || !File.Exists(scriptPath))
            {
                MessageBox.Show("run_grain_scratch.py nije pronađen.");
                return;
            }

            if (isProcessing)
                return;

            isProcessing = true;
            ToggleUi(false);

            string scriptDir = Path.GetDirectoryName(scriptPath)!;
            string outputDir = AppPaths.GetWritableWorkDirectory("BckgRemoval", "GrainScratch");
            outputImagePath = Path.Combine(outputDir, $"restored_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            double denoise = trackDenoise.Value;
            double scratch = trackScratch.Value;

            lblStatus.Text = "Pokrećem obradu...";
            progressBar.Style = ProgressBarStyle.Marquee;
            txtLog.AppendText($"[RUN] denoise={denoise:0.0}, scratch={scratch:0.0}, use_lama=false{Environment.NewLine}");
            ShowOverlay(true);

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments =
                    $"\"{scriptPath}\" --input \"{inputImagePath}\" --output \"{outputImagePath}\" --denoise {denoise.ToString(CultureInfo.InvariantCulture)} --scratch {scratch.ToString(CultureInfo.InvariantCulture)} --no-lama --device cpu",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = scriptDir
            };

            var outputBuffer = new StringBuilder();

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
                MessageBox.Show("Nema slike za spremiti!");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = $"restored_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                picOutput.Image.Save(sfd.FileName);
            txtLog.AppendText("Slika spremljena.\r\n");
            lblStatus.Text = "Saved.";
        }
        }

        private void trackDenoise_Scroll(object sender, EventArgs e)
        {
            lblDenoiseValue.Text = trackDenoise.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void trackScratch_Scroll(object sender, EventArgs e)
        {
            lblScratchValue.Text = $"{trackScratch.Value}%";
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
            btnRun.Enabled = enabled && !string.IsNullOrEmpty(inputImagePath);
            btnSave.Enabled = enabled && picOutput.Image != null;
            trackDenoise.Enabled = enabled;
            trackScratch.Enabled = enabled;
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelTop.BackColor = Theme.PanelAlt;
            panelCenter.BackColor = Theme.Panel;
            panelBottom.BackColor = Theme.PanelAlt;

            Theme.StyleButton(btnLoad, Theme.BtnPrimary);
            Theme.StyleButton(btnRun, Theme.BtnAction);
            Theme.StyleButton(btnSave, Theme.BtnSave);
            Theme.StyleLabel(lblTitle);
            Theme.StyleLabel(lblInputTitle);
            Theme.StyleLabel(lblOutputTitle);
            Theme.StyleLabel(lblDenoise);
            Theme.StyleLabel(lblScratch);
            Theme.StyleLabel(lblDenoiseValue);
            Theme.StyleLabel(lblScratchValue);
            Theme.StyleLabel(lblStatus);

            txtLog.BackColor = Theme.Panel;
            txtLog.ForeColor = Theme.Text;
            progressBar.BackColor = Theme.Panel;

            picInput.BackColor = Color.Black;
            picOutput.BackColor = Color.Black;

            Theme.StyleTrackBar(trackDenoise);
            Theme.StyleTrackBar(trackScratch);
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
