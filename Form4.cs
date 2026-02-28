using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form4 : Form
    {
        private static readonly Color ThemeBg = Color.FromArgb(30, 30, 30);
        private static readonly Color ThemePanel = Color.FromArgb(35, 35, 35);
        private static readonly Color ThemeButton = Color.FromArgb(70, 70, 120);
        private static readonly Color ThemeText = Color.FromArgb(45, 45, 45);

        private string selectedImage = string.Empty;
        private string lastOutputImage = string.Empty;
        private Size? originalSize;

        private readonly string projectRoot;
        private readonly string pythonExe;
        private readonly string codeformerDir;
        private readonly string imagesRoot;

        public Form4()
        {
            InitializeComponent();

            projectRoot = AppPaths.FindAppRoot();
            pythonExe = AppPaths.FindPythonExe();
            string cfDir = AppPaths.ResolveDirectory("CODEFORMER");
            codeformerDir = string.IsNullOrWhiteSpace(cfDir) ? Path.Combine(projectRoot, "CODEFORMER") : cfDir;

            // Privremeni work/output folder (ne koristi projectRoot\\images).
            imagesRoot = Path.Combine(Path.GetTempPath(), "WindowsFormsApp", "CodeFormer");

            Directory.CreateDirectory(imagesRoot);

            InitUi();
            ApplyTheme();
        }

        private void UseOutputAsInput()
        {
            if (string.IsNullOrEmpty(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("Nema izlazne slike za ponovno korištenje.");
                return;
            }

            try
            {
                LoadInputImage(lastOutputImage);
                lblStatus.Text = "Izlaz je postavljen kao novi input.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mogu učitati izlaznu sliku: " + ex.Message);
            }
        }

        private void InitUi()
        {
            trackCFWeight.Minimum = 0;
            trackCFWeight.Maximum = 100;
            if (trackCFWeight.Value == 0)
                trackCFWeight.Value = 50;

            lblCFValue.Text = (trackCFWeight.Value / 100.0).ToString("0.00", CultureInfo.InvariantCulture);
            trackCFWeight.Scroll += (s, e) =>
            {
                lblCFValue.Text = (trackCFWeight.Value / 100.0).ToString("0.00", CultureInfo.InvariantCulture);
            };

            comboDetectionModel.Items.Clear();
            comboDetectionModel.Items.Add("retinaface_resnet50");
            comboDetectionModel.Items.Add("retinaface_mobile0.25");
            comboDetectionModel.SelectedIndex = 0;

            numUpscale.Minimum = 1;
            numUpscale.Maximum = 4;
            numUpscale.Value = 2;

            chkFaceUpsample.Checked = true;
            chkBgUpsample.Checked = false;
            chkOnlyCenterFace.Checked = true;
            chkDrawBox.Checked = false;

            btnSaveAs.Enabled = false;
            lblStatus.Text = "Spreman.";
            progressBar.Style = ProgressBarStyle.Blocks;

            AllowDrop = true;
            DragEnter += Form4_DragEnter;
            DragDrop += Form4_DragDrop;
        }

        private static Image LoadUnlocked(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            using var img = Image.FromStream(ms);
            return (Image)img.Clone();
        }

        private static bool IsSupportedImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".png" or ".jpg" or ".jpeg" or ".bmp";
        }

        private static Image ResizeTo(Image src, Size size)
        {
            var dest = new Bitmap(size.Width, size.Height);
            using var g = Graphics.FromImage(dest);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(src, 0, 0, size.Width, size.Height);
            return dest;
        }

        private void LoadInputImage(string path)
        {
            selectedImage = path;
            txtInput.Text = selectedImage;
            originalSize = null;

            pbInput.Image = LoadUnlocked(selectedImage);
            pbOutput.Image = null;
            lastOutputImage = string.Empty;
            btnSaveAs.Enabled = false;

            lblStatus.Text = "Slika učitana.";
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadInputImage(ofd.FileName);
            }
        }

        private string PrepareOutputDir()
        {
            string cfOutDir = Path.Combine(imagesRoot, "codeformer_out");
            try
            {
                if (Directory.Exists(cfOutDir))
                    Directory.Delete(cfOutDir, true);
            }
            catch
            {
            }

            Directory.CreateDirectory(cfOutDir);
            return cfOutDir;
        }

        private string? GetNewestImage(string folder)
        {
            if (!Directory.Exists(folder))
                return null;

            var candidates = Directory
                .GetFiles(folder, "*.png", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(folder, "*.jpg", SearchOption.AllDirectories))
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .ToList();

            return candidates.FirstOrDefault();
        }

        private async Task<string?> RunCodeFormer(string inputPath, double weight)
        {
            if (!string.Equals(pythonExe, "python.exe", StringComparison.OrdinalIgnoreCase) && !File.Exists(pythonExe))
            {
                MessageBox.Show($"PythonRuntime/python.exe nije pronađen:\n{pythonExe}",
                    "CodeFormer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string inferScript = Path.Combine(codeformerDir, "inference_codeformer.py");
            if (!File.Exists(inferScript))
            {
                MessageBox.Show(
                    "CodeFormer nije pronađen.\n" +
                    "Očekujem CODEFORMER\\inference_codeformer.py i modele u CODEFORMER\\weights.",
                    "CodeFormer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string cfWeight = Path.Combine(codeformerDir, "weights", "CodeFormer", "codeformer.pth");
            if (!File.Exists(cfWeight))
            {
                MessageBox.Show(
                    "Nedostaje CodeFormer model:\n" + cfWeight,
                    "CodeFormer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            bool faceUpsample = chkFaceUpsample.Checked;
            bool bgUpsample = chkBgUpsample.Checked;
            bool onlyCenter = chkOnlyCenterFace.Checked;
            bool drawBox = chkDrawBox.Checked;

            string realesrganX2 = Path.Combine(codeformerDir, "weights", "RealESRGAN_x2plus.pth");
            if ((faceUpsample || bgUpsample) && !File.Exists(realesrganX2))
            {
                MessageBox.Show(
                    "Nedostaje RealESRGAN_x2plus model potreban za upsample:\n" + realesrganX2 +
                    "\n\nIsključi Face/Background upsample ili dodaj model.",
                    "CodeFormer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string outputRoot = PrepareOutputDir();

            string weightArg = weight.ToString("0.00", CultureInfo.InvariantCulture);
            int upscale = (int)numUpscale.Value;
            string detectionModel = comboDetectionModel.SelectedItem?.ToString() ?? "retinaface_resnet50";

            string args =
                $"\"{inferScript}\" " +
                $"-i \"{inputPath}\" " +
                $"-o \"{outputRoot}\" " +
                $"-w {weightArg} " +
                $"-s {upscale} " +
                $"--detection_model {detectionModel} ";

            if (onlyCenter)
                args += "--only_center_face ";
            if (drawBox)
                args += "--draw_box ";

            if (bgUpsample)
                args += "--bg_upsampler realesrgan ";
            else
                args += "--bg_upsampler None ";

            if (faceUpsample)
                args += "--face_upsample ";

            string stdOut = string.Empty;
            string stdErr = string.Empty;

            await Task.Run(async () =>
            {
                using var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pythonExe,
                        Arguments = args,
                        WorkingDirectory = codeformerDir,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                var readOut = proc.StandardOutput.ReadToEndAsync();
                var readErr = proc.StandardError.ReadToEndAsync();
                await Task.WhenAll(readOut, readErr);
                proc.WaitForExit();
                stdOut = readOut.Result;
                stdErr = readErr.Result;
            });

            var outputImg = GetNewestImage(outputRoot);
            if (outputImg == null)
            {
                MessageBox.Show(
                    "CodeFormer nije generirao izlaz.\n\n" +
                    "STDOUT:\n" + stdOut + "\n\n" +
                    "STDERR:\n" + stdErr,
                    "CodeFormer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return outputImg;
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedImage) || !File.Exists(selectedImage))
            {
                MessageBox.Show("Odaberite ulaznu sliku.");
                return;
            }

            originalSize ??= pbInput.Image?.Size;

            btnRestore.Enabled = false;
            btnSaveAs.Enabled = false;
            btnSelect.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Pokrećem CodeFormer...";

            try
            {
                double weight = trackCFWeight.Value / 100.0;

                var cfOutput = await RunCodeFormer(selectedImage, weight);
                if (cfOutput == null)
                    return;

                lastOutputImage = cfOutput;
                pbOutput.Image = LoadUnlocked(cfOutput);
                btnSaveAs.Enabled = true;
                lblStatus.Text = "Gotovo.";
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                btnRestore.Enabled = true;
                btnSelect.Enabled = true;
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("Nema izlazne slike.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp",
                FileName = Path.GetFileName(lastOutputImage)
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(lastOutputImage, sfd.FileName, true);
                lblStatus.Text = "Spremljeno.";
            }
        }

        private void Form4_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Any(IsSupportedImage))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void Form4_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string? firstImage = files.FirstOrDefault(IsSupportedImage);
            if (firstImage == null)
                return;

            try
            {
                LoadInputImage(firstImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mogu učitati sliku: " + ex.Message);
            }
        }

        private void ApplyDarkTheme(Control root)
        {
            root.BackColor = root is Form ? ThemeBg : root.BackColor;
            foreach (Control c in root.Controls)
            {
                ApplyDarkTheme(c);
                switch (c)
                {
                    case Panel or GroupBox:
                        c.BackColor = ThemePanel;
                        break;
                    case SplitContainer sc:
                        sc.BackColor = ThemePanel;
                        sc.Panel1.BackColor = ThemePanel;
                        sc.Panel2.BackColor = ThemePanel;
                        break;
                    case Button b:
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 0;
                        b.BackColor = ThemeButton;
                        b.ForeColor = Color.White;
                        break;
                    case Label lbl:
                        lbl.ForeColor = Color.White;
                        break;
                    case TextBox tb:
                        tb.BackColor = ThemeText;
                        tb.ForeColor = Color.White;
                        break;
                    case ComboBox cb:
                        cb.BackColor = ThemeText;
                        cb.ForeColor = Color.White;
                        break;
                    case NumericUpDown nud:
                        nud.BackColor = ThemeText;
                        nud.ForeColor = Color.White;
                        break;
                    case TrackBar track:
                        track.BackColor = ThemePanel;
                        break;
                    case ProgressBar pb:
                        pb.BackColor = ThemePanel;
                        break;
                }
            }
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelImages.BackColor = Theme.Panel;
            panelControls.BackColor = Theme.PanelAlt;
            panelStatus.BackColor = Theme.PanelAlt;
            panelTop.BackColor = Theme.PanelAlt;
            grpSettings.BackColor = Theme.Panel;
            grpSettings.ForeColor = Theme.Text;

            pbInput.BackColor = Color.Black;
            pbOutput.BackColor = Color.Black;

            Theme.StyleButton(btnSelect, Theme.BtnPrimary);
            Theme.StyleButton(btnRestore, Theme.BtnAction);
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);
            Theme.StyleButton(btnUseOutput, Theme.BtnAccent);

            Theme.StyleComboBox(comboDetectionModel);
            Theme.StyleTrackBar(trackCFWeight);

            txtInput.BackColor = Theme.SidePanel;
            txtInput.ForeColor = Theme.Text;

            foreach (var lbl in new[] { lblTitle, lblInput, lblOutput, lblCFValue, lblStatus, lblWeight, lblDetectionModel, lblUpscale })
                Theme.StyleLabel(lbl);

            chkFaceUpsample.ForeColor = Theme.Text;
            chkBgUpsample.ForeColor = Theme.Text;
            chkOnlyCenterFace.ForeColor = Theme.Text;
            chkDrawBox.ForeColor = Theme.Text;

            progressBar.BackColor = Theme.Panel;
        }

        private void btnUseOutput_Click(object sender, EventArgs e)
        {
            UseOutputAsInput();
        }

        private void chkFaceUpsample_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
