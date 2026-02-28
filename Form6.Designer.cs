using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class Form6
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelTop;
        private Panel panelBottom;
        private Panel panelCenter;

        private Button btnLoadInput;
        private Button btnReloadStyles;
        private Button btnAddStyle;
        private Button btnPrimijeniStil;
        private Button btnSaveOutput;
        private Button btnClearOutput;

        private FlowLayoutPanel flowStyles;

        private PictureBox picInput;
        private PictureBox picStyle;
        private PictureBox picOutput;

        private TrackBar trackStyleStrength;
        private Label lblStyleStrength;
        private Label lblStyleStrengthValue;
        private Label lblStatus;

        private ProgressBar progressBar;
        private TextBox txtLog;

        private Label lblInputTitle;
        private Label lblStyleTitle;
        private Label lblOutputTitle;

        private TableLayoutPanel bottomLayout;
        private TableLayoutPanel table;

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
            btnLoadInput = new Button();
            btnReloadStyles = new Button();
            btnAddStyle = new Button();
            btnPrimijeniStil = new Button();
            btnSaveOutput = new Button();
            btnClearOutput = new Button();
            lblStyleStrength = new Label();
            trackStyleStrength = new TrackBar();
            lblStyleStrengthValue = new Label();
            panelBottom = new Panel();
            bottomLayout = new TableLayoutPanel();
            progressBar = new ProgressBar();
            txtLog = new TextBox();
            lblStatus = new Label();
            panelCenter = new Panel();
            table = new TableLayoutPanel();
            lblInputTitle = new Label();
            lblStyleTitle = new Label();
            lblOutputTitle = new Label();
            picInput = new PictureBox();
            picStyle = new PictureBox();
            picOutput = new PictureBox();
            flowStyles = new FlowLayoutPanel();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackStyleStrength).BeginInit();
            panelBottom.SuspendLayout();
            bottomLayout.SuspendLayout();
            panelCenter.SuspendLayout();
            table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStyle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(40, 40, 40);
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnLoadInput);
            panelTop.Controls.Add(btnReloadStyles);
            panelTop.Controls.Add(btnAddStyle);
            panelTop.Controls.Add(btnPrimijeniStil);
            panelTop.Controls.Add(btnSaveOutput);
            panelTop.Controls.Add(btnClearOutput);
            panelTop.Controls.Add(lblStyleStrength);
            panelTop.Controls.Add(trackStyleStrength);
            panelTop.Controls.Add(lblStyleStrengthValue);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1623, 70);
            panelTop.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(9, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(283, 37);
            lblTitle.TabIndex = 9;
            lblTitle.Text = "Style Transfer (AdaIN)";
            lblTitle.Click += lblTitle_Click;
            // 
            // btnLoadInput
            // 
            btnLoadInput.BackColor = Color.FromArgb(70, 70, 120);
            btnLoadInput.FlatAppearance.BorderSize = 0;
            btnLoadInput.FlatStyle = FlatStyle.Flat;
            btnLoadInput.ForeColor = Color.White;
            btnLoadInput.Location = new Point(304, 18);
            btnLoadInput.Name = "btnLoadInput";
            btnLoadInput.Size = new Size(150, 35);
            btnLoadInput.TabIndex = 0;
            btnLoadInput.Text = "Učitaj sliku";
            btnLoadInput.UseVisualStyleBackColor = false;
            btnLoadInput.Click += btnLoadInput_Click;
            // 
            // btnReloadStyles
            // 
            btnReloadStyles.BackColor = Color.FromArgb(70, 120, 70);
            btnReloadStyles.FlatAppearance.BorderSize = 0;
            btnReloadStyles.FlatStyle = FlatStyle.Flat;
            btnReloadStyles.ForeColor = Color.White;
            btnReloadStyles.Location = new Point(460, 18);
            btnReloadStyles.Name = "btnReloadStyles";
            btnReloadStyles.Size = new Size(150, 35);
            btnReloadStyles.TabIndex = 1;
            btnReloadStyles.Text = "Osvježi stilove";
            btnReloadStyles.UseVisualStyleBackColor = false;
            btnReloadStyles.Click += btnReloadStyles_Click;
            // 
            // btnAddStyle
            // 
            btnAddStyle.BackColor = Color.FromArgb(90, 110, 140);
            btnAddStyle.FlatAppearance.BorderSize = 0;
            btnAddStyle.FlatStyle = FlatStyle.Flat;
            btnAddStyle.ForeColor = Color.White;
            btnAddStyle.Location = new Point(616, 18);
            btnAddStyle.Name = "btnAddStyle";
            btnAddStyle.Size = new Size(150, 35);
            btnAddStyle.TabIndex = 2;
            btnAddStyle.Text = "Dodaj stil";
            btnAddStyle.UseVisualStyleBackColor = false;
            btnAddStyle.Click += btnAddStyle_Click;
            // 
            // btnPrimijeniStil
            // 
            btnPrimijeniStil.BackColor = Color.FromArgb(120, 80, 40);
            btnPrimijeniStil.FlatAppearance.BorderSize = 0;
            btnPrimijeniStil.FlatStyle = FlatStyle.Flat;
            btnPrimijeniStil.ForeColor = Color.White;
            btnPrimijeniStil.Location = new Point(772, 18);
            btnPrimijeniStil.Name = "btnPrimijeniStil";
            btnPrimijeniStil.Size = new Size(170, 35);
            btnPrimijeniStil.TabIndex = 3;
            btnPrimijeniStil.Text = "Primijeni stil (AdaIN)";
            btnPrimijeniStil.UseVisualStyleBackColor = false;
            btnPrimijeniStil.Click += btnPrimijeniStil_Click;
            // 
            // btnSaveOutput
            // 
            btnSaveOutput.BackColor = Color.FromArgb(80, 80, 80);
            btnSaveOutput.FlatAppearance.BorderSize = 0;
            btnSaveOutput.FlatStyle = FlatStyle.Flat;
            btnSaveOutput.ForeColor = Color.White;
            btnSaveOutput.Location = new Point(948, 18);
            btnSaveOutput.Name = "btnSaveOutput";
            btnSaveOutput.Size = new Size(120, 35);
            btnSaveOutput.TabIndex = 4;
            btnSaveOutput.Text = "Spremi";
            btnSaveOutput.UseVisualStyleBackColor = false;
            btnSaveOutput.Click += btnSaveOutput_Click;
            // 
            // btnClearOutput
            // 
            btnClearOutput.BackColor = Color.FromArgb(90, 60, 110);
            btnClearOutput.FlatAppearance.BorderSize = 0;
            btnClearOutput.FlatStyle = FlatStyle.Flat;
            btnClearOutput.ForeColor = Color.White;
            btnClearOutput.Location = new Point(1074, 18);
            btnClearOutput.Name = "btnClearOutput";
            btnClearOutput.Size = new Size(120, 35);
            btnClearOutput.TabIndex = 5;
            btnClearOutput.Text = "Očisti output";
            btnClearOutput.UseVisualStyleBackColor = false;
            btnClearOutput.Click += btnClearOutput_Click;
            // 
            // lblStyleStrength
            // 
            lblStyleStrength.AutoSize = true;
            lblStyleStrength.ForeColor = Color.White;
            lblStyleStrength.Location = new Point(1260, 25);
            lblStyleStrength.Name = "lblStyleStrength";
            lblStyleStrength.Size = new Size(83, 20);
            lblStyleStrength.TabIndex = 6;
            lblStyleStrength.Text = "Jačina stila:";
            // 
            // trackStyleStrength
            // 
            trackStyleStrength.Location = new Point(1349, 14);
            trackStyleStrength.Maximum = 100;
            trackStyleStrength.Name = "trackStyleStrength";
            trackStyleStrength.Size = new Size(220, 56);
            trackStyleStrength.TabIndex = 7;
            trackStyleStrength.TickFrequency = 10;
            trackStyleStrength.Value = 80;
            trackStyleStrength.Scroll += trackStyleStrength_Scroll;
            // 
            // lblStyleStrengthValue
            // 
            lblStyleStrengthValue.AutoSize = true;
            lblStyleStrengthValue.ForeColor = Color.White;
            lblStyleStrengthValue.Location = new Point(1575, 25);
            lblStyleStrengthValue.Name = "lblStyleStrengthValue";
            lblStyleStrengthValue.Size = new Size(36, 20);
            lblStyleStrengthValue.TabIndex = 8;
            lblStyleStrengthValue.Text = "0.80";
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(40, 40, 40);
            panelBottom.Controls.Add(bottomLayout);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 640);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1623, 180);
            panelBottom.TabIndex = 1;
            // 
            // bottomLayout
            // 
            bottomLayout.ColumnCount = 1;
            bottomLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            bottomLayout.Controls.Add(progressBar, 0, 0);
            bottomLayout.Controls.Add(txtLog, 0, 1);
            bottomLayout.Controls.Add(lblStatus, 0, 2);
            bottomLayout.Dock = DockStyle.Fill;
            bottomLayout.Location = new Point(0, 0);
            bottomLayout.Name = "bottomLayout";
            bottomLayout.RowCount = 3;
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            bottomLayout.Size = new Size(1623, 180);
            bottomLayout.TabIndex = 0;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(3, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1617, 19);
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
            txtLog.Size = new Size(1617, 124);
            txtLog.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(3, 155);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new Size(1617, 25);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Spremno.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelCenter
            // 
            panelCenter.BackColor = Color.FromArgb(35, 35, 35);
            panelCenter.Controls.Add(table);
            panelCenter.Controls.Add(flowStyles);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 70);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(1623, 570);
            panelCenter.TabIndex = 0;
            // 
            // table
            // 
            table.ColumnCount = 3;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            table.Controls.Add(lblInputTitle, 0, 0);
            table.Controls.Add(lblStyleTitle, 1, 0);
            table.Controls.Add(lblOutputTitle, 2, 0);
            table.Controls.Add(picInput, 0, 1);
            table.Controls.Add(picStyle, 1, 1);
            table.Controls.Add(picOutput, 2, 1);
            table.Dock = DockStyle.Fill;
            table.Location = new Point(292, 0);
            table.Name = "table";
            table.RowCount = 2;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            table.Size = new Size(1331, 570);
            table.TabIndex = 0;
            // 
            // lblInputTitle
            // 
            lblInputTitle.Dock = DockStyle.Fill;
            lblInputTitle.ForeColor = Color.White;
            lblInputTitle.Location = new Point(3, 0);
            lblInputTitle.Name = "lblInputTitle";
            lblInputTitle.Size = new Size(433, 28);
            lblInputTitle.TabIndex = 0;
            lblInputTitle.Text = "INPUT SLIKA";
            lblInputTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStyleTitle
            // 
            lblStyleTitle.Dock = DockStyle.Fill;
            lblStyleTitle.ForeColor = Color.White;
            lblStyleTitle.Location = new Point(442, 0);
            lblStyleTitle.Name = "lblStyleTitle";
            lblStyleTitle.Size = new Size(433, 28);
            lblStyleTitle.TabIndex = 1;
            lblStyleTitle.Text = "STYLE SLIKA";
            lblStyleTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.Dock = DockStyle.Fill;
            lblOutputTitle.ForeColor = Color.White;
            lblOutputTitle.Location = new Point(881, 0);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(447, 28);
            lblOutputTitle.TabIndex = 2;
            lblOutputTitle.Text = "OUTPUT SLIKA";
            lblOutputTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picInput
            // 
            picInput.BackColor = Color.Black;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(3, 31);
            picInput.Name = "picInput";
            picInput.Size = new Size(433, 536);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 3;
            picInput.TabStop = false;
            // 
            // picStyle
            // 
            picStyle.BackColor = Color.Black;
            picStyle.Dock = DockStyle.Fill;
            picStyle.Location = new Point(442, 31);
            picStyle.Name = "picStyle";
            picStyle.Size = new Size(433, 536);
            picStyle.SizeMode = PictureBoxSizeMode.Zoom;
            picStyle.TabIndex = 4;
            picStyle.TabStop = false;
            // 
            // picOutput
            // 
            picOutput.BackColor = Color.Black;
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(881, 31);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(447, 536);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 5;
            picOutput.TabStop = false;
            // 
            // flowStyles
            // 
            flowStyles.AutoScroll = true;
            flowStyles.BackColor = Color.FromArgb(25, 25, 25);
            flowStyles.Dock = DockStyle.Left;
            flowStyles.Location = new Point(0, 0);
            flowStyles.Name = "flowStyles";
            flowStyles.Padding = new Padding(10);
            flowStyles.Size = new Size(292, 570);
            flowStyles.TabIndex = 1;
            // 
            // Form6
            // 
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1623, 820);
            Controls.Add(panelCenter);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI Style Transfer (AdaIN)";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackStyleStrength).EndInit();
            panelBottom.ResumeLayout(false);
            bottomLayout.ResumeLayout(false);
            bottomLayout.PerformLayout();
            panelCenter.ResumeLayout(false);
            table.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStyle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            ResumeLayout(false);
        }
        private Label lblTitle;
    }
}
