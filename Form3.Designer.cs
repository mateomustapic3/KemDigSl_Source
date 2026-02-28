using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelTop;
        private Panel panelImages;
        private Panel panelControls;
        private Panel panelStatus;
        private Panel panelCenter;

        private PictureBox picInput;
        private PictureBox picOutput;
        private Label lblInputPreview;
        private Label lblOutputPreview;
        private TableLayoutPanel tableImages;
        private Label lblTitle;

        private TextBox txtInput;
        private Button btnBrowseInput;
        private TextBox txtOutput;
        private Button btnBrowseOutput;
        private ComboBox cboUpscale;
        private CheckBox chkOnlyCenterFace;
        private CheckBox chkPasteBack;
        private Button btnRun;
        private Button btnUseOutput;
        private Button btnSaveAs;

        private ProgressBar progressBar;
        private Label lblStatus;

        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblTitle = new Label();
            panelImages = new Panel();
            tableImages = new TableLayoutPanel();
            lblInputPreview = new Label();
            lblOutputPreview = new Label();
            picInput = new PictureBox();
            picOutput = new PictureBox();
            panelControls = new Panel();
            btnUseOutput = new Button();
            btnRun = new Button();
            btnSaveAs = new Button();
            chkPasteBack = new CheckBox();
            chkOnlyCenterFace = new CheckBox();
            cboUpscale = new ComboBox();
            btnBrowseInput = new Button();
            btnBrowseOutput = new Button();
            txtOutput = new TextBox();
            txtInput = new TextBox();
            panelStatus = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            panelCenter = new Panel();
            panelTop.SuspendLayout();
            panelImages.SuspendLayout();
            tableImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).BeginInit();
            panelControls.SuspendLayout();
            panelStatus.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(40, 40, 40);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(3, 4, 3, 4);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(11, 11, 11, 11);
            panelTop.Size = new Size(1349, 59);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(11, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(293, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Obnova lica (GFPGAN)";
            // 
            // panelImages
            // 
            panelImages.BackColor = Color.FromArgb(35, 35, 35);
            panelImages.BorderStyle = BorderStyle.FixedSingle;
            panelImages.Controls.Add(tableImages);
            panelImages.Dock = DockStyle.Fill;
            panelImages.Location = new Point(0, 0);
            panelImages.Margin = new Padding(0);
            panelImages.Name = "panelImages";
            panelImages.Padding = new Padding(11, 8, 11, 13);
            panelImages.Size = new Size(1349, 634);
            panelImages.TabIndex = 1;
            // 
            // tableImages
            // 
            tableImages.ColumnCount = 2;
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.Controls.Add(lblInputPreview, 0, 0);
            tableImages.Controls.Add(lblOutputPreview, 1, 0);
            tableImages.Controls.Add(picInput, 0, 1);
            tableImages.Controls.Add(picOutput, 1, 1);
            tableImages.Dock = DockStyle.Fill;
            tableImages.Location = new Point(11, 8);
            tableImages.Margin = new Padding(3, 4, 3, 4);
            tableImages.Name = "tableImages";
            tableImages.RowCount = 2;
            tableImages.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableImages.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableImages.Size = new Size(1325, 611);
            tableImages.TabIndex = 4;
            // 
            // lblInputPreview
            // 
            lblInputPreview.AutoSize = true;
            lblInputPreview.ForeColor = Color.White;
            lblInputPreview.Location = new Point(3, 0);
            lblInputPreview.Name = "lblInputPreview";
            lblInputPreview.Size = new Size(99, 20);
            lblInputPreview.TabIndex = 0;
            lblInputPreview.Text = "INPUT IMAGE";
            // 
            // lblOutputPreview
            // 
            lblOutputPreview.AutoSize = true;
            lblOutputPreview.ForeColor = Color.White;
            lblOutputPreview.Location = new Point(665, 0);
            lblOutputPreview.Name = "lblOutputPreview";
            lblOutputPreview.Size = new Size(113, 20);
            lblOutputPreview.TabIndex = 1;
            lblOutputPreview.Text = "OUTPUT IMAGE";
            // 
            // picInput
            // 
            picInput.BackColor = Color.Black;
            picInput.BorderStyle = BorderStyle.FixedSingle;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(3, 44);
            picInput.Margin = new Padding(3, 4, 3, 4);
            picInput.Name = "picInput";
            picInput.Size = new Size(656, 563);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 2;
            picInput.TabStop = false;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.BorderStyle = BorderStyle.FixedSingle;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(665, 44);
            picOutput.Margin = new Padding(3, 4, 3, 4);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(657, 563);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 3;
            picOutput.TabStop = false;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.FromArgb(40, 40, 40);
            panelControls.Controls.Add(btnUseOutput);
            panelControls.Controls.Add(btnRun);
            panelControls.Controls.Add(btnSaveAs);
            panelControls.Controls.Add(chkPasteBack);
            panelControls.Controls.Add(chkOnlyCenterFace);
            panelControls.Controls.Add(cboUpscale);
            panelControls.Controls.Add(btnBrowseInput);
            panelControls.Dock = DockStyle.Bottom;
            panelControls.Location = new Point(0, 634);
            panelControls.Margin = new Padding(3, 4, 3, 4);
            panelControls.Name = "panelControls";
            panelControls.Padding = new Padding(11, 13, 11, 13);
            panelControls.Size = new Size(1349, 160);
            panelControls.TabIndex = 2;
            // 
            // btnUseOutput
            // 
            btnUseOutput.Location = new Point(537, 13);
            btnUseOutput.Margin = new Padding(3, 4, 3, 4);
            btnUseOutput.Name = "btnUseOutput";
            btnUseOutput.Size = new Size(177, 37);
            btnUseOutput.TabIndex = 8;
            btnUseOutput.Text = "Koristi output kao input";
            btnUseOutput.UseVisualStyleBackColor = true;
            btnUseOutput.Click += btnUseOutput_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(360, 13);
            btnRun.Margin = new Padding(3, 4, 3, 4);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(157, 37);
            btnRun.TabIndex = 7;
            btnRun.Text = "Transformiraj";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(183, 13);
            btnSaveAs.Margin = new Padding(3, 4, 3, 4);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(160, 37);
            btnSaveAs.TabIndex = 9;
            btnSaveAs.Text = "Spremi kao...";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // chkPasteBack
            // 
            chkPasteBack.AutoSize = true;
            chkPasteBack.ForeColor = Color.White;
            chkPasteBack.Location = new Point(360, 76);
            chkPasteBack.Margin = new Padding(3, 4, 3, 4);
            chkPasteBack.Name = "chkPasteBack";
            chkPasteBack.Size = new Size(100, 24);
            chkPasteBack.TabIndex = 6;
            chkPasteBack.Text = "Paste back";
            // 
            // chkOnlyCenterFace
            // 
            chkOnlyCenterFace.AutoSize = true;
            chkOnlyCenterFace.ForeColor = Color.White;
            chkOnlyCenterFace.Location = new Point(183, 75);
            chkOnlyCenterFace.Margin = new Padding(3, 4, 3, 4);
            chkOnlyCenterFace.Name = "chkOnlyCenterFace";
            chkOnlyCenterFace.Size = new Size(138, 24);
            chkOnlyCenterFace.TabIndex = 5;
            chkOnlyCenterFace.Text = "Only center face";
            // 
            // cboUpscale
            // 
            cboUpscale.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUpscale.Location = new Point(16, 72);
            cboUpscale.Margin = new Padding(3, 4, 3, 4);
            cboUpscale.Name = "cboUpscale";
            cboUpscale.Size = new Size(137, 28);
            cboUpscale.TabIndex = 4;
            // 
            // btnBrowseInput
            // 
            btnBrowseInput.Location = new Point(11, 13);
            btnBrowseInput.Margin = new Padding(3, 4, 3, 4);
            btnBrowseInput.Name = "btnBrowseInput";
            btnBrowseInput.Size = new Size(160, 37);
            btnBrowseInput.TabIndex = 1;
            btnBrowseInput.Text = "Učitaj sliku";
            btnBrowseInput.UseVisualStyleBackColor = true;
            btnBrowseInput.Click += btnBrowseInput_Click;
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Location = new Point(601, 13);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(130, 28);
            btnBrowseOutput.TabIndex = 3;
            btnBrowseOutput.Text = "Browse Output";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Visible = false;
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            // 
            // txtOutput
            // 
            txtOutput.BackColor = Color.FromArgb(25, 25, 25);
            txtOutput.ForeColor = Color.White;
            txtOutput.Location = new Point(747, 15);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(381, 27);
            txtOutput.TabIndex = 2;
            txtOutput.Visible = false;
            // 
            // txtInput
            // 
            txtInput.BackColor = Color.FromArgb(25, 25, 25);
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(160, 12);
            txtInput.Name = "txtInput";
            txtInput.ReadOnly = true;
            txtInput.Size = new Size(360, 27);
            txtInput.TabIndex = 0;
            txtInput.Visible = false;
            // 
            // panelStatus
            // 
            panelStatus.BackColor = Color.FromArgb(40, 40, 40);
            panelStatus.Controls.Add(lblStatus);
            panelStatus.Controls.Add(progressBar);
            panelStatus.Dock = DockStyle.Bottom;
            panelStatus.Location = new Point(0, 853);
            panelStatus.Margin = new Padding(3, 4, 3, 4);
            panelStatus.Name = "panelStatus";
            panelStatus.Padding = new Padding(11, 8, 11, 8);
            panelStatus.Size = new Size(1349, 80);
            panelStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(11, 24);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1327, 48);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Spreman.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(11, 8);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1327, 16);
            progressBar.TabIndex = 0;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(panelImages);
            panelCenter.Controls.Add(panelControls);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 59);
            panelCenter.Margin = new Padding(3, 4, 3, 4);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1349, 794);
            panelCenter.TabIndex = 4;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1349, 933);
            Controls.Add(panelCenter);
            Controls.Add(panelStatus);
            Controls.Add(panelTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form3";
            Text = "GFPGAN Face Restoration";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelImages.ResumeLayout(false);
            tableImages.ResumeLayout(false);
            tableImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            panelStatus.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
