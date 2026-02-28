using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBoxInput;
        private PictureBox pictureBoxOutput;
        private ComboBox comboBoxTransform;
        private Button btnLoadImage;
        private Button btnTransform;
        private Button btnSaveAs;
        private ProgressBar progressBar;
        private TrackBar trackParam;
        private Label lblParam;
        private Label lblParamValue;
        private Button btnReset;
        private Button btnUseOutput;
        private Panel panelControls;
        private Panel panelTop;
        private Panel panelImages;
        private Panel panelCenter;
        private TableLayoutPanel tableImages;
        private Label form1Title;
        private Label lblInputHeader;
        private Label lblOutputHeader;

        private void InitializeComponent()
        {
            pictureBoxInput = new PictureBox();
            pictureBoxOutput = new PictureBox();
            comboBoxTransform = new ComboBox();
            btnLoadImage = new Button();
            btnTransform = new Button();
            btnSaveAs = new Button();
            progressBar = new ProgressBar();
            trackParam = new TrackBar();
            lblParam = new Label();
            lblParamValue = new Label();
            btnReset = new Button();
            btnUseOutput = new Button();
            panelControls = new Panel();
            panelImages = new Panel();
            tableImages = new TableLayoutPanel();
            lblInputHeader = new Label();
            lblOutputHeader = new Label();
            panelTop = new Panel();
            form1Title = new Label();
            panelCenter = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackParam).BeginInit();
            panelControls.SuspendLayout();
            panelImages.SuspendLayout();
            tableImages.SuspendLayout();
            panelTop.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxInput
            // 
            pictureBoxInput.BackColor = Color.Black;
            pictureBoxInput.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxInput.Dock = DockStyle.Fill;
            pictureBoxInput.Location = new Point(3, 24);
            pictureBoxInput.Margin = new Padding(3, 2, 3, 2);
            pictureBoxInput.Name = "pictureBoxInput";
            pictureBoxInput.Size = new Size(491, 346);
            pictureBoxInput.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxInput.TabIndex = 0;
            pictureBoxInput.TabStop = false;
            // 
            // pictureBoxOutput
            // 
            pictureBoxOutput.BackColor = Color.Black;
            pictureBoxOutput.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxOutput.Dock = DockStyle.Fill;
            pictureBoxOutput.Location = new Point(500, 24);
            pictureBoxOutput.Margin = new Padding(3, 2, 3, 2);
            pictureBoxOutput.Name = "pictureBoxOutput";
            pictureBoxOutput.Size = new Size(492, 346);
            pictureBoxOutput.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOutput.TabIndex = 1;
            pictureBoxOutput.TabStop = false;
            // 
            // comboBoxTransform
            // 
            comboBoxTransform.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTransform.Location = new Point(184, 15);
            comboBoxTransform.Margin = new Padding(3, 2, 3, 2);
            comboBoxTransform.Name = "comboBoxTransform";
            comboBoxTransform.Size = new Size(184, 23);
            comboBoxTransform.TabIndex = 2;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(9, 12);
            btnLoadImage.Margin = new Padding(3, 2, 3, 2);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(158, 28);
            btnLoadImage.TabIndex = 3;
            btnLoadImage.Text = "Učitaj sliku";
            // 
            // btnTransform
            // 
            btnTransform.Location = new Point(9, 52);
            btnTransform.Margin = new Padding(3, 2, 3, 2);
            btnTransform.Name = "btnTransform";
            btnTransform.Size = new Size(158, 28);
            btnTransform.TabIndex = 4;
            btnTransform.Text = "Transformiraj";
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(184, 52);
            btnSaveAs.Margin = new Padding(3, 2, 3, 2);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(140, 28);
            btnSaveAs.TabIndex = 5;
            btnSaveAs.Text = "Spremi kao";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(9, 105);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(752, 9);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 6;
            progressBar.Visible = false;
            // 
            // trackParam
            // 
            trackParam.Location = new Point(374, 15);
            trackParam.Margin = new Padding(3, 2, 3, 2);
            trackParam.Maximum = 100;
            trackParam.Minimum = 1;
            trackParam.Name = "trackParam";
            trackParam.Size = new Size(315, 45);
            trackParam.TabIndex = 10;
            trackParam.Value = 50;
            // 
            // lblParam
            // 
            lblParam.AutoSize = true;
            lblParam.Location = new Point(374, 1);
            lblParam.Name = "lblParam";
            lblParam.Size = new Size(90, 15);
            lblParam.TabIndex = 11;
            lblParam.Text = "Parametar (n/a)";
            // 
            // lblParamValue
            // 
            lblParamValue.AutoSize = true;
            lblParamValue.Location = new Point(682, 24);
            lblParamValue.Name = "lblParamValue";
            lblParamValue.Size = new Size(12, 15);
            lblParamValue.TabIndex = 12;
            lblParamValue.Text = "-";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(332, 52);
            btnReset.Margin = new Padding(3, 2, 3, 2);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(131, 28);
            btnReset.TabIndex = 12;
            btnReset.Text = "Očisti output";
            // 
            // btnUseOutput
            // 
            btnUseOutput.Location = new Point(481, 52);
            btnUseOutput.Margin = new Padding(3, 2, 3, 2);
            btnUseOutput.Name = "btnUseOutput";
            btnUseOutput.Size = new Size(158, 28);
            btnUseOutput.TabIndex = 13;
            btnUseOutput.Text = "Koristi output kao input";
            // 
            // panelControls
            // 
            panelControls.BackColor = SystemColors.ControlLightLight;
            panelControls.BorderStyle = BorderStyle.FixedSingle;
            panelControls.Controls.Add(btnUseOutput);
            panelControls.Controls.Add(btnReset);
            panelControls.Controls.Add(lblParamValue);
            panelControls.Controls.Add(lblParam);
            panelControls.Controls.Add(trackParam);
            panelControls.Controls.Add(progressBar);
            panelControls.Controls.Add(btnSaveAs);
            panelControls.Controls.Add(btnTransform);
            panelControls.Controls.Add(btnLoadImage);
            panelControls.Controls.Add(comboBoxTransform);
            panelControls.Dock = DockStyle.Bottom;
            panelControls.Location = new Point(0, 430);
            panelControls.Margin = new Padding(3, 2, 3, 2);
            panelControls.Name = "panelControls";
            panelControls.Padding = new Padding(9, 8, 9, 8);
            panelControls.Size = new Size(1015, 118);
            panelControls.TabIndex = 15;
            // 
            // panelImages
            // 
            panelImages.BackColor = Color.FromArgb(35, 35, 35);
            panelImages.BorderStyle = BorderStyle.FixedSingle;
            panelImages.Controls.Add(tableImages);
            panelImages.Dock = DockStyle.Fill;
            panelImages.Location = new Point(0, 40);
            panelImages.Margin = new Padding(3, 2, 3, 2);
            panelImages.Name = "panelImages";
            panelImages.Padding = new Padding(9, 8, 9, 8);
            panelImages.Size = new Size(1015, 390);
            panelImages.TabIndex = 16;
            // 
            // tableImages
            // 
            tableImages.ColumnCount = 2;
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableImages.Controls.Add(pictureBoxInput, 0, 1);
            tableImages.Controls.Add(pictureBoxOutput, 1, 1);
            tableImages.Controls.Add(lblInputHeader, 0, 0);
            tableImages.Controls.Add(lblOutputHeader, 1, 0);
            tableImages.Dock = DockStyle.Fill;
            tableImages.Location = new Point(9, 8);
            tableImages.Margin = new Padding(3, 2, 3, 2);
            tableImages.Name = "tableImages";
            tableImages.RowCount = 2;
            tableImages.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableImages.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableImages.Size = new Size(995, 372);
            tableImages.TabIndex = 16;
            // 
            // lblInputHeader
            // 
            lblInputHeader.Dock = DockStyle.Fill;
            lblInputHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblInputHeader.Location = new Point(3, 0);
            lblInputHeader.Name = "lblInputHeader";
            lblInputHeader.Size = new Size(491, 22);
            lblInputHeader.TabIndex = 17;
            lblInputHeader.Text = "INPUT SLIKA";
            lblInputHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOutputHeader
            // 
            lblOutputHeader.Dock = DockStyle.Fill;
            lblOutputHeader.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblOutputHeader.Location = new Point(500, 0);
            lblOutputHeader.Name = "lblOutputHeader";
            lblOutputHeader.Size = new Size(492, 22);
            lblOutputHeader.TabIndex = 18;
            lblOutputHeader.Text = "OUTPUT SLIKA";
            lblOutputHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(40, 40, 40);
            panelTop.Controls.Add(form1Title);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(3, 2, 3, 2);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1015, 40);
            panelTop.TabIndex = 17;
            // 
            // form1Title
            // 
            form1Title.AutoSize = true;
            form1Title.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            form1Title.ForeColor = Color.White;
            form1Title.Location = new Point(10, 6);
            form1Title.Name = "form1Title";
            form1Title.Size = new Size(272, 32);
            form1Title.TabIndex = 9;
            form1Title.Text = "Osnovne transformacije";
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(panelImages);
            panelCenter.Controls.Add(panelControls);
            panelCenter.Controls.Add(panelTop);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 0);
            panelCenter.Margin = new Padding(3, 2, 3, 2);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1015, 548);
            panelCenter.TabIndex = 17;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1015, 548);
            Controls.Add(panelCenter);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Image Transformer";
            ((System.ComponentModel.ISupportInitialize)pictureBoxInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutput).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackParam).EndInit();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            panelImages.ResumeLayout(false);
            tableImages.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
