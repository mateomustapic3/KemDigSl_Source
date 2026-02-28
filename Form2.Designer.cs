namespace Project
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.PictureBox pbResult;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnEnhance;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnUseOutput;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbScale;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton radioUpscale;
        private System.Windows.Forms.RadioButton radioDownscale;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.TableLayoutPanel tableImages;
        private System.Windows.Forms.Label lblOriginalTitle;
        private System.Windows.Forms.Label lblResultTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pbOriginal = new PictureBox();
            pbResult = new PictureBox();
            btnSelectImage = new Button();
            btnEnhance = new Button();
            btnSaveAs = new Button();
            btnUseOutput = new Button();
            lblStatus = new Label();
            cbScale = new ComboBox();
            progressBar1 = new ProgressBar();
            radioUpscale = new RadioButton();
            radioDownscale = new RadioButton();
            lblTitle = new Label();
            panelTop = new Panel();
            panelCenter = new Panel();
            tableImages = new TableLayoutPanel();
            lblOriginalTitle = new Label();
            lblResultTitle = new Label();
            panelBottom = new Panel();
            ((System.ComponentModel.ISupportInitialize)pbOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbResult).BeginInit();
            panelTop.SuspendLayout();
            panelCenter.SuspendLayout();
            tableImages.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pbOriginal
            // 
            pbOriginal.BorderStyle = BorderStyle.FixedSingle;
            pbOriginal.Dock = DockStyle.Fill;
            pbOriginal.Location = new Point(10, 30);
            pbOriginal.Margin = new Padding(10, 0, 10, 10);
            pbOriginal.Name = "pbOriginal";
            pbOriginal.Size = new Size(560, 564);
            pbOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pbOriginal.TabIndex = 0;
            pbOriginal.TabStop = false;
            // 
            // pbResult
            // 
            pbResult.BorderStyle = BorderStyle.FixedSingle;
            pbResult.Dock = DockStyle.Fill;
            pbResult.Location = new Point(590, 30);
            pbResult.Margin = new Padding(10, 0, 10, 10);
            pbResult.Name = "pbResult";
            pbResult.Size = new Size(560, 564);
            pbResult.SizeMode = PictureBoxSizeMode.Zoom;
            pbResult.TabIndex = 1;
            pbResult.TabStop = false;
            // 
            // btnSelectImage
            // 
            btnSelectImage.Location = new Point(13, 44);
            btnSelectImage.Name = "btnSelectImage";
            btnSelectImage.Size = new Size(140, 36);
            btnSelectImage.TabIndex = 2;
            btnSelectImage.Text = "Učitaj sliku";
            btnSelectImage.UseVisualStyleBackColor = true;
            btnSelectImage.Click += btnSelectImage_Click;
            // 
            // btnEnhance
            // 
            btnEnhance.Location = new Point(178, 44);
            btnEnhance.Name = "btnEnhance";
            btnEnhance.Size = new Size(120, 36);
            btnEnhance.TabIndex = 4;
            btnEnhance.Text = "Transformiraj";
            btnEnhance.UseVisualStyleBackColor = true;
            btnEnhance.Click += btnEnhance_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(324, 44);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(120, 36);
            btnSaveAs.TabIndex = 5;
            btnSaveAs.Text = "Spremi kao";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // btnUseOutput
            // 
            btnUseOutput.Location = new Point(472, 44);
            btnUseOutput.Name = "btnUseOutput";
            btnUseOutput.Size = new Size(160, 36);
            btnUseOutput.TabIndex = 6;
            btnUseOutput.Text = "Koristi output kao input";
            btnUseOutput.UseVisualStyleBackColor = true;
            btnUseOutput.Click += btnUseOutput_Click;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Location = new Point(10, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1160, 26);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Status: Idle";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cbScale
            // 
            cbScale.DropDownStyle = ComboBoxStyle.DropDownList;
            cbScale.Font = new Font("Segoe UI", 10F);
            cbScale.Location = new Point(862, 55);
            cbScale.Name = "cbScale";
            cbScale.Size = new Size(100, 25);
            cbScale.TabIndex = 5;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Top;
            progressBar1.Location = new Point(10, 10);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1160, 14);
            progressBar1.TabIndex = 7;
            // 
            // radioUpscale
            // 
            radioUpscale.AutoSize = true;
            radioUpscale.Font = new Font("Segoe UI", 10F);
            radioUpscale.Location = new Point(675, 55);
            radioUpscale.Name = "radioUpscale";
            radioUpscale.Size = new Size(74, 23);
            radioUpscale.TabIndex = 3;
            radioUpscale.TabStop = true;
            radioUpscale.Text = "Upscale";
            radioUpscale.UseVisualStyleBackColor = true;
            radioUpscale.CheckedChanged += radioUpscale_CheckedChanged;
            // 
            // radioDownscale
            // 
            radioDownscale.AutoSize = true;
            radioDownscale.Font = new Font("Segoe UI", 10F);
            radioDownscale.Location = new Point(755, 55);
            radioDownscale.Name = "radioDownscale";
            radioDownscale.Size = new Size(92, 23);
            radioDownscale.TabIndex = 4;
            radioDownscale.TabStop = true;
            radioDownscale.Text = "Downscale";
            radioDownscale.UseVisualStyleBackColor = true;
            radioDownscale.CheckedChanged += radioDownscale_CheckedChanged;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(310, 28);
            lblTitle.TabIndex = 8;
            lblTitle.Text = "Upscale/Downscale (ESRGAN)";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(40, 40, 40);
            panelTop.Controls.Add(radioDownscale);
            panelTop.Controls.Add(radioUpscale);
            panelTop.Controls.Add(btnUseOutput);
            panelTop.Controls.Add(btnSaveAs);
            panelTop.Controls.Add(btnEnhance);
            panelTop.Controls.Add(cbScale);
            panelTop.Controls.Add(btnSelectImage);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10);
            panelTop.Size = new Size(1180, 80);
            panelTop.TabIndex = 8;
            // 
            // panelCenter
            // 
            panelCenter.BackColor = Color.FromArgb(35, 35, 35);
            panelCenter.Controls.Add(tableImages);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 80);
            panelCenter.Margin = new Padding(0);
            panelCenter.Name = "panelCenter";
            panelCenter.Padding = new Padding(10);
            panelCenter.Size = new Size(1180, 624);
            panelCenter.TabIndex = 11;
            // 
            // tableImages
            // 
            tableImages.ColumnCount = 2;
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.Controls.Add(lblOriginalTitle, 0, 0);
            tableImages.Controls.Add(lblResultTitle, 1, 0);
            tableImages.Controls.Add(pbOriginal, 0, 1);
            tableImages.Controls.Add(pbResult, 1, 1);
            tableImages.Dock = DockStyle.Fill;
            tableImages.Location = new Point(10, 10);
            tableImages.Name = "tableImages";
            tableImages.RowCount = 2;
            tableImages.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableImages.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableImages.Size = new Size(1160, 604);
            tableImages.TabIndex = 10;
            // 
            // lblOriginalTitle
            // 
            lblOriginalTitle.Dock = DockStyle.Fill;
            lblOriginalTitle.ForeColor = Color.White;
            lblOriginalTitle.Location = new Point(3, 0);
            lblOriginalTitle.Name = "lblOriginalTitle";
            lblOriginalTitle.Size = new Size(574, 30);
            lblOriginalTitle.TabIndex = 2;
            lblOriginalTitle.Text = "INPUT SLIKA";
            lblOriginalTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblResultTitle
            // 
            lblResultTitle.Dock = DockStyle.Fill;
            lblResultTitle.ForeColor = Color.White;
            lblResultTitle.Location = new Point(583, 0);
            lblResultTitle.Name = "lblResultTitle";
            lblResultTitle.Size = new Size(574, 30);
            lblResultTitle.TabIndex = 3;
            lblResultTitle.Text = "OUTPUT SLIKA";
            lblResultTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(40, 40, 40);
            panelBottom.Controls.Add(lblStatus);
            panelBottom.Controls.Add(progressBar1);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 704);
            panelBottom.Margin = new Padding(0);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(10);
            panelBottom.Size = new Size(1180, 60);
            panelBottom.TabIndex = 9;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1180, 764);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Margin = new Padding(0);
            Name = "Form2";
            Text = "Resolution Enhancer (ESRGAN)";
            ((System.ComponentModel.ISupportInitialize)pbOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbResult).EndInit();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelCenter.ResumeLayout(false);
            tableImages.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
