using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Project
{
    public partial class Form1 : Form
    {
        private static readonly Color ThemeBg = Color.FromArgb(30, 30, 30);
        private static readonly Color ThemePanel = Color.FromArgb(35, 35, 35);
        private static readonly Color ThemeButton = Color.FromArgb(70, 70, 120);
        private static readonly Color ThemeText = Color.FromArgb(45, 45, 45);
        private Label? overlayLabel;

        public Form1()
        {
            InitializeComponent();

            // Event handleri
            btnLoadImage.Click += BtnLoadImage_Click;
            btnTransform.Click += btnTransform_Click;
            btnSaveAs.Click += BtnSaveAs_Click;
            comboBoxTransform.SelectedIndexChanged += (_, __) => UpdateParamUI();
            trackParam.Scroll += (_, __) => UpdateParamValueLabel();
            btnReset.Click += (_, __) => ResetOutput();
            btnUseOutput.Click += (_, __) => UseOutputAsInput();
            AllowDrop = true;
            DragEnter += Form1_DragEnter;
            DragDrop += Form1_DragDrop;

            ApplyTheme();
            InitOverlay();

            // ComboBox transformacije
            comboBoxTransform.Items.AddRange(new string[]
            {
                "invert",
                "grayscale",
                "blur",
                "rotate_90",
                "rotate_custom",
                "mirror",
                "flip",
                "sepia",
                "brightness",
                "contrast",
                "saturation",
                "sharpness"
            });
            comboBoxTransform.SelectedIndex = 0;
            UpdateParamUI();
        }

        private void ApplyTheme()
        {
            BackColor = Theme.Background;
            panelControls.BackColor = Theme.PanelAlt;
            panelImages.BackColor = Theme.Panel;
            panelTop.BackColor = Theme.PanelAlt;
            panelCenter.BackColor = Theme.Panel;

            pictureBoxInput.BackColor = Color.Black;
            pictureBoxOutput.BackColor = Color.Black;

            Theme.StyleButton(btnLoadImage, Theme.BtnPrimary);
            Theme.StyleButton(btnTransform, Theme.BtnAction);
            Theme.StyleButton(btnSaveAs, Theme.BtnSave);
            Theme.StyleButton(btnReset, Theme.BtnClear);
            Theme.StyleButton(btnUseOutput, Theme.BtnAccent);

            Theme.StyleComboBox(comboBoxTransform);
            Theme.StyleTrackBar(trackParam);
            progressBar.BackColor = Theme.Panel;

            foreach (var lbl in new[] { form1Title, lblInputHeader, lblOutputHeader, lblParam, lblParamValue })
                Theme.StyleLabel(lbl);
        }

        // Koristimo %TEMP% umjesto lokalnog "images" foldera uz aplikaciju.
        private string inputPath => Path.Combine(Path.GetTempPath(), "WindowsFormsApp", "BasicTransforms", "input.jpg");
        private string outputPath => Path.Combine(Path.GetTempPath(), "WindowsFormsApp", "BasicTransforms", "output.jpg");
        private static string PythonExe => AppPaths.FindPythonExe();
        private static string TransformScriptPath => AppPaths.ResolveFile("python", "transform.py");

        private static bool IsImageFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".jpg" or ".jpeg" or ".png" or ".bmp";
        }

        private void LoadInputFromPath(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            using var img = Image.FromStream(ms);
            pictureBoxInput.Image?.Dispose();
            pictureBoxInput.Image = new Bitmap(img);
            Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);
            pictureBoxInput.Image.Save(inputPath);
        }

        private double MapParam(string operation, int sliderValue)
        {
            return operation switch
            {
                "blur" => Math.Round(sliderValue / 10.0, 1),
                "rotate_custom" => Math.Round(sliderValue * 3.6, 1),
                "brightness" => Math.Round(Math.Pow(2, (sliderValue - 50) / 12.5), 2),
                "contrast" => Math.Round(Math.Pow(2, (sliderValue - 50) / 12.5), 2),
                "saturation" => Math.Round(Math.Pow(2, (sliderValue - 50) / 25.0), 2),
                "sharpness" => Math.Round(sliderValue / 25.0, 2),
                _ => 0
            };
        }

        private void UpdateParamValueLabel()
        {
            string op = comboBoxTransform.SelectedItem?.ToString() ?? "";
            bool needsParam = new[] { "blur", "rotate_custom", "brightness", "contrast", "saturation", "sharpness" }.Contains(op);
            lblParamValue.Text = needsParam
                ? MapParam(op, trackParam.Value).ToString("0.##", CultureInfo.InvariantCulture)
                : "-";
        }

        private void UpdateParamUI()
        {
            string op = comboBoxTransform.SelectedItem?.ToString() ?? "";
            bool needsParam = new[] { "blur", "rotate_custom", "brightness", "contrast", "saturation", "sharpness" }.Contains(op);

            trackParam.Enabled = needsParam;
            lblParam.Visible = true;
            lblParamValue.Visible = true;
            UpdateParamValueLabel();

            if (!needsParam)
            {
                lblParam.Text = "Parametar (n/a)";
                trackParam.Enabled = false;
            }
            else
            {
                lblParam.Text = op switch
                {
                    "blur" => "Blur radius",
                    "rotate_custom" => "Rotate angle (0-360)",
                    "brightness" => "Brightness (0.07x-16x)",
                    "contrast" => "Contrast (0.07x-16x)",
                    "saturation" => "Saturation (0.25x-4x)",
                    "sharpness" => "Sharpness (0-4x)",
                    _ => "Parametar"
                };
            }
        }
        private async void btnTransform_Click(object sender, EventArgs e)
        {
            if (pictureBoxInput.Image == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            btnTransform.Enabled = false;
            progressBar.Visible = true;
            ShowOverlay(true);

            try
            {
                string pythonExe = PythonExe;
                string scriptPath = TransformScriptPath;
                if (string.IsNullOrWhiteSpace(scriptPath) || !File.Exists(scriptPath))
                {
                    MessageBox.Show("Skripta nije pronađena: python/transform.py");
                    return;
                }

                // Privremeno spremimo input sliku samo za Python
                string tempInput = Path.Combine(Path.GetTempPath(), "py_input.png");
                pictureBoxInput.Image.Save(tempInput);

                string operation = comboBoxTransform.SelectedItem?.ToString() ?? "invert";
                double paramValue = MapParam(operation, trackParam.Value);
                string paramArg = paramValue.ToString(CultureInfo.InvariantCulture);

                var psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = $"\"{scriptPath}\" \"{tempInput}\" \"{operation}\" \"{paramArg}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                if (process == null)
                    throw new Exception("Ne mogu pokrenuti Python proces.");

                var stdOutTask = process.StandardOutput.ReadToEndAsync();
                var stdErrTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(stdOutTask, stdErrTask);
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception($"Python greška (exit {process.ExitCode}).\n{stdErrTask.Result}");

                string base64 = (stdOutTask.Result ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(base64))
                    throw new Exception("Python nije vratio base64 output.");

                byte[] bytes;
                try
                {
                    bytes = Convert.FromBase64String(base64);
                }
                catch (FormatException)
                {
                    throw new Exception($"Python output nije valjani base64.\nSTDERR:\n{stdErrTask.Result}\n\nSTDOUT:\n{base64}");
                }

                using var ms = new MemoryStream(bytes);
                using var img = Image.FromStream(ms);
                pictureBoxOutput.Image?.Dispose();
                pictureBoxOutput.Image = new Bitmap(img);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnTransform.Enabled = true;
                progressBar.Visible = false;
                ShowOverlay(false);
            }
        }
        private void BtnLoadImage_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string? err;
                var bmp = LoadImageSafe(ofd.FileName, out err);
                if (bmp == null)
                {
                    MessageBox.Show("Ne mogu ucitati sliku: " + err);
                    return;
                }

                pictureBoxInput.Image?.Dispose();
                pictureBoxInput.Image = bmp;

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);
                    bmp.Save(inputPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ne mogu spremiti ulaznu kopiju: " + ex.Message);
                }
            }
        }

        private void BtnSaveAs_Click(object? sender, EventArgs e)
        {
            if (pictureBoxOutput.Image == null)
            {
                MessageBox.Show("No transformed image to save.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp",
                FileName = "output"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxOutput.Image.Save(sfd.FileName);
            }
        }


        private void ResetOutput()
        {
            pictureBoxOutput.Image?.Dispose();
            pictureBoxOutput.Image = null;
            progressBar.Visible = false;
        }

        private void UseOutputAsInput()
        {
            if (pictureBoxOutput.Image == null)
            {
                MessageBox.Show("No output to reuse.");
                return;
            }

            pictureBoxInput.Image?.Dispose();
            pictureBoxInput.Image = new Bitmap(pictureBoxOutput.Image);
            pictureBoxOutput.Image?.Dispose();
            pictureBoxOutput.Image = null;
        }

        private Bitmap? LoadImageSafe(string path, out string? error)
        {
            error = null;
            if (!File.Exists(path))
            {
                error = "Datoteka ne postoji.";
                return null;
            }

            try
            {
                // 1) Bitmap ctor (najtolerantniji)
                using var bmpFile = new Bitmap(path);
                return new Bitmap(bmpFile);
            }
            catch
            {
                // 2) Pokusaj iz memorije bez validateImageData (neke JPEG varijante)
                try
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    if (bytes.Length == 0)
                    {
                        error = "Datoteka je prazna.";
                        return null;
                    }

                    using var ms = new MemoryStream(bytes);
                    using var temp = Image.FromStream(ms, useEmbeddedColorManagement: false, validateImageData: false);
                    return new Bitmap(temp);
                }
                catch (Exception ex2)
                {
                    // 3) Pokusaj preko FileStream bez CM/validation
                    try
                    {
                        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using var temp = Image.FromStream(fs, useEmbeddedColorManagement: false, validateImageData: false);
                        return new Bitmap(temp);
                    }
                    catch (Exception ex3)
                    {
                        error = $"{ex2.Message}; {ex3.Message}";
                        return null;
                    }
                }
            }
        }

        private static string? GetPythonExePath()
        {
            return AppPaths.FindPythonExe();
        }

        private void Form1_DragEnter(object? sender, DragEventArgs e)
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

        private void Form1_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string? first = files.FirstOrDefault(IsImageFile);
            if (first == null)
                return;

            try
            {
                LoadInputFromPath(first);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load image: " + ex.Message);
            }
        }

        private void pictureBoxInput_Click(object sender, EventArgs e)
        {

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
            pictureBoxOutput.Controls.Add(overlayLabel);
            overlayLabel.BringToFront();
        }

        private void ShowOverlay(bool show)
        {
            if (overlayLabel != null)
                overlayLabel.Visible = show;
        }
    }
}
