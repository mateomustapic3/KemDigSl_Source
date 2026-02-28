using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form7
    {
        private System.ComponentModel.IContainer components = null!;

        private Panel panelTop;
        private Panel panelBottom;
        private Panel panelCenter;

        private Button btnLoadImage;
        private Button btnRunDetection;

        private TrackBar trackConfidence;
        private Label lblConfTitle;
        private Label lblConfValue;

        private PictureBox picDetection;
        private ListBox listDetections;

        private ProgressBar progressBar;
        private TextBox txtLog;
        private Label lblStatus;

        private Label lblImageTitle;
        private Label lblDetectionsTitle;

        private TableLayoutPanel bottomLayout;
        private TableLayoutPanel centerLayout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblTitle = new Label();
            btnLoadImage = new Button();
            btnRunDetection = new Button();
            lblConfTitle = new Label();
            trackConfidence = new TrackBar();
            lblConfValue = new Label();
            panelBottom = new Panel();
            bottomLayout = new TableLayoutPanel();
            progressBar = new ProgressBar();
            txtLog = new TextBox();
            lblStatus = new Label();
            panelCenter = new Panel();
            centerLayout = new TableLayoutPanel();
            lblImageTitle = new Label();
            lblDetectionsTitle = new Label();
            picDetection = new PictureBox();
            listDetections = new ListBox();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackConfidence).BeginInit();
            panelBottom.SuspendLayout();
            bottomLayout.SuspendLayout();
            panelCenter.SuspendLayout();
            centerLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picDetection).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(40, 40, 40);
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnLoadImage);
            panelTop.Controls.Add(btnRunDetection);
            panelTop.Controls.Add(lblConfTitle);
            panelTop.Controls.Add(trackConfidence);
            panelTop.Controls.Add(lblConfValue);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10);
            panelTop.Size = new Size(1549, 70);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(26, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(310, 30);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "Detekcija objekata (YOLOv8n)";
            // 
            // btnLoadImage
            // 
            btnLoadImage.BackColor = Color.FromArgb(70, 70, 120);
            btnLoadImage.FlatAppearance.BorderSize = 0;
            btnLoadImage.FlatStyle = FlatStyle.Flat;
            btnLoadImage.ForeColor = Color.White;
            btnLoadImage.Location = new Point(372, 21);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(150, 35);
            btnLoadImage.TabIndex = 0;
            btnLoadImage.Text = "Učitaj sliku";
            btnLoadImage.UseVisualStyleBackColor = false;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // btnRunDetection
            // 
            btnRunDetection.BackColor = Color.FromArgb(120, 80, 40);
            btnRunDetection.FlatAppearance.BorderSize = 0;
            btnRunDetection.FlatStyle = FlatStyle.Flat;
            btnRunDetection.ForeColor = Color.White;
            btnRunDetection.Location = new Point(537, 21);
            btnRunDetection.Name = "btnRunDetection";
            btnRunDetection.Size = new Size(170, 35);
            btnRunDetection.TabIndex = 1;
            btnRunDetection.Text = "Detektiraj objekte";
            btnRunDetection.UseVisualStyleBackColor = false;
            btnRunDetection.Click += btnRunDetection_Click;
            // 
            // lblConfTitle
            // 
            lblConfTitle.AutoSize = true;
            lblConfTitle.ForeColor = Color.White;
            lblConfTitle.Location = new Point(737, 29);
            lblConfTitle.Name = "lblConfTitle";
            lblConfTitle.Size = new Size(92, 15);
            lblConfTitle.TabIndex = 2;
            lblConfTitle.Text = "Min. povjerenje:";
            // 
            // trackConfidence
            // 
            trackConfidence.Location = new Point(835, 25);
            trackConfidence.Maximum = 90;
            trackConfidence.Minimum = 10;
            trackConfidence.Name = "trackConfidence";
            trackConfidence.Size = new Size(220, 45);
            trackConfidence.TabIndex = 3;
            trackConfidence.TickFrequency = 10;
            trackConfidence.Value = 50;
            trackConfidence.Scroll += trackConfidence_Scroll;
            // 
            // lblConfValue
            // 
            lblConfValue.AutoSize = true;
            lblConfValue.ForeColor = Color.White;
            lblConfValue.Location = new Point(1061, 29);
            lblConfValue.Name = "lblConfValue";
            lblConfValue.Size = new Size(28, 15);
            lblConfValue.TabIndex = 4;
            lblConfValue.Text = "0.50";
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(40, 40, 40);
            panelBottom.Controls.Add(bottomLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 620);
            panelBottom.Margin = new Padding(0);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(10);
            panelBottom.Size = new Size(1549, 180);
            panelBottom.TabIndex = 2;
            // 
            // bottomLayout
            // 
            bottomLayout.ColumnCount = 1;
            bottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bottomLayout.Controls.Add(progressBar, 0, 0);
            bottomLayout.Controls.Add(txtLog, 0, 1);
            bottomLayout.Controls.Add(lblStatus, 0, 2);
            bottomLayout.Dock = DockStyle.Fill;
            bottomLayout.Location = new Point(10, 10);
            bottomLayout.Name = "bottomLayout";
            bottomLayout.RowCount = 3;
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            bottomLayout.Size = new Size(1529, 160);
            bottomLayout.TabIndex = 0;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(3, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1523, 19);
            progressBar.TabIndex = 0;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(25, 25, 25);
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Dock = DockStyle.Fill;
            txtLog.ForeColor = Color.White;
            txtLog.Location = new Point(3, 28);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1523, 104);
            txtLog.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(3, 135);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new Size(1523, 25);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Spremno.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelCenter
            // 
            panelCenter.BackColor = Color.FromArgb(35, 35, 35);
            panelCenter.Controls.Add(centerLayout);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 70);
            panelCenter.Margin = new Padding(0);
            panelCenter.Name = "panelCenter";
            panelCenter.Padding = new Padding(10);
            panelCenter.Size = new Size(1549, 550);
            panelCenter.TabIndex = 1;
            // 
            // centerLayout
            // 
            centerLayout.ColumnCount = 2;
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            centerLayout.Controls.Add(lblImageTitle, 0, 0);
            centerLayout.Controls.Add(lblDetectionsTitle, 1, 0);
            centerLayout.Controls.Add(picDetection, 0, 1);
            centerLayout.Controls.Add(listDetections, 1, 1);
            centerLayout.Dock = DockStyle.Fill;
            centerLayout.Location = new Point(10, 10);
            centerLayout.Name = "centerLayout";
            centerLayout.RowCount = 2;
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            centerLayout.Size = new Size(1529, 530);
            centerLayout.TabIndex = 0;
            // 
            // lblImageTitle
            // 
            lblImageTitle.Dock = DockStyle.Fill;
            lblImageTitle.ForeColor = Color.White;
            lblImageTitle.Location = new Point(3, 0);
            lblImageTitle.Name = "lblImageTitle";
            lblImageTitle.Size = new Size(1064, 28);
            lblImageTitle.TabIndex = 0;
            lblImageTitle.Text = "SLIKA + BOXES";
            lblImageTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDetectionsTitle
            // 
            lblDetectionsTitle.Dock = DockStyle.Fill;
            lblDetectionsTitle.ForeColor = Color.White;
            lblDetectionsTitle.Location = new Point(1073, 0);
            lblDetectionsTitle.Name = "lblDetectionsTitle";
            lblDetectionsTitle.Size = new Size(453, 28);
            lblDetectionsTitle.TabIndex = 1;
            lblDetectionsTitle.Text = "DETEKCIJE";
            lblDetectionsTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picDetection
            // 
            picDetection.BackColor = Color.Black;
            picDetection.Dock = DockStyle.Fill;
            picDetection.Location = new Point(3, 31);
            picDetection.Name = "picDetection";
            picDetection.Size = new Size(1064, 496);
            picDetection.SizeMode = PictureBoxSizeMode.Zoom;
            picDetection.TabIndex = 2;
            picDetection.TabStop = false;
            // 
            // listDetections
            // 
            listDetections.BackColor = Color.FromArgb(25, 25, 25);
            listDetections.BorderStyle = BorderStyle.FixedSingle;
            listDetections.Dock = DockStyle.Fill;
            listDetections.ForeColor = Color.White;
            listDetections.FormattingEnabled = true;
            listDetections.ItemHeight = 15;
            listDetections.Location = new Point(1073, 31);
            listDetections.Name = "listDetections";
            listDetections.Size = new Size(453, 496);
            listDetections.TabIndex = 3;
            // 
            // Form7
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1549, 800);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form7";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI Object Detection (YOLOv8n)";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackConfidence).EndInit();
            panelBottom.ResumeLayout(false);
            bottomLayout.ResumeLayout(false);
            bottomLayout.PerformLayout();
            panelCenter.ResumeLayout(false);
            centerLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picDetection).EndInit();
            ResumeLayout(false);
        }
        private Label lblTitle;
    }
}
