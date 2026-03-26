using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Project
{
    public partial class Form6 : Form
    {
        private string? inputImagePath;
        private string? selectedStylePath;
        private readonly Dictionary<PictureBox, string> thumbnailMap = new();

        // Session-only stilovi: žive dok je aplikacija pokrenuta (čak i ako se forma zatvori/ponovno otvori),
        // ali se ne spremaju u STYLE_TRANSFER/StyleTemplates pa nestanu nakon restarta aplikacije.
        private static readonly object SessionLock = new();
        private static bool SessionInitialized;
        private static readonly List<string> SessionStylePaths = new();
        private static readonly string SessionStyleFolder = Path.Combine(
            Path.GetTempPath(),
            "WindowsFormsApp",
            "StyleTemplatesSession",
            Guid.NewGuid().ToString("N")
        );

        public Form6()
        {
            InitializeComponent();
            EnsureSessionStorage();
            InitializeResponsiveTopBar();

            LoadStyleTemplates();
            UpdateSliderLabel();
        }

        private void InitializeResponsiveTopBar()
        {
            panelTop.Height = 104;
            panelTop.Resize += (_, __) => UpdateTopBarLayout();
            Resize += (_, __) => UpdateTopBarLayout();
            Shown += (_, __) => BeginInvoke(new Action(UpdateTopBarLayout));
            UpdateTopBarLayout();
        }

        private void UpdateTopBarLayout()
        {
            if (panelTop.ClientSize.Width <= 0)
                return;

            const int margin = 14;
            const int preferredGap = 10;
            const int compactGap = 6;
            const int rowGap = 6;
            const int topPadding = 10;
            const int bottomPadding = 8;
            const int minTrackWidth = 120;
            const int maxTrackWidth = 220;
            Button[] buttons =
            {
                btnAddStyle,
                btnLoadInput,
                btnReloadStyles,
                btnPrimijeniStil,
                btnSaveOutput,
                btnClearOutput
            };

            panelTop.SuspendLayout();
            lblTitle.Location = new Point(margin, topPadding);

            int buttonRowY = lblTitle.Bottom + rowGap;
            int rowHeight = Math.Max(btnLoadInput.Height, Math.Max(trackStyleStrength.Height, lblStyleStrength.Height));
            int valueWidth = Math.Max(lblStyleStrengthValue.PreferredWidth, 36);
            int labelWidth = lblStyleStrength.PreferredWidth;
            int buttonWidths = 0;
            foreach (var button in buttons)
                buttonWidths += button.Width;

            int gap = preferredGap;
            int availableForTrack = panelTop.ClientSize.Width - (margin * 2) - buttonWidths - (gap * (buttons.Length + 2)) - labelWidth - valueWidth;
            if (availableForTrack < minTrackWidth)
            {
                gap = compactGap;
                availableForTrack = panelTop.ClientSize.Width - (margin * 2) - buttonWidths - (gap * (buttons.Length + 2)) - labelWidth - valueWidth;
            }

            int trackWidth = Math.Max(80, Math.Min(maxTrackWidth, availableForTrack));
            int x = margin;
            int buttonOffsetY = Math.Max(0, (rowHeight - btnLoadInput.Height) / 2);

            foreach (var button in buttons)
            {
                button.Location = new Point(x, buttonRowY + buttonOffsetY);
                x += button.Width + gap;
            }

            int labelY = buttonRowY + Math.Max(0, (rowHeight - lblStyleStrength.Height) / 2);
            int trackY = buttonRowY + Math.Max(0, (rowHeight - trackStyleStrength.Height) / 2);
            int valueY = buttonRowY + Math.Max(0, (rowHeight - lblStyleStrengthValue.Height) / 2);

            lblStyleStrength.Location = new Point(x, labelY);
            trackStyleStrength.Size = new Size(trackWidth, trackStyleStrength.Height);
            trackStyleStrength.Location = new Point(lblStyleStrength.Right + gap, trackY);
            lblStyleStrengthValue.Location = new Point(trackStyleStrength.Right + gap, valueY);

            int neededHeight = buttonRowY + rowHeight + bottomPadding;
            if (panelTop.Height != neededHeight)
                panelTop.Height = neededHeight;

            panelTop.ResumeLayout(true);
        }

        private static void EnsureSessionStorage()
        {
            lock (SessionLock)
            {
                if (SessionInitialized)
                    return;

                Directory.CreateDirectory(SessionStyleFolder);

                // Cleanup pokušaj kad se aplikacija zatvara (nije kritično ako ne uspije).
                Application.ApplicationExit += (_, __) =>
                {
                    try
                    {
                        if (Directory.Exists(SessionStyleFolder))
                            Directory.Delete(SessionStyleFolder, true);
                    }
                    catch
                    {
                        // ignore cleanup failure
                    }
                };

                SessionInitialized = true;
            }
        }

        // --------------------------------------------------------
        // PATH UTILITIES
        // --------------------------------------------------------
        private string GetProjectRoot()
        {
            return AppPaths.FindAppRoot();
        }

        private string GetPythonInterpreter()
        {
            string embedded = AppPaths.FindPythonExe();
            if (!string.Equals(embedded, "python.exe", StringComparison.OrdinalIgnoreCase) && File.Exists(embedded))
                return embedded;

            string venvPython = Path.Combine(GetProjectRoot(), "STYLE_TRANSFER", "venv", "Scripts", "python.exe");
            return File.Exists(venvPython) ? venvPython : "python.exe";
        }

        // --------------------------------------------------------
        // LOAD STYLE TEMPLATE THUMBNAILS
        // --------------------------------------------------------
        private string FindStyleTemplatesFolder()
        {
            foreach (var root in AppPaths.CandidateRoots())
            {
                string dir = Path.Combine(root, "STYLE_TRANSFER", "StyleTemplates");
                if (!Directory.Exists(dir))
                    continue;

                foreach (var pattern in new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp" })
                {
                    if (Directory.GetFiles(dir, pattern).Length > 0)
                        return dir;
                }
            }

            foreach (var root in AppPaths.CandidateRoots())
            {
                string dir = Path.Combine(root, "STYLE_TRANSFER", "StyleTemplates");
                if (Directory.Exists(dir))
                    return dir;
            }

            return Path.Combine(GetProjectRoot(), "STYLE_TRANSFER", "StyleTemplates");
        }

        private void LoadStyleTemplates()
        {
            string folder = FindStyleTemplatesFolder();

            flowStyles.Controls.Clear();
            thumbnailMap.Clear();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                lblStatus.Text = "Folder za stilove kreiran. Dodajte slike.";
            }

            var files = new List<string>();
            foreach (var pattern in new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp" })
                files.AddRange(Directory.GetFiles(folder, pattern));

            foreach (var file in files)
            {
                try
                {
                    PictureBox pb = new PictureBox
                    {
                        Width = 100,
                        Height = 100,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Margin = new Padding(8),
                        BorderStyle = BorderStyle.FixedSingle,
                        Cursor = Cursors.Hand
                    };

                    using var img = Image.FromFile(file);
                    pb.Image = new Bitmap(img);

                    pb.Click += Thumbnail_Click;
                    thumbnailMap[pb] = file;
                    flowStyles.Controls.Add(pb);
                }
                catch
                {
                    // ignore corrupted images
                }
            }

            // Session-only stilovi (dodani preko "Add style") - vidljivi samo dok aplikacija radi.
            foreach (var file in SessionStylePaths)
            {
                if (!File.Exists(file))
                    continue;

                try
                {
                    PictureBox pb = new PictureBox
                    {
                        Width = 100,
                        Height = 100,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Margin = new Padding(8),
                        BorderStyle = BorderStyle.FixedSingle,
                        Cursor = Cursors.Hand
                    };

                    using var img = Image.FromFile(file);
                    pb.Image = new Bitmap(img);

                    pb.Click += Thumbnail_Click;
                    thumbnailMap[pb] = file;
                    flowStyles.Controls.Add(pb);
                }
                catch
                {
                    // ignore corrupted images
                }
            }

            lblStatus.Text = "Stilovi učitani.";
        }

        private void SelectStyleThumbnail(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            foreach (Control c in flowStyles.Controls)
                if (c is PictureBox pic) pic.BorderStyle = BorderStyle.FixedSingle;

            foreach (var kv in thumbnailMap)
            {
                if (string.Equals(kv.Value, path, StringComparison.OrdinalIgnoreCase))
                {
                    kv.Key.BorderStyle = BorderStyle.Fixed3D;
                    selectedStylePath = kv.Value;
                    try
                    {
                        using var img = Image.FromFile(selectedStylePath);
                        picStyle.Image = new Bitmap(img);
                    }
                    catch { }
                    lblStatus.Text = $"Odabran stil: {Path.GetFileName(selectedStylePath)}";
                    break;
                }
            }
        }

        // --------------------------------------------------------
        // STYLE THUMBNAIL CLICK
        // --------------------------------------------------------
        private void Thumbnail_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox pb)
            {
                foreach (Control c in flowStyles.Controls)
                    if (c is PictureBox pic) pic.BorderStyle = BorderStyle.FixedSingle;

                pb.BorderStyle = BorderStyle.Fixed3D;
                selectedStylePath = thumbnailMap[pb];

                try
                {
                    using var img = Image.FromFile(selectedStylePath);
                    picStyle.Image = new Bitmap(img);
                }
                catch { }

                lblStatus.Text = $"Odabran stil: {Path.GetFileName(selectedStylePath)}";
            }
        }

        // --------------------------------------------------------
        // LOAD INPUT IMAGE
        // --------------------------------------------------------
        private void btnLoadInput_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "Slike|*.jpg;*.jpeg;*.png",
                Title = "Odaberi ulaznu sliku"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputImagePath = ofd.FileName;
                using var img = Image.FromFile(inputImagePath);
                picInput.Image = new Bitmap(img);
                lblStatus.Text = "Ulazna slika učitana.";
            }
        }

        // --------------------------------------------------------
        // SLIDER UPDATE
        // --------------------------------------------------------
        private void trackStyleStrength_Scroll(object sender, EventArgs e)
        {
            UpdateSliderLabel();
        }

        private void UpdateSliderLabel()
        {
            double val = trackStyleStrength.Value / 100.0;
            lblStyleStrengthValue.Text = val.ToString("0.00", CultureInfo.InvariantCulture);
            UpdateTopBarLayout();
        }

        // --------------------------------------------------------
        // ADAIN STYLE TRANSFER
        // --------------------------------------------------------
        private void btnPrimijeniStil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputImagePath))
            {
                MessageBox.Show("Odaberi ulaznu sliku.", "Greška");
                return;
            }

            if (string.IsNullOrEmpty(selectedStylePath))
            {
                MessageBox.Show("Odaberi stil.", "Greška");
                return;
            }

            string python = GetPythonInterpreter();
            if (python == "")
            {
                MessageBox.Show("Python venv nije pronađen!", "Greška");
                return;
            }

            string adainFolder = AppPaths.ResolveDirectory("STYLE_TRANSFER", "AdaIN");
            string runner = AppPaths.ResolveFile("STYLE_TRANSFER", "AdaIN", "adain_run.py");
            if (string.IsNullOrWhiteSpace(adainFolder) && !string.IsNullOrWhiteSpace(runner))
                adainFolder = Path.GetDirectoryName(runner) ?? string.Empty;

            string modelFolder = AppPaths.ResolveDirectory("STYLE_TRANSFER", "AdaIN", "models");
            string vggPath = AppPaths.ResolveFile("STYLE_TRANSFER", "AdaIN", "models", "vgg_normalised.pth");
            string decoderPath = AppPaths.ResolveFile("STYLE_TRANSFER", "AdaIN", "models", "decoder.pth");
            if (string.IsNullOrWhiteSpace(modelFolder) && !string.IsNullOrWhiteSpace(vggPath))
                modelFolder = Path.GetDirectoryName(vggPath) ?? string.Empty;

            if (!File.Exists(vggPath) || !File.Exists(decoderPath))
            {
                MessageBox.Show("Nedostaju model datoteke (vgg_normalised.pth/decoder.pth).", "Greska");
                return;
            }

            string outputFile = Path.Combine(Path.GetTempPath(), "style_output.jpg");

            double alpha = trackStyleStrength.Value / 100.0;
            string alphaStr = alpha.ToString(CultureInfo.InvariantCulture);

            lblStatus.Text = "Obrada (AdaIN)...";
            btnPrimijeniStil.Enabled = false;

            // Limit size to improve quality and match AdaIN reference script defaults
            const int resizeShortSide = 512;

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = python,
                Arguments =
                    $"\"{runner}\" --content \"{inputImagePath}\" --style \"{selectedStylePath}\" " +
                    $"--alpha {alphaStr} --output \"{outputFile}\" --modelpath \"{modelFolder}\" " +
                    $"--content_size {resizeShortSide} --style_size {resizeShortSide}",

                WorkingDirectory = adainFolder,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            psi.EnvironmentVariables["PYTHONUNBUFFERED"] = "1";

            var proc = new Process();
            proc.StartInfo = psi;
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.ErrorDataReceived += Proc_ErrorDataReceived;

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();

            if (File.Exists(outputFile))
            {
                using var fs = new FileStream(outputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var img = Image.FromStream(fs, useEmbeddedColorManagement: true, validateImageData: true);
                picOutput.Image?.Dispose();
                picOutput.Image = new Bitmap(img);
                lblStatus.Text = "Gotovo.";
            }
            else
            {
                lblStatus.Text = "Greška!";
                MessageBox.Show("AdaIN nije generirao izlaznu sliku.");
            }

            btnPrimijeniStil.Enabled = true;
        }

        // --------------------------------------------------------
        // SAVE OUTPUT
        // --------------------------------------------------------
        private void btnSaveOutput_Click(object sender, EventArgs e)
        {
            if (picOutput.Image == null)
            {
                MessageBox.Show("Nema slike za spremiti.");
                return;
            }

            using SaveFileDialog sfd = new()
            {
                Filter = "JPEG|*.jpg|PNG|*.png",
                Title = "Spremi rezultat"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                picOutput.Image.Save(sfd.FileName);
                lblStatus.Text = "Slika spremljena.";
            }
        }

        // --------------------------------------------------------
        // CLEAR OUTPUT
        // --------------------------------------------------------
        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            if (picOutput.Image != null)
            {
                picOutput.Image.Dispose();
                picOutput.Image = null;
            }

            string tempFile = Path.Combine(Path.GetTempPath(), "style_output.jpg");
            try
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
            catch
            {
                // ignore cleanup failure
            }

            lblStatus.Text = "Output ociscen.";
        }

        // --------------------------------------------------------
        // RELOAD TEMPLATES
        // --------------------------------------------------------
        private void btnReloadStyles_Click(object sender, EventArgs e)
        {
            LoadStyleTemplates();
        }

        // --------------------------------------------------------
        // ADD CUSTOM STYLE
        // --------------------------------------------------------
        private void btnAddStyle_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "Slike|*.jpg;*.jpeg;*.png",
                Title = "Odaberi sliku za stil"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            string fileName = Path.GetFileName(ofd.FileName);
            string destPath = Path.Combine(SessionStyleFolder, fileName);
            string nameNoExt = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            int counter = 1;
            while (File.Exists(destPath))
            {
                destPath = Path.Combine(SessionStyleFolder, $"{nameNoExt}_{counter}{ext}");
                counter++;
            }

            File.Copy(ofd.FileName, destPath);
            SessionStylePaths.Add(destPath);

            // Refresh and select the new style
            LoadStyleTemplates();
            SelectStyleThumbnail(destPath);
            lblStatus.Text = "Novi stil dodan (samo za ovu sesiju).";
        }

        // --------------------------------------------------------
        // PROCESS OUTPUT HANDLERS
        // --------------------------------------------------------
        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                AppendLog(e.Data.Trim());
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                AppendLog("ERR: " + e.Data.Trim());
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

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
