using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form10
    {
        private System.ComponentModel.IContainer components = null!;

        private Panel panelTop;
        private Panel panelCenter;
        private Panel panelBottom;
        private Panel panelRight;
        private Button btnLoad;
        private Button btnRun;
        private Button btnSave;
        private Button btnClearMask;
        private PictureBox picImage;
        private Panel panelOutputHost;
        private PictureBox picOutput;
        private TrackBar trackBrush;
        private Label lblBrush;
        private Label lblBrushValue;
        private ProgressBar progressBar;
        private Label lblStatus;
        private TextBox txtLog;
        private Label lblOutputTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new Panel();
            btnLoad = new Button();
            btnRun = new Button();
            btnSave = new Button();
            btnClearMask = new Button();
            panelCenter = new Panel();
            picImage = new PictureBox();
            panelRight = new Panel();
            lblOutputTitle = new Label();
            panelOutputHost = new Panel();
            picOutput = new PictureBox();
            lblBrush = new Label();
            trackBrush = new TrackBar();
            lblBrushValue = new Label();
            panelBottom = new Panel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            txtLog = new TextBox();
            lblTitle = new Label();
            panelTop.SuspendLayout();
            panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            panelRight.SuspendLayout();
            panelOutputHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picOutput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBrush).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnLoad);
            panelTop.Controls.Add(btnRun);
            panelTop.Controls.Add(btnSave);
            panelTop.Controls.Add(btnClearMask);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1264, 60);
            panelTop.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(409, 15);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(110, 30);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Učitaj sliku";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(529, 15);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(120, 30);
            btnRun.TabIndex = 1;
            btnRun.Text = "Ukloni objekt";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(659, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 30);
            btnSave.TabIndex = 2;
            btnSave.Text = "Spremi output";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnClearMask
            // 
            btnClearMask.Location = new Point(789, 15);
            btnClearMask.Name = "btnClearMask";
            btnClearMask.Size = new Size(120, 30);
            btnClearMask.TabIndex = 3;
            btnClearMask.Text = "Obriši masku";
            btnClearMask.UseVisualStyleBackColor = true;
            btnClearMask.Click += btnClearMask_Click;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(picImage);
            panelCenter.Controls.Add(panelRight);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 60);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1264, 581);
            panelCenter.TabIndex = 1;
            // 
            // picImage
            // 
            picImage.BackColor = Color.Black;
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(864, 581);
            picImage.SizeMode = PictureBoxSizeMode.Zoom;
            picImage.TabIndex = 0;
            picImage.TabStop = false;
            picImage.MouseDown += picImage_MouseDown;
            picImage.MouseMove += picImage_MouseMove;
            picImage.MouseUp += picImage_MouseUp;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(lblOutputTitle);
            panelRight.Controls.Add(panelOutputHost);
            panelRight.Controls.Add(lblBrush);
            panelRight.Controls.Add(trackBrush);
            panelRight.Controls.Add(lblBrushValue);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(864, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(400, 581);
            panelRight.TabIndex = 1;
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.AutoSize = true;
            lblOutputTitle.Location = new Point(16, 9);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(103, 15);
            lblOutputTitle.TabIndex = 4;
            lblOutputTitle.Text = "OUTPUT PREVIEW";
            // 
            // panelOutputHost
            // 
            panelOutputHost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelOutputHost.BackColor = Color.Black;
            panelOutputHost.Controls.Add(picOutput);
            panelOutputHost.Location = new Point(16, 27);
            panelOutputHost.Name = "panelOutputHost";
            panelOutputHost.Size = new Size(368, 368);
            panelOutputHost.TabIndex = 5;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(0, 0);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(368, 368);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 3;
            picOutput.TabStop = false;
            picOutput.Click += picOutput_Click;
            // 
            // lblBrush
            // 
            lblBrush.AutoSize = true;
            lblBrush.Location = new Point(16, 413);
            lblBrush.Name = "lblBrush";
            lblBrush.Size = new Size(62, 15);
            lblBrush.TabIndex = 0;
            lblBrush.Text = "Brush size:";
            // 
            // trackBrush
            // 
            trackBrush.Location = new Point(16, 431);
            trackBrush.Maximum = 80;
            trackBrush.Minimum = 4;
            trackBrush.Name = "trackBrush";
            trackBrush.Size = new Size(280, 45);
            trackBrush.TabIndex = 1;
            trackBrush.TickFrequency = 4;
            trackBrush.Value = 18;
            trackBrush.Scroll += trackBrush_Scroll;
            // 
            // lblBrushValue
            // 
            lblBrushValue.AutoSize = true;
            lblBrushValue.Location = new Point(302, 441);
            lblBrushValue.Name = "lblBrushValue";
            lblBrushValue.Size = new Size(34, 15);
            lblBrushValue.TabIndex = 2;
            lblBrushValue.Text = "18 px";
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(progressBar);
            panelBottom.Controls.Add(lblStatus);
            panelBottom.Controls.Add(txtLog);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 641);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1264, 100);
            panelBottom.TabIndex = 2;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(10, 6);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1244, 8);
            progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(10, 20);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.ForeColor = Color.White;
            txtLog.Location = new Point(10, 40);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1244, 52);
            txtLog.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(12, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(361, 30);
            lblTitle.TabIndex = 13;
            lblTitle.Text = "Uklanjanje objekata (mask + LaMa)";
            // 
            // Form10
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 741);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form10";
            Text = "Object removal";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            panelOutputHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBrush).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
        private Label lblTitle;
    }
}
