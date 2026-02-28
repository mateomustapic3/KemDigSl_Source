using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form3 : Form
    {
        private static readonly Color ThemeBg = Color.FromArgb(30, 30, 30);
        private static readonly Color ThemePanel = Color.FromArgb(35, 35, 35);
        private static readonly Color ThemeButton = Color.FromArgb(70, 70, 120);
        private static readonly Color ThemeText = Color.FromArgb(45, 45, 45);

        private string lastOutputImage = "";
        private string? currentInputImage;

        public Form3()
        {
            InitializeComponent();
            AllowDrop = true;
            DragEnter += Form3_DragEnter;
            DragDrop += Form3_DragDrop;
            InitializeCustomControls();
            ApplyTheme();
        }

        private void InitializeCustomControls()
        {
            cboUpscale.Items.Clear();
            cboUpscale.Items.Add("x1 (bez povecanja)");
            cboUpscale.Items.Add("x2");
            cboUpscale.Items.Add("x4");
            cboUpscale.SelectedIndex = 1; // default x2

            lblStatus.Text = "Spreman.";
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;
            btnSaveAs.Enabled = false;
        }

        private void UseOutputAsInput()
        {
            if (string.IsNullOrEmpty(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("Nema izlazne slike za ponovno korištenje.");
                return;
            }

            ApplyInputPath(lastOutputImage);
            UpdateStatus("Izlaz je postavljen kao novi input.");
        }

        private static bool IsImageFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".png" or ".jpg" or ".jpeg" or ".bmp";
        }

        private void ApplyInputPath(string path)
        {
            currentInputImage = path;
            LoadImageSafe(picInput, path);
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ApplyInputPath(ofd.FileName);
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png";
                if (!string.IsNullOrWhiteSpace(txtOutput.Text))
                {
                    sfd.InitialDirectory = Path.GetDirectoryName(txtOutput.Text);
                    sfd.FileName = Path.GetFileName(txtOutput.Text);
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtOutput.Text = sfd.FileName;
                }
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            string inputPath = currentInputImage ?? "";
            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                MessageBox.Show("Odaberi valjanu ulaznu sliku.", "Greska",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string outputPath = Path.Combine(Path.GetTempPath(), $"gfpgan_{Guid.NewGuid():N}.png");

            int upscale = 2;
            switch (cboUpscale.SelectedIndex)
            {
                case 0: upscale = 1; break;
                case 1: upscale = 2; break;
                case 2: upscale = 4; break;
            }

            bool onlyCenterFace = chkOnlyCenterFace.Checked;
            bool pasteBack = chkPasteBack.Checked;

            btnRun.Enabled = false;
            btnBrowseInput.Enabled = false;
            btnSaveAs.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;
            Cursor = Cursors.WaitCursor;
            UpdateStatus("Pokrecem GFPGAN...");

            try
            {
                var result = await Task.Run(() =>
                    RunGfpganProcess(inputPath, outputPath, upscale, onlyCenterFace, pasteBack)
                );

                if (result.ExitCode == 0 && File.Exists(outputPath))
                {
                    UpdateStatus("Gotovo.");
                    LoadImageSafe(picOutput, outputPath);
                    lastOutputImage = outputPath;
                    btnSaveAs.Enabled = true;
                }
                else
                {
                    UpdateStatus("Greska pri obradi.");
                    MessageBox.Show(
                        "GFPGAN nije uspio.\n\n" +
                        "Exit code: " + result.ExitCode + "\n\n" +
                        "STDOUT:\n" + result.StdOut + "\n\n" +
                        "STDERR:\n" + result.StdErr,
                        "GFPGAN greska",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Neocekivana greska.");
                MessageBox.Show("Neocekivana greska: " + ex.Message, "Greska",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRun.Enabled = true;
                btnBrowseInput.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.MarqueeAnimationSpeed = 0;
                progressBar.Value = 0;
                Cursor = Cursors.Default;
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("Nema izlazne slike za spremiti.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG|*.png|JPEG|*.jpg;*.jpeg|Bitmap|*.bmp",
                FileName = $"{Path.GetFileNameWithoutExtension(currentInputImage ?? "gfpgan")}_gfpgan"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(lastOutputImage, sfd.FileName, true);
                    UpdateStatus("Spremljeno.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska pri spremanju: " + ex.Message);
                }
            }
        }

        private void btnUseOutput_Click(object sender, EventArgs e)
        {
            UseOutputAsInput();
        }

        private class GfpganResult
        {
            public int ExitCode { get; set; }
            public string StdOut { get; set; } = "";
            public string StdErr { get; set; } = "";
        }

        private GfpganResult RunGfpganProcess(
            string inputPath,
            string outputPath,
            int upscale,
            bool onlyCenterFace,
            bool pasteBack)
        {
            string projectBase = AppPaths.FindAppRoot();
            string pythonExe = AppPaths.FindPythonExe();
            string scriptPath = AppPaths.ResolveFile("python", "gfpgan", "run_gfpgan.py");

            if (!string.Equals(pythonExe, "python.exe", StringComparison.OrdinalIgnoreCase) && !File.Exists(pythonExe))
            {
                return new GfpganResult
                {
                    ExitCode = -1,
                    StdOut = "",
                    StdErr = "Python nije pronaden: " + pythonExe
                };
            }

            if (!File.Exists(scriptPath))
            {
                return new GfpganResult
                {
                    ExitCode = -2,
                    StdOut = "",
                    StdErr = "GFPGAN skripta nije pronadena: " + scriptPath
                };
            }

            string args =
                $"\"{scriptPath}\" \"{inputPath}\" \"{outputPath}\" --upscale {upscale}";

            if (onlyCenterFace)
            {
                args += " --only-center-face";
            }
            if (!pasteBack)
            {
                args += " --no-paste-back";
            }

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = args,
                WorkingDirectory = Path.GetDirectoryName(scriptPath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var proc = new Process())
            {
                proc.StartInfo = psi;
                proc.Start();

                string stdOut = proc.StandardOutput.ReadToEnd();
                string stdErr = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                return new GfpganResult
                {
                    ExitCode = proc.ExitCode,
                    StdOut = stdOut,
                    StdErr = stdErr
                };
            }
        }

        private void UpdateStatus(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => lblStatus.Text = text));
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        private void LoadImageSafe(PictureBox box, string path)
        {
            try
            {
                if (!File.Exists(path))
                    return;

                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Image img = Image.FromStream(fs);
                var old = box.Image;
                box.Image = (Image)img.Clone();
                img.Dispose();
                old?.Dispose();
            }
            catch
            {
            }
        }

        private void Form3_DragEnter(object? sender, DragEventArgs e)
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

        private void Form3_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string? first = files.FirstOrDefault(IsImageFile);
            if (first == null)
                return;

            try
            {
                ApplyInputPath(first);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mogu ucitati sliku: " + ex.Message);
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
            panelCenter.BackColor = Theme.Panel;

            picInput.BackColor = Color.Black;
            picOutput.BackColor = Color.Black;

            Theme.StyleButton(btnBrowseInput, Theme.BtnPrimary);
            Theme.StyleButton(btnBrowseOutput, Theme.BtnAccent);
            Theme.StyleButton(btnRun, Theme.BtnAction);
            Theme.StyleButton(btnUseOutput, Theme.BtnAccent);
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);

            Theme.StyleComboBox(cboUpscale);

            txtInput.BackColor = Theme.SidePanel;
            txtInput.ForeColor = Theme.Text;
            txtOutput.BackColor = Theme.SidePanel;
            txtOutput.ForeColor = Theme.Text;

            chkOnlyCenterFace.ForeColor = Theme.Text;
            chkPasteBack.ForeColor = Theme.Text;

            foreach (var lbl in new[] { lblTitle, lblStatus, lblInputPreview, lblOutputPreview })
                Theme.StyleLabel(lbl);

            progressBar.BackColor = Theme.Panel;
        }
    }
}
