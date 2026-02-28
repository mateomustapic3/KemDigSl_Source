using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form9
    {
        private System.ComponentModel.IContainer components = null!;

        private Panel panelTop;
        private Panel panelCenter;
        private Panel panelBottom;
        private Button btnLoad;
        private Button btnRun;
        private Button btnSave;
        private Label lblInputTitle;
        private Label lblOutputTitle;
        private PictureBox picInput;
        private PictureBox picOutput;
        private Label lblDenoise;
        private Label lblDenoiseValue;
        private TrackBar trackDenoise;
        private Label lblScratch;
        private Label lblScratchValue;
        private TrackBar trackScratch;
        private TextBox txtLog;
        private ProgressBar progressBar;
        private Label lblStatus;
        private TableLayoutPanel centerLayout;
        private TableLayoutPanel bottomLayout;

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
            btnLoad = new Button();
            btnRun = new Button();
            btnSave = new Button();
            lblDenoise = new Label();
            trackDenoise = new TrackBar();
            lblDenoiseValue = new Label();
            lblScratch = new Label();
            trackScratch = new TrackBar();
            lblScratchValue = new Label();
            panelCenter = new Panel();
            centerLayout = new TableLayoutPanel();
            lblInputTitle = new Label();
            lblOutputTitle = new Label();
            picInput = new PictureBox();
            picOutput = new PictureBox();
            panelBottom = new Panel();
            bottomLayout = new TableLayoutPanel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            txtLog = new TextBox();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackDenoise).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackScratch).BeginInit();
            panelCenter.SuspendLayout();
            centerLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).BeginInit();
            panelBottom.SuspendLayout();
            bottomLayout.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnLoad);
            panelTop.Controls.Add(btnRun);
            panelTop.Controls.Add(btnSave);
            panelTop.Controls.Add(lblDenoise);
            panelTop.Controls.Add(trackDenoise);
            panelTop.Controls.Add(lblDenoiseValue);
            panelTop.Controls.Add(lblScratch);
            panelTop.Controls.Add(trackScratch);
            panelTop.Controls.Add(lblScratchValue);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1445, 153);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(14, 45);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(366, 37);
            lblTitle.TabIndex = 12;
            lblTitle.Text = "Film grain && scratch removal";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(402, 25);
            btnLoad.Margin = new Padding(3, 4, 3, 4);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(137, 40);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Učitaj sliku";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(561, 25);
            btnRun.Margin = new Padding(3, 4, 3, 4);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(137, 40);
            btnRun.TabIndex = 1;
            btnRun.Text = "Transformiraj";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(721, 25);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(149, 40);
            btnSave.TabIndex = 2;
            btnSave.Text = "Spremi output";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // lblDenoise
            // 
            lblDenoise.AutoSize = true;
            lblDenoise.Location = new Point(371, 88);
            lblDenoise.Name = "lblDenoise";
            lblDenoise.Size = new Size(144, 20);
            lblDenoise.TabIndex = 3;
            lblDenoise.Text = "Grain denoise (0-60)";
            // 
            // trackDenoise
            // 
            trackDenoise.Location = new Point(518, 79);
            trackDenoise.Margin = new Padding(3, 4, 3, 4);
            trackDenoise.Maximum = 60;
            trackDenoise.Name = "trackDenoise";
            trackDenoise.Size = new Size(263, 56);
            trackDenoise.TabIndex = 4;
            trackDenoise.TickFrequency = 5;
            trackDenoise.Value = 35;
            trackDenoise.Scroll += trackDenoise_Scroll;
            // 
            // lblDenoiseValue
            // 
            lblDenoiseValue.AutoSize = true;
            lblDenoiseValue.Location = new Point(777, 80);
            lblDenoiseValue.Name = "lblDenoiseValue";
            lblDenoiseValue.Size = new Size(25, 20);
            lblDenoiseValue.TabIndex = 5;
            lblDenoiseValue.Text = "35";
            // 
            // lblScratch
            // 
            lblScratch.AutoSize = true;
            lblScratch.Location = new Point(823, 80);
            lblScratch.Name = "lblScratch";
            lblScratch.Size = new Size(151, 20);
            lblScratch.TabIndex = 6;
            lblScratch.Text = "Scratch sensitivity (%)";
            // 
            // trackScratch
            // 
            trackScratch.Location = new Point(969, 79);
            trackScratch.Margin = new Padding(3, 4, 3, 4);
            trackScratch.Maximum = 100;
            trackScratch.Name = "trackScratch";
            trackScratch.Size = new Size(263, 56);
            trackScratch.TabIndex = 7;
            trackScratch.TickFrequency = 10;
            trackScratch.Value = 55;
            trackScratch.Scroll += trackScratch_Scroll;
            // 
            // lblScratchValue
            // 
            lblScratchValue.AutoSize = true;
            lblScratchValue.Location = new Point(1239, 80);
            lblScratchValue.Name = "lblScratchValue";
            lblScratchValue.Size = new Size(37, 20);
            lblScratchValue.TabIndex = 8;
            lblScratchValue.Text = "55%";
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(centerLayout);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 153);
            panelCenter.Margin = new Padding(3, 4, 3, 4);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1445, 675);
            panelCenter.TabIndex = 1;
            // 
            // centerLayout
            // 
            centerLayout.ColumnCount = 2;
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            centerLayout.Controls.Add(lblInputTitle, 0, 0);
            centerLayout.Controls.Add(lblOutputTitle, 1, 0);
            centerLayout.Controls.Add(picInput, 0, 1);
            centerLayout.Controls.Add(picOutput, 1, 1);
            centerLayout.Dock = DockStyle.Fill;
            centerLayout.Location = new Point(0, 0);
            centerLayout.Margin = new Padding(3, 4, 3, 4);
            centerLayout.Name = "centerLayout";
            centerLayout.RowCount = 2;
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            centerLayout.Size = new Size(1445, 675);
            centerLayout.TabIndex = 0;
            // 
            // lblInputTitle
            // 
            lblInputTitle.Anchor = AnchorStyles.Left;
            lblInputTitle.AutoSize = true;
            lblInputTitle.Location = new Point(11, 10);
            lblInputTitle.Margin = new Padding(11, 0, 0, 0);
            lblInputTitle.Name = "lblInputTitle";
            lblInputTitle.Size = new Size(92, 20);
            lblInputTitle.TabIndex = 0;
            lblInputTitle.Text = "INPUT SLIKA";
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.Anchor = AnchorStyles.Left;
            lblOutputTitle.AutoSize = true;
            lblOutputTitle.Location = new Point(733, 10);
            lblOutputTitle.Margin = new Padding(11, 0, 0, 0);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(106, 20);
            lblOutputTitle.TabIndex = 1;
            lblOutputTitle.Text = "OUTPUT SLIKA";
            // 
            // picInput
            // 
            picInput.BackColor = Color.Black;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(11, 53);
            picInput.Margin = new Padding(11, 13, 11, 13);
            picInput.Name = "picInput";
            picInput.Size = new Size(700, 609);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 2;
            picInput.TabStop = false;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(733, 53);
            picOutput.Margin = new Padding(11, 13, 11, 13);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(701, 609);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 3;
            picOutput.TabStop = false;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(bottomLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 828);
            panelBottom.Margin = new Padding(3, 4, 3, 4);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1445, 160);
            panelBottom.TabIndex = 2;
            // 
            // bottomLayout
            // 
            bottomLayout.ColumnCount = 1;
            bottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bottomLayout.Controls.Add(progressBar, 0, 0);
            bottomLayout.Controls.Add(lblStatus, 0, 1);
            bottomLayout.Controls.Add(txtLog, 0, 2);
            bottomLayout.Dock = DockStyle.Fill;
            bottomLayout.Location = new Point(0, 0);
            bottomLayout.Margin = new Padding(0);
            bottomLayout.Name = "bottomLayout";
            bottomLayout.RowCount = 3;
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomLayout.Size = new Size(1445, 160);
            bottomLayout.TabIndex = 0;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(7, 4);
            progressBar.Margin = new Padding(7, 4, 7, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1431, 8);
            progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(11, 23);
            lblStatus.Margin = new Padding(11, 0, 0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(113, 20);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status: spreman";
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.ForeColor = Color.White;
            txtLog.Location = new Point(7, 43);
            txtLog.Margin = new Padding(7, 0, 7, 8);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1431, 109);
            txtLog.TabIndex = 2;
            // 
            // Form9
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1445, 988);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form9";
            Text = "Film grain & scratch remover";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackDenoise).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackScratch).EndInit();
            panelCenter.ResumeLayout(false);
            centerLayout.ResumeLayout(false);
            centerLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            panelBottom.ResumeLayout(false);
            bottomLayout.ResumeLayout(false);
            bottomLayout.PerformLayout();
            ResumeLayout(false);
        }
        private Label lblTitle;
    }
}
