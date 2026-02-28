using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form8
    {
        private System.ComponentModel.IContainer components = null!;

        private Panel panelTop;
        private Panel panelCenter;
        private Panel panelBottom;
        private Button btnLoad;
        private Button btnRun;
        private Button btnSave;
        private ComboBox cmbStyle;
        private Label lblInputTitle;
        private Label lblOutputTitle;
        private PictureBox picInput;
        private PictureBox picOutput;
        private Label lblBlend;
        private Label lblBlendValue;
        private TrackBar trackBlend;
        private Label lblDenoise;
        private Label lblDenoiseValue;
        private TrackBar trackDenoise;
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
            cmbStyle = new ComboBox();
            lblBlend = new Label();
            trackBlend = new TrackBar();
            lblBlendValue = new Label();
            lblDenoise = new Label();
            trackDenoise = new TrackBar();
            lblDenoiseValue = new Label();
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
            ((System.ComponentModel.ISupportInitialize)trackBlend).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackDenoise).BeginInit();
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
            panelTop.Controls.Add(cmbStyle);
            panelTop.Controls.Add(lblBlend);
            panelTop.Controls.Add(trackBlend);
            panelTop.Controls.Add(lblBlendValue);
            panelTop.Controls.Add(lblDenoise);
            panelTop.Controls.Add(trackDenoise);
            panelTop.Controls.Add(lblDenoiseValue);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1264, 90);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(12, 26);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(274, 30);
            lblTitle.TabIndex = 11;
            lblTitle.Text = "Cartoonify (AnimeGANv2)";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(324, 10);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(110, 30);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Učitaj sliku";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(444, 10);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(120, 30);
            btnRun.TabIndex = 1;
            btnRun.Text = "Transformiraj";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(574, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 30);
            btnSave.TabIndex = 2;
            btnSave.Text = "Spremi output";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // cmbStyle
            // 
            cmbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStyle.FormattingEnabled = true;
            cmbStyle.Items.AddRange(new object[] { "v1", "v2", "paprika", "celeba" });
            cmbStyle.Location = new Point(712, 13);
            cmbStyle.Name = "cmbStyle";
            cmbStyle.Size = new Size(140, 23);
            cmbStyle.TabIndex = 3;
            // 
            // lblBlend
            // 
            lblBlend.AutoSize = true;
            lblBlend.Location = new Point(324, 51);
            lblBlend.Name = "lblBlend";
            lblBlend.Size = new Size(64, 15);
            lblBlend.TabIndex = 4;
            lblBlend.Text = "Blend ratio";
            // 
            // trackBlend
            // 
            trackBlend.Location = new Point(402, 45);
            trackBlend.Maximum = 100;
            trackBlend.Name = "trackBlend";
            trackBlend.Size = new Size(220, 45);
            trackBlend.TabIndex = 5;
            trackBlend.TickFrequency = 10;
            trackBlend.Value = 100;
            trackBlend.Scroll += trackBlend_Scroll;
            // 
            // lblBlendValue
            // 
            lblBlendValue.AutoSize = true;
            lblBlendValue.Location = new Point(632, 51);
            lblBlendValue.Name = "lblBlendValue";
            lblBlendValue.Size = new Size(35, 15);
            lblBlendValue.TabIndex = 6;
            lblBlendValue.Text = "100%";
            // 
            // lblDenoise
            // 
            lblDenoise.AutoSize = true;
            lblDenoise.Location = new Point(672, 51);
            lblDenoise.Name = "lblDenoise";
            lblDenoise.Size = new Size(94, 15);
            lblDenoise.TabIndex = 7;
            lblDenoise.Text = "Uklanjanje šuma";
            // 
            // trackDenoise
            // 
            trackDenoise.Location = new Point(772, 45);
            trackDenoise.Maximum = 100;
            trackDenoise.Name = "trackDenoise";
            trackDenoise.Size = new Size(210, 45);
            trackDenoise.TabIndex = 8;
            trackDenoise.TickFrequency = 10;
            trackDenoise.Scroll += trackDenoise_Scroll;
            // 
            // lblDenoiseValue
            // 
            lblDenoiseValue.AutoSize = true;
            lblDenoiseValue.Location = new Point(992, 51);
            lblDenoiseValue.Name = "lblDenoiseValue";
            lblDenoiseValue.Size = new Size(23, 15);
            lblDenoiseValue.TabIndex = 9;
            lblDenoiseValue.Text = "0%";
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(centerLayout);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 90);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1264, 531);
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
            centerLayout.Name = "centerLayout";
            centerLayout.RowCount = 2;
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            centerLayout.Size = new Size(1264, 531);
            centerLayout.TabIndex = 0;
            // 
            // lblInputTitle
            // 
            lblInputTitle.Anchor = AnchorStyles.Left;
            lblInputTitle.AutoSize = true;
            lblInputTitle.Location = new Point(10, 7);
            lblInputTitle.Margin = new Padding(10, 0, 0, 0);
            lblInputTitle.Name = "lblInputTitle";
            lblInputTitle.Size = new Size(74, 15);
            lblInputTitle.TabIndex = 0;
            lblInputTitle.Text = "INPUT SLIKA";
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.Anchor = AnchorStyles.Left;
            lblOutputTitle.AutoSize = true;
            lblOutputTitle.Location = new Point(642, 7);
            lblOutputTitle.Margin = new Padding(10, 0, 0, 0);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(86, 15);
            lblOutputTitle.TabIndex = 1;
            lblOutputTitle.Text = "OUTPUT SLIKA";
            // 
            // picInput
            // 
            picInput.BackColor = Color.Black;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(10, 40);
            picInput.Margin = new Padding(10);
            picInput.Name = "picInput";
            picInput.Size = new Size(612, 481);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 2;
            picInput.TabStop = false;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(642, 40);
            picOutput.Margin = new Padding(10);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(612, 481);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 3;
            picOutput.TabStop = false;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(bottomLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 621);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1264, 120);
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
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomLayout.Size = new Size(1264, 120);
            bottomLayout.TabIndex = 0;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(6, 3);
            progressBar.Margin = new Padding(6, 3, 6, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1252, 6);
            progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(10, 17);
            lblStatus.Margin = new Padding(10, 0, 0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(64, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status: Idle";
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.ForeColor = Color.White;
            txtLog.Location = new Point(6, 32);
            txtLog.Margin = new Padding(6, 0, 6, 6);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1252, 82);
            txtLog.TabIndex = 2;
            // 
            // Form8
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 741);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form8";
            Text = "Cartoonify (AnimeGAN v2)";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBlend).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackDenoise).EndInit();
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
