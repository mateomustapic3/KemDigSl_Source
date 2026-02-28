using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Project
{
    public partial class Form7 : Form
    {
        private string? inputImagePath;
        private Bitmap? originalImage;
        private readonly List<Detection> detections = new();

        private class Detection
        {
            public string Label { get; set; } = "";
            public float Confidence { get; set; }
            public RectangleF Box { get; set; }
        }

        public Form7()
        {
            InitializeComponent();
            UpdateConfLabel();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        private string GetProjectRoot()
        {
            return AppPaths.FindAppRoot();
        }

        private string? GetPythonInterpreter()
        {
            // Prefer embedded PythonRuntime, then (optional) venvs, then system python.
            string projectRoot = GetProjectRoot();

            string embedded = AppPaths.FindPythonExe();
            if (!string.Equals(embedded, "python.exe", StringComparison.OrdinalIgnoreCase) && File.Exists(embedded))
                return embedded;

            string detectVenv = Path.Combine(projectRoot, "DETECTION", "venv", "Scripts", "python.exe");
            if (File.Exists(detectVenv)) return detectVenv;

            string styleVenv = Path.Combine(projectRoot, "STYLE_TRANSFER", "venv", "Scripts", "python.exe");
            if (File.Exists(styleVenv)) return styleVenv;

            // fallback: rely on PATH
            return "python";
        }

        // ---------------------- UI HANDLERS -----------------------

        private Bitmap LoadUnlockedImage(string path)
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

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new()
            {
                Filter = "Slike|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Odaberi sliku za detekciju"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                inputImagePath = ofd.FileName;

                try
                {
                    originalImage?.Dispose();
                    picDetection.Image?.Dispose();

                    if (!File.Exists(inputImagePath))
                    {
                        MessageBox.Show("Datoteka ne postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    originalImage = LoadUnlockedImage(inputImagePath);
                    picDetection.Image = new Bitmap(originalImage);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Greška pri učitavanju slike.";
                    MessageBox.Show("Slika se ne može učitati: " + ex.Message + "\nDatoteka je možda oštećena ili nije podržan format.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                detections.Clear();
                listDetections.Items.Clear();
                txtLog.Clear();
                progressBar.Value = 0;

                lblStatus.Text = $"Učitana slika: {Path.GetFileName(inputImagePath)}";
            }
        }

        private void trackConfidence_Scroll(object sender, EventArgs e)
        {
            UpdateConfLabel();
        }

        private void UpdateConfLabel()
        {
            double val = trackConfidence.Value / 100.0;
            lblConfValue.Text = val.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void btnRunDetection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputImagePath) || originalImage == null)
            {
                MessageBox.Show("Najprije učitaj sliku.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string python = GetPythonInterpreter() ?? "";
            if (string.IsNullOrWhiteSpace(python))
            {
                MessageBox.Show("Nije pronađen Python interpreter.", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string detectDir = AppPaths.ResolveDirectory("DETECTION");
            string scriptPath = AppPaths.ResolveFile("DETECTION", "detect_yolo.py");
            if (string.IsNullOrWhiteSpace(detectDir) && !string.IsNullOrWhiteSpace(scriptPath))
            {
                detectDir = Path.GetDirectoryName(scriptPath) ?? string.Empty;
            }
            string outputTxt = Path.Combine(Path.GetTempPath(), "yolo_detections.txt");

            if (!File.Exists(scriptPath))
            {
                MessageBox.Show("detect_yolo.py nije pronađen u DETECTION folderu.", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double conf = trackConfidence.Value / 100.0;
            string confStr = conf.ToString(CultureInfo.InvariantCulture);

            lblStatus.Text = "Detekcija u tijeku...";
            progressBar.Value = 0;
            txtLog.Clear();
            detections.Clear();
            listDetections.Items.Clear();
            btnRunDetection.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = python,
                    Arguments = $"\"{scriptPath}\" \"{inputImagePath}\" \"{outputTxt}\" {confStr}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = detectDir
                };

                using var proc = new Process { StartInfo = psi };
                proc.OutputDataReceived += Proc_OutputDataReceived;
                proc.ErrorDataReceived += Proc_ErrorDataReceived;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();

                progressBar.Value = 100;

                if (proc.ExitCode != 0)
                {
                    lblStatus.Text = $"Python skripta je vratila kod {proc.ExitCode}.";
                    MessageBox.Show("Detekcija nije uspjela (provjeri log).", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (File.Exists(outputTxt))
                {
                    ParseDetections(outputTxt);
                    DrawDetections();
                    FillDetectionsList();
                    lblStatus.Text = $"Gotovo. Nađeno: {detections.Count} objekata.";
                }
                else
                {
                    lblStatus.Text = "Nema izlaznog fajla (provjeri Python skriptu).";
                    MessageBox.Show("Skripta nije generirala izlazni TXT.", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greška pri detekciji.";
                MessageBox.Show("Dogodila se greška:\n" + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRunDetection.Enabled = true;
            }
        }

        // ---------------------- PROCESS OUTPUT --------------------

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data)) return;
            AppendLog(e.Data.Trim());
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data)) return;
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

        // ---------------------- PARSE & DRAW ----------------------

        private void ParseDetections(string txtPath)
        {
            detections.Clear();

            foreach (var line in File.ReadAllLines(txtPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                if (parts.Length != 6) continue;

                try
                {
                    string label = parts[0];
                    float score = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    float x1 = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    float y1 = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    float x2 = float.Parse(parts[4], CultureInfo.InvariantCulture);
                    float y2 = float.Parse(parts[5], CultureInfo.InvariantCulture);

                    var rect = new RectangleF(x1, y1, x2 - x1, y2 - y1);
                    detections.Add(new Detection
                    {
                        Label = label,
                        Confidence = score,
                        Box = rect
                    });
                }
                catch
                {
                    // ignore malformed lines
                }
            }
        }

        private void DrawDetections()
        {
            if (originalImage == null)
                return;

            Bitmap annotated = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(annotated))
            {
                g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);

                using Pen pen = new Pen(Color.Lime, 2);
                using Brush textBg = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
                using Brush textBrush = new SolidBrush(Color.Lime);
                using Font font = new Font("Segoe UI", 9, FontStyle.Bold);

                foreach (var det in detections)
                {
                    g.DrawRectangle(pen, det.Box.X, det.Box.Y, det.Box.Width, det.Box.Height);

                    string caption = $"{det.Label} {det.Confidence:0.00}";
                    var size = g.MeasureString(caption, font);
                    var bgRect = new RectangleF(det.Box.X, det.Box.Y - size.Height, size.Width, size.Height);
                    g.FillRectangle(textBg, bgRect);
                    g.DrawString(caption, font, textBrush, det.Box.X, det.Box.Y - size.Height);
                }
            }

            picDetection.Image?.Dispose();
            picDetection.Image = annotated;
        }

        private void FillDetectionsList()
        {
            listDetections.Items.Clear();
            foreach (var d in detections)
            {
                listDetections.Items.Add($"{d.Label} ({d.Confidence:0.00})");
            }
        }
    }
}
