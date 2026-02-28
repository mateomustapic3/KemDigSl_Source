using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form5
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelTop;
        private Panel panelImages;
        private Panel panelControls;
        private Panel panelStatus;
        private Panel panelCenter;
        private TableLayoutPanel tableImages;
        private PictureBox picInput;
        private PictureBox picOutput;
        private Label lblInputPreview;
        private Label lblOutputPreview;
        private Label lblTitle;

        private TextBox txtInput;
        private Button btnBrowseInput;
        private TextBox txtOutput;
        private Button btnBrowseOutput;
        private Button btnRun;
        private Button btnUseOutput;
        private Button btnSaveAs;
        private Label lblStrength;
        private Label lblStrengthValue;
        private TrackBar trackStrength;

        private ProgressBar progressBar;
        private Label lblStatus;

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
            panelTop = new Panel();
            lblTitle = new Label();
            panelImages = new Panel();
            tableImages = new TableLayoutPanel();
            lblInputPreview = new Label();
            lblOutputPreview = new Label();
            picInput = new PictureBox();
            picOutput = new PictureBox();
            panelControls = new Panel();
            lblStrengthValue = new Label();
            lblStrength = new Label();
            trackStrength = new TrackBar();
            btnSaveAs = new Button();
            btnUseOutput = new Button();
            btnRun = new Button();
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
            ((System.ComponentModel.ISupportInitialize)trackStrength).BeginInit();
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
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10, 8, 10, 8);
            panelTop.Size = new Size(1180, 44);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(10, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(232, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Kolorizacija (DDColor)";
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
            panelImages.Padding = new Padding(10, 6, 10, 10);
            panelImages.Size = new Size(1180, 476);
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
            tableImages.Location = new Point(10, 6);
            tableImages.Name = "tableImages";
            tableImages.RowCount = 2;
            tableImages.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableImages.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableImages.Size = new Size(1158, 458);
            tableImages.TabIndex = 4;
            // 
            // lblInputPreview
            // 
            lblInputPreview.AutoSize = true;
            lblInputPreview.ForeColor = Color.White;
            lblInputPreview.Location = new Point(3, 0);
            lblInputPreview.Name = "lblInputPreview";
            lblInputPreview.Size = new Size(80, 15);
            lblInputPreview.TabIndex = 0;
            lblInputPreview.Text = "INPUT IMAGE";
            // 
            // lblOutputPreview
            // 
            lblOutputPreview.AutoSize = true;
            lblOutputPreview.ForeColor = Color.White;
            lblOutputPreview.Location = new Point(582, 0);
            lblOutputPreview.Name = "lblOutputPreview";
            lblOutputPreview.Size = new Size(92, 15);
            lblOutputPreview.TabIndex = 1;
            lblOutputPreview.Text = "OUTPUT IMAGE";
            // 
            // picInput
            // 
            picInput.BackColor = Color.Black;
            picInput.BorderStyle = BorderStyle.FixedSingle;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(3, 33);
            picInput.Name = "picInput";
            picInput.Size = new Size(573, 422);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 2;
            picInput.TabStop = false;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.BorderStyle = BorderStyle.FixedSingle;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(582, 33);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(573, 422);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 3;
            picOutput.TabStop = false;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.FromArgb(40, 40, 40);
            panelControls.Controls.Add(lblStrengthValue);
            panelControls.Controls.Add(lblStrength);
            panelControls.Controls.Add(trackStrength);
            panelControls.Controls.Add(btnSaveAs);
            panelControls.Controls.Add(btnUseOutput);
            panelControls.Controls.Add(btnRun);
            panelControls.Controls.Add(btnBrowseInput);
            panelControls.Dock = DockStyle.Bottom;
            panelControls.Location = new Point(0, 476);
            panelControls.Name = "panelControls";
            panelControls.Padding = new Padding(10);
            panelControls.Size = new Size(1180, 140);
            panelControls.TabIndex = 2;
            // 
            // lblStrengthValue
            // 
            lblStrengthValue.AutoSize = true;
            lblStrengthValue.ForeColor = Color.White;
            lblStrengthValue.Location = new Point(658, 17);
            lblStrengthValue.Name = "lblStrengthValue";
            lblStrengthValue.Size = new Size(22, 15);
            lblStrengthValue.TabIndex = 8;
            lblStrengthValue.Text = "1.0";
            // 
            // lblStrength
            // 
            lblStrength.AutoSize = true;
            lblStrength.ForeColor = Color.White;
            lblStrength.Location = new Point(187, 17);
            lblStrength.Name = "lblStrength";
            lblStrength.Size = new Size(39, 15);
            lblStrength.TabIndex = 7;
            lblStrength.Text = "Jačina";
            // 
            // trackStrength
            // 
            trackStrength.Location = new Point(232, 10);
            trackStrength.Maximum = 200;
            trackStrength.Name = "trackStrength";
            trackStrength.Size = new Size(420, 45);
            trackStrength.TabIndex = 9;
            trackStrength.Value = 100;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(330, 54);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(130, 34);
            btnSaveAs.TabIndex = 6;
            btnSaveAs.Text = "Spremi kao";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // btnUseOutput
            // 
            btnUseOutput.Location = new Point(160, 54);
            btnUseOutput.Name = "btnUseOutput";
            btnUseOutput.Size = new Size(160, 34);
            btnUseOutput.TabIndex = 5;
            btnUseOutput.Text = "Koristi output kao input";
            btnUseOutput.UseVisualStyleBackColor = true;
            btnUseOutput.Click += btnUseOutput_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(10, 54);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(140, 34);
            btnRun.TabIndex = 4;
            btnRun.Text = "Transformiraj";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnBrowseInput
            // 
            btnBrowseInput.Location = new Point(10, 10);
            btnBrowseInput.Name = "btnBrowseInput";
            btnBrowseInput.Size = new Size(130, 28);
            btnBrowseInput.TabIndex = 1;
            btnBrowseInput.Text = "Učitaj sliku";
            btnBrowseInput.UseVisualStyleBackColor = true;
            btnBrowseInput.Click += btnBrowseInput_Click;
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Location = new Point(601, 12);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(120, 28);
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
            txtOutput.Location = new Point(743, 14);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(389, 23);
            txtOutput.TabIndex = 2;
            txtOutput.Visible = false;
            // 
            // txtInput
            // 
            txtInput.BackColor = Color.FromArgb(25, 25, 25);
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(150, 12);
            txtInput.Name = "txtInput";
            txtInput.ReadOnly = true;
            txtInput.Size = new Size(400, 23);
            txtInput.TabIndex = 0;
            txtInput.Visible = false;
            // 
            // panelStatus
            // 
            panelStatus.BackColor = Color.FromArgb(40, 40, 40);
            panelStatus.Controls.Add(lblStatus);
            panelStatus.Controls.Add(progressBar);
            panelStatus.Dock = DockStyle.Bottom;
            panelStatus.Location = new Point(0, 660);
            panelStatus.Name = "panelStatus";
            panelStatus.Padding = new Padding(10, 6, 10, 6);
            panelStatus.Size = new Size(1180, 60);
            panelStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(10, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1160, 36);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status: Idle";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(10, 6);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1160, 12);
            progressBar.TabIndex = 0;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(panelImages);
            panelCenter.Controls.Add(panelControls);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 44);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1180, 616);
            panelCenter.TabIndex = 4;
            // 
            // Form5
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1180, 720);
            Controls.Add(panelCenter);
            Controls.Add(panelStatus);
            Controls.Add(panelTop);
            Name = "Form5";
            Text = "DDColor";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelImages.ResumeLayout(false);
            tableImages.ResumeLayout(false);
            tableImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackStrength).EndInit();
            panelStatus.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
