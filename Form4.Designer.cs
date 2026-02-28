using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form4
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelTop;
        private Panel panelImages;
        private TableLayoutPanel tableImages;
        private Panel panelControls;
        private Panel panelStatus;
        private Panel panelCenter;

        private PictureBox pbInput;
        private PictureBox pbOutput;
        private Label lblInput;
        private Label lblOutput;
        private Label lblTitle;

        private TextBox txtInput;
        private Button btnSelect;
        private Button btnRestore;
        private Button btnSaveAs;
        private Button btnUseOutput;
        private ComboBox comboDetectionModel;
        private NumericUpDown numUpscale;
        private TrackBar trackCFWeight;
        private Label lblCFValue;
        private Label lblWeight;
        private Label lblDetectionModel;
        private Label lblUpscale;
        private CheckBox chkFaceUpsample;
        private CheckBox chkBgUpsample;
        private CheckBox chkOnlyCenterFace;
        private CheckBox chkDrawBox;
        private GroupBox grpSettings;

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
            lblInput = new Label();
            lblOutput = new Label();
            pbInput = new PictureBox();
            pbOutput = new PictureBox();
            panelControls = new Panel();
            grpSettings = new GroupBox();
            chkDrawBox = new CheckBox();
            chkOnlyCenterFace = new CheckBox();
            chkBgUpsample = new CheckBox();
            chkFaceUpsample = new CheckBox();
            lblUpscale = new Label();
            lblDetectionModel = new Label();
            lblWeight = new Label();
            lblCFValue = new Label();
            trackCFWeight = new TrackBar();
            numUpscale = new NumericUpDown();
            comboDetectionModel = new ComboBox();
            btnUseOutput = new Button();
            btnSaveAs = new Button();
            btnRestore = new Button();
            btnSelect = new Button();
            txtInput = new TextBox();
            panelStatus = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            panelCenter = new Panel();
            panelTop.SuspendLayout();
            panelImages.SuspendLayout();
            tableImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbOutput).BeginInit();
            panelControls.SuspendLayout();
            grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackCFWeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUpscale).BeginInit();
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
            panelTop.Size = new Size(1334, 59);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(11, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(439, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Ukloni šum/blur lica (CodeFormer)";
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
            panelImages.Size = new Size(1334, 583);
            panelImages.TabIndex = 1;
            // 
            // tableImages
            // 
            tableImages.ColumnCount = 2;
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.Controls.Add(lblInput, 0, 0);
            tableImages.Controls.Add(lblOutput, 1, 0);
            tableImages.Controls.Add(pbInput, 0, 1);
            tableImages.Controls.Add(pbOutput, 1, 1);
            tableImages.Dock = DockStyle.Fill;
            tableImages.Location = new Point(11, 8);
            tableImages.Margin = new Padding(3, 4, 3, 4);
            tableImages.Name = "tableImages";
            tableImages.RowCount = 2;
            tableImages.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableImages.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableImages.Size = new Size(1310, 560);
            tableImages.TabIndex = 4;
            // 
            // lblInput
            // 
            lblInput.AutoSize = true;
            lblInput.ForeColor = Color.White;
            lblInput.Location = new Point(3, 0);
            lblInput.Name = "lblInput";
            lblInput.Size = new Size(92, 20);
            lblInput.TabIndex = 0;
            lblInput.Text = "INPUT SLIKA";
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.ForeColor = Color.White;
            lblOutput.Location = new Point(658, 0);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(106, 20);
            lblOutput.TabIndex = 1;
            lblOutput.Text = "OUTPUT SLIKA";
            // 
            // pbInput
            // 
            pbInput.BackColor = Color.Black;
            pbInput.BorderStyle = BorderStyle.FixedSingle;
            pbInput.Dock = DockStyle.Fill;
            pbInput.Location = new Point(3, 44);
            pbInput.Margin = new Padding(3, 4, 3, 4);
            pbInput.Name = "pbInput";
            pbInput.Size = new Size(649, 512);
            pbInput.SizeMode = PictureBoxSizeMode.Zoom;
            pbInput.TabIndex = 2;
            pbInput.TabStop = false;
            // 
            // pbOutput
            // 
            pbOutput.BackColor = Color.Black;
            pbOutput.BorderStyle = BorderStyle.FixedSingle;
            pbOutput.Dock = DockStyle.Fill;
            pbOutput.Location = new Point(658, 44);
            pbOutput.Margin = new Padding(3, 4, 3, 4);
            pbOutput.Name = "pbOutput";
            pbOutput.Size = new Size(649, 512);
            pbOutput.SizeMode = PictureBoxSizeMode.Zoom;
            pbOutput.TabIndex = 3;
            pbOutput.TabStop = false;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.FromArgb(40, 40, 40);
            panelControls.Controls.Add(grpSettings);
            panelControls.Controls.Add(btnUseOutput);
            panelControls.Controls.Add(btnSaveAs);
            panelControls.Controls.Add(btnRestore);
            panelControls.Controls.Add(btnSelect);
            panelControls.Controls.Add(txtInput);
            panelControls.Dock = DockStyle.Bottom;
            panelControls.Location = new Point(0, 583);
            panelControls.Margin = new Padding(3, 4, 3, 4);
            panelControls.Name = "panelControls";
            panelControls.Padding = new Padding(11, 13, 11, 13);
            panelControls.Size = new Size(1334, 187);
            panelControls.TabIndex = 2;
            // 
            // grpSettings
            // 
            grpSettings.Controls.Add(chkDrawBox);
            grpSettings.Controls.Add(chkOnlyCenterFace);
            grpSettings.Controls.Add(chkBgUpsample);
            grpSettings.Controls.Add(chkFaceUpsample);
            grpSettings.Controls.Add(lblUpscale);
            grpSettings.Controls.Add(lblDetectionModel);
            grpSettings.Controls.Add(lblWeight);
            grpSettings.Controls.Add(lblCFValue);
            grpSettings.Controls.Add(trackCFWeight);
            grpSettings.Controls.Add(numUpscale);
            grpSettings.Controls.Add(comboDetectionModel);
            grpSettings.ForeColor = Color.White;
            grpSettings.Location = new Point(27, 67);
            grpSettings.Margin = new Padding(3, 4, 3, 4);
            grpSettings.Name = "grpSettings";
            grpSettings.Padding = new Padding(3, 4, 3, 4);
            grpSettings.Size = new Size(857, 112);
            grpSettings.TabIndex = 5;
            grpSettings.TabStop = false;
            grpSettings.Text = "Postavke";
            // 
            // chkDrawBox
            // 
            chkDrawBox.AutoSize = true;
            chkDrawBox.ForeColor = Color.White;
            chkDrawBox.Location = new Point(702, 72);
            chkDrawBox.Margin = new Padding(3, 4, 3, 4);
            chkDrawBox.Name = "chkDrawBox";
            chkDrawBox.Size = new Size(95, 24);
            chkDrawBox.TabIndex = 10;
            chkDrawBox.Text = "Draw box";
            // 
            // chkOnlyCenterFace
            // 
            chkOnlyCenterFace.AutoSize = true;
            chkOnlyCenterFace.ForeColor = Color.White;
            chkOnlyCenterFace.Location = new Point(549, 72);
            chkOnlyCenterFace.Margin = new Padding(3, 4, 3, 4);
            chkOnlyCenterFace.Name = "chkOnlyCenterFace";
            chkOnlyCenterFace.Size = new Size(138, 24);
            chkOnlyCenterFace.TabIndex = 9;
            chkOnlyCenterFace.Text = "Only center face";
            // 
            // chkBgUpsample
            // 
            chkBgUpsample.AutoSize = true;
            chkBgUpsample.ForeColor = Color.White;
            chkBgUpsample.Location = new Point(411, 72);
            chkBgUpsample.Margin = new Padding(3, 4, 3, 4);
            chkBgUpsample.Name = "chkBgUpsample";
            chkBgUpsample.Size = new Size(132, 24);
            chkBgUpsample.TabIndex = 8;
            chkBgUpsample.Text = "BG upsampling";
            // 
            // chkFaceUpsample
            // 
            chkFaceUpsample.AutoSize = true;
            chkFaceUpsample.ForeColor = Color.White;
            chkFaceUpsample.Location = new Point(265, 72);
            chkFaceUpsample.Margin = new Padding(3, 4, 3, 4);
            chkFaceUpsample.Name = "chkFaceUpsample";
            chkFaceUpsample.Size = new Size(129, 24);
            chkFaceUpsample.TabIndex = 7;
            chkFaceUpsample.Text = "Face upsample";
            chkFaceUpsample.CheckedChanged += chkFaceUpsample_CheckedChanged;
            // 
            // lblUpscale
            // 
            lblUpscale.AutoSize = true;
            lblUpscale.ForeColor = Color.White;
            lblUpscale.Location = new Point(32, 72);
            lblUpscale.Name = "lblUpscale";
            lblUpscale.Size = new Size(61, 20);
            lblUpscale.TabIndex = 6;
            lblUpscale.Text = "Upscale";
            // 
            // lblDetectionModel
            // 
            lblDetectionModel.AutoSize = true;
            lblDetectionModel.ForeColor = Color.White;
            lblDetectionModel.Location = new Point(32, 33);
            lblDetectionModel.Name = "lblDetectionModel";
            lblDetectionModel.Size = new Size(121, 20);
            lblDetectionModel.TabIndex = 5;
            lblDetectionModel.Text = "Detection model";
            // 
            // lblWeight
            // 
            lblWeight.AutoSize = true;
            lblWeight.ForeColor = Color.White;
            lblWeight.Location = new Point(400, 33);
            lblWeight.Name = "lblWeight";
            lblWeight.Size = new Size(76, 20);
            lblWeight.TabIndex = 4;
            lblWeight.Text = "Weight CF";
            // 
            // lblCFValue
            // 
            lblCFValue.AutoSize = true;
            lblCFValue.ForeColor = Color.White;
            lblCFValue.Location = new Point(784, 37);
            lblCFValue.Name = "lblCFValue";
            lblCFValue.Size = new Size(28, 20);
            lblCFValue.TabIndex = 3;
            lblCFValue.Text = "1.0";
            // 
            // trackCFWeight
            // 
            trackCFWeight.Location = new Point(521, 29);
            trackCFWeight.Margin = new Padding(3, 4, 3, 4);
            trackCFWeight.Maximum = 200;
            trackCFWeight.Name = "trackCFWeight";
            trackCFWeight.Size = new Size(266, 56);
            trackCFWeight.TabIndex = 2;
            // 
            // numUpscale
            // 
            numUpscale.Location = new Point(174, 69);
            numUpscale.Margin = new Padding(3, 4, 3, 4);
            numUpscale.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numUpscale.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numUpscale.Name = "numUpscale";
            numUpscale.Size = new Size(75, 27);
            numUpscale.TabIndex = 1;
            numUpscale.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // comboDetectionModel
            // 
            comboDetectionModel.DropDownStyle = ComboBoxStyle.DropDownList;
            comboDetectionModel.Location = new Point(174, 29);
            comboDetectionModel.Margin = new Padding(3, 4, 3, 4);
            comboDetectionModel.Name = "comboDetectionModel";
            comboDetectionModel.Size = new Size(212, 28);
            comboDetectionModel.TabIndex = 0;
            // 
            // btnUseOutput
            // 
            btnUseOutput.Location = new Point(960, 16);
            btnUseOutput.Margin = new Padding(3, 4, 3, 4);
            btnUseOutput.Name = "btnUseOutput";
            btnUseOutput.Size = new Size(183, 37);
            btnUseOutput.TabIndex = 4;
            btnUseOutput.Text = "Koristi output kao input";
            btnUseOutput.UseVisualStyleBackColor = true;
            btnUseOutput.Click += btnUseOutput_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(811, 16);
            btnSaveAs.Margin = new Padding(3, 4, 3, 4);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(137, 37);
            btnSaveAs.TabIndex = 3;
            btnSaveAs.Text = "Spremi kao";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(651, 16);
            btnRestore.Margin = new Padding(3, 4, 3, 4);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(149, 37);
            btnRestore.TabIndex = 2;
            btnRestore.Text = "Transformiraj";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(11, 16);
            btnSelect.Margin = new Padding(3, 4, 3, 4);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(149, 37);
            btnSelect.TabIndex = 1;
            btnSelect.Text = "Učitaj sliku";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // txtInput
            // 
            txtInput.BackColor = Color.FromArgb(25, 25, 25);
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(171, 19);
            txtInput.Margin = new Padding(3, 4, 3, 4);
            txtInput.Name = "txtInput";
            txtInput.ReadOnly = true;
            txtInput.Size = new Size(457, 27);
            txtInput.TabIndex = 0;
            // 
            // panelStatus
            // 
            panelStatus.BackColor = Color.FromArgb(40, 40, 40);
            panelStatus.Controls.Add(lblStatus);
            panelStatus.Controls.Add(progressBar);
            panelStatus.Dock = DockStyle.Bottom;
            panelStatus.Location = new Point(0, 829);
            panelStatus.Margin = new Padding(3, 4, 3, 4);
            panelStatus.Name = "panelStatus";
            panelStatus.Padding = new Padding(11, 8, 11, 8);
            panelStatus.Size = new Size(1334, 80);
            panelStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(11, 21);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1312, 51);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status: Idle";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(11, 8);
            progressBar.Margin = new Padding(3, 4, 3, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1312, 13);
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
            panelCenter.Size = new Size(1334, 770);
            panelCenter.TabIndex = 4;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1334, 909);
            Controls.Add(panelCenter);
            Controls.Add(panelStatus);
            Controls.Add(panelTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form4";
            Text = "Ukloni šum/blur lica (CodeFormer)";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelImages.ResumeLayout(false);
            tableImages.ResumeLayout(false);
            tableImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbOutput).EndInit();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            grpSettings.ResumeLayout(false);
            grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackCFWeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUpscale).EndInit();
            panelStatus.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
