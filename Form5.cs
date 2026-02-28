using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;

namespace Project
{
    public partial class Form5 : Form
    {
        private static readonly Color ThemeBg = Color.FromArgb(30, 30, 30);
        private static readonly Color ThemePanel = Color.FromArgb(35, 35, 35);
        private static readonly Color ThemeButton = Color.FromArgb(70, 70, 120);
        private static readonly Color ThemeText = Color.FromArgb(45, 45, 45);
        private string lastOutputImage = "";
        private string? currentInputImage;

        public Form5()
        {
            InitializeComponent();
            AllowDrop = true;
            DragEnter += Form5_DragEnter;
            DragDrop += Form5_DragDrop;
            InitializeCustomControls();
            ApplyTheme();
        }

        private void InitializeCustomControls()
        {
            lblStatus.Text = "Spreman.";
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;

            trackStrength.Minimum = 0;
            trackStrength.Maximum = 100;
            trackStrength.TickFrequency = 10;
            if (trackStrength.Value == 0)
                trackStrength.Value = 80;
            lblStrengthValue.Text = $"{trackStrength.Value}%";
            trackStrength.Scroll += (s, e) =>
            {
                lblStrengthValue.Text = $"{trackStrength.Value}%";
            };

            btnSaveAs.Enabled = false;
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

        private void UseOutputAsInput()
        {
            if (string.IsNullOrWhiteSpace(lastOutputImage) || !File.Exists(lastOutputImage))
            {
                MessageBox.Show("Nema izlazne slike za ponovno korištenje.");
                return;
            }

            ApplyInputPath(lastOutputImage);
            UpdateStatus("Izlaz je postavljen kao novi input.");
        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ApplyInputPath(ofd.FileName);
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image|*.png"
            };

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

        private async void btnRun_Click(object sender, EventArgs e)
        {
            string inputPath = currentInputImage ?? "";

            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                MessageBox.Show("Odaberi valjanu ulaznu sliku.", "Greska",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRun.Enabled = false;
            btnBrowseInput.Enabled = false;
            btnSaveAs.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;
            Cursor = Cursors.WaitCursor;
            UpdateStatus("Pokrecem DDColor...");

            try
            {
                double strength = trackStrength.Value / 100.0;
                string tempOutput = Path.Combine(Path.GetTempPath(), $"ddcolor_preview_{Guid.NewGuid():N}.png");

                var result = await Task.Run(() => RunDdcolorProcess(inputPath, tempOutput, strength));

                if (result.ExitCode == 0 && File.Exists(tempOutput))
                {
                    UpdateStatus("Gotovo.");
                    LoadImageSafe(picOutput, tempOutput);
                    lastOutputImage = tempOutput;
                    btnSaveAs.Enabled = true;
                }
                else
                {
                    UpdateStatus("Greska pri obradi.");
                    MessageBox.Show(
                        "DDColor nije uspio.\n\n" +
                        "Exit code: " + result.ExitCode + "\n\n" +
                        "STDOUT:\n" + result.StdOut + "\n\n" +
                        "STDERR:\n" + result.StdErr,
                        "DDColor greska",
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

        private class DdcolorResult
        {
            public int ExitCode { get; set; }
            public string StdOut { get; set; } = "";
            public string StdErr { get; set; } = "";
        }

        private DdcolorResult RunDdcolorProcess(string inputPath, string outputPath, double strength)
        {
            string projectBase = AppPaths.FindAppRoot();
            string pythonExe = AppPaths.FindPythonExe();
            string scriptPath = AppPaths.ResolveFile("DDCOLOR", "ddcolor_run.py");

            if (!string.Equals(pythonExe, "python.exe", StringComparison.OrdinalIgnoreCase) && !File.Exists(pythonExe))
            {
                return new DdcolorResult
                {
                    ExitCode = -1,
                    StdErr = "Python nije pronađen: " + pythonExe
                };
            }

            if (!File.Exists(scriptPath))
            {
                return new DdcolorResult
                {
                    ExitCode = -2,
                    StdErr = "DDColor skripta nije pronađena: " + scriptPath
                };
            }

            string strengthArg = strength.ToString("0.00", CultureInfo.InvariantCulture);
            string args = $"\"{scriptPath}\" \"{inputPath}\" \"{outputPath}\" {strengthArg}";

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = args,
                WorkingDirectory = Path.GetDirectoryName(scriptPath) ?? projectBase,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var proc = new Process();
            proc.StartInfo = psi;
            proc.Start();

            string stdOut = proc.StandardOutput.ReadToEnd();
            string stdErr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            return new DdcolorResult
            {
                ExitCode = proc.ExitCode,
                StdOut = stdOut,
                StdErr = stdErr
            };
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
                // swallow preview load errors
            }
        }

        private void Form5_DragEnter(object? sender, DragEventArgs e)
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

        private void Form5_DragDrop(object? sender, DragEventArgs e)
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
                MessageBox.Show("Ne mogu učitati sliku: " + ex.Message);
            }
        }

        private void btnUseOutput_Click(object sender, EventArgs e)
        {
            UseOutputAsInput();
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
                FileName = $"{Path.GetFileNameWithoutExtension(currentInputImage ?? "ddcolor")}_ddcolor"
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
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);
            Theme.StyleButton(btnUseOutput, Theme.BtnAccent);

            txtInput.BackColor = Theme.SidePanel;
            txtInput.ForeColor = Theme.Text;
            txtOutput.BackColor = Theme.SidePanel;
            txtOutput.ForeColor = Theme.Text;

            Theme.StyleTrackBar(trackStrength);
            progressBar.BackColor = Theme.Panel;

            foreach (var lbl in new[] { lblStrengthValue, lblStrength, lblStatus, lblTitle, lblInputPreview, lblOutputPreview })
                Theme.StyleLabel(lbl);
        }

        private void panelControls_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
