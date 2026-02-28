namespace Project
{
    partial class Form11
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.FlowLayoutPanel flowTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.CheckBox chkAllowExposure;
        private System.Windows.Forms.CheckBox chkAllowSharpen;
        private System.Windows.Forms.CheckBox chkAllowFaceRestore;
        private System.Windows.Forms.CheckBox chkAllowColorize;
        private System.Windows.Forms.CheckBox chkAllowUpscale;

        private System.Windows.Forms.TableLayoutPanel body;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelOutput;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.Label lblInputTitle;
        private System.Windows.Forms.Label lblOutputTitle;
        private System.Windows.Forms.PictureBox picInput;
        private System.Windows.Forms.PictureBox picOutput;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progress;

        private void InitializeComponent()
        {
            panelTop = new Panel();
            flowTop = new FlowLayoutPanel();
            lblTitle = new Label();
            btnLoad = new Button();
            btnRun = new Button();
            btnSaveAs = new Button();
            chkAllowExposure = new CheckBox();
            chkAllowSharpen = new CheckBox();
            chkAllowFaceRestore = new CheckBox();
            chkAllowColorize = new CheckBox();
            chkAllowUpscale = new CheckBox();
            body = new TableLayoutPanel();
            panelInput = new Panel();
            picInput = new PictureBox();
            lblInputTitle = new Label();
            panelOutput = new Panel();
            picOutput = new PictureBox();
            lblOutputTitle = new Label();
            panelLog = new Panel();
            txtLog = new TextBox();
            panelStatus = new Panel();
            lblStatus = new Label();
            progress = new ProgressBar();
            panelTop.SuspendLayout();
            flowTop.SuspendLayout();
            body.SuspendLayout();
            panelInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picInput).BeginInit();
            panelOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picOutput).BeginInit();
            panelLog.SuspendLayout();
            panelStatus.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(flowTop);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12);
            panelTop.Size = new Size(1100, 66);
            panelTop.TabIndex = 0;
            // 
            // flowTop
            // 
            flowTop.Controls.Add(lblTitle);
            flowTop.Controls.Add(btnLoad);
            flowTop.Controls.Add(btnRun);
            flowTop.Controls.Add(btnSaveAs);
            flowTop.Controls.Add(chkAllowExposure);
            flowTop.Controls.Add(chkAllowSharpen);
            flowTop.Controls.Add(chkAllowFaceRestore);
            flowTop.Controls.Add(chkAllowColorize);
            flowTop.Controls.Add(chkAllowUpscale);
            flowTop.Dock = DockStyle.Fill;
            flowTop.Location = new Point(12, 12);
            flowTop.Name = "flowTop";
            flowTop.Size = new Size(1076, 42);
            flowTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(3, 6);
            lblTitle.Margin = new Padding(3, 6, 18, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(82, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "AutoFix";
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(106, 6);
            btnLoad.Margin = new Padding(3, 6, 3, 3);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(130, 30);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Učitaj sliku";
            btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(242, 6);
            btnRun.Margin = new Padding(3, 6, 3, 3);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(110, 30);
            btnRun.TabIndex = 2;
            btnRun.Text = "AutoFix";
            btnRun.UseVisualStyleBackColor = true;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(358, 6);
            btnSaveAs.Margin = new Padding(3, 6, 18, 3);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(130, 30);
            btnSaveAs.TabIndex = 3;
            btnSaveAs.Text = "Spremi kao";
            btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // chkAllowExposure
            // 
            chkAllowExposure.AutoSize = true;
            chkAllowExposure.Location = new Point(509, 12);
            chkAllowExposure.Margin = new Padding(3, 12, 12, 3);
            chkAllowExposure.Name = "chkAllowExposure";
            chkAllowExposure.Size = new Size(150, 19);
            chkAllowExposure.TabIndex = 4;
            chkAllowExposure.Text = "Auto exposure/contrast";
            chkAllowExposure.UseVisualStyleBackColor = true;
            // 
            // chkAllowSharpen
            // 
            chkAllowSharpen.AutoSize = true;
            chkAllowSharpen.Location = new Point(674, 12);
            chkAllowSharpen.Margin = new Padding(3, 12, 12, 3);
            chkAllowSharpen.Name = "chkAllowSharpen";
            chkAllowSharpen.Size = new Size(138, 19);
            chkAllowSharpen.TabIndex = 5;
            chkAllowSharpen.Text = "Auto sharpen/grain";
            chkAllowSharpen.UseVisualStyleBackColor = true;
            // 
            // chkAllowFaceRestore
            // 
            chkAllowFaceRestore.AutoSize = true;
            chkAllowFaceRestore.Location = new Point(786, 12);
            chkAllowFaceRestore.Margin = new Padding(3, 12, 12, 3);
            chkAllowFaceRestore.Name = "chkAllowFaceRestore";
            chkAllowFaceRestore.Size = new Size(203, 19);
            chkAllowFaceRestore.TabIndex = 6;
            chkAllowFaceRestore.Text = "Auto face restore (GFPGAN+CF)";
            chkAllowFaceRestore.UseVisualStyleBackColor = true;
            // 
            // chkAllowColorize
            // 
            chkAllowColorize.AutoSize = true;
            chkAllowColorize.Location = new Point(3, 51);
            chkAllowColorize.Margin = new Padding(3, 12, 12, 3);
            chkAllowColorize.Name = "chkAllowColorize";
            chkAllowColorize.Size = new Size(152, 19);
            chkAllowColorize.TabIndex = 7;
            chkAllowColorize.Text = "Auto colorize (DDColor)";
            chkAllowColorize.UseVisualStyleBackColor = true;
            // 
            // chkAllowUpscale
            // 
            chkAllowUpscale.AutoSize = true;
            chkAllowUpscale.Location = new Point(170, 51);
            chkAllowUpscale.Margin = new Padding(3, 12, 12, 3);
            chkAllowUpscale.Name = "chkAllowUpscale";
            chkAllowUpscale.Size = new Size(150, 19);
            chkAllowUpscale.TabIndex = 8;
            chkAllowUpscale.Text = "Auto upscale (ESRGAN)";
            chkAllowUpscale.UseVisualStyleBackColor = true;
            // 
            // body
            // 
            body.ColumnCount = 2;
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            body.Controls.Add(panelInput, 0, 0);
            body.Controls.Add(panelOutput, 1, 0);
            body.Controls.Add(panelLog, 0, 1);
            body.Dock = DockStyle.Fill;
            body.Location = new Point(0, 66);
            body.Name = "body";
            body.RowCount = 2;
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 78F));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
            body.Size = new Size(1100, 634);
            body.TabIndex = 1;
            // 
            // panelInput
            // 
            panelInput.Controls.Add(picInput);
            panelInput.Controls.Add(lblInputTitle);
            panelInput.Dock = DockStyle.Fill;
            panelInput.Location = new Point(12, 12);
            panelInput.Margin = new Padding(12);
            panelInput.Name = "panelInput";
            panelInput.Padding = new Padding(12);
            panelInput.Size = new Size(526, 470);
            panelInput.TabIndex = 0;
            // 
            // picInput
            // 
            picInput.AllowDrop = true;
            picInput.Dock = DockStyle.Fill;
            picInput.Location = new Point(12, 36);
            picInput.Name = "picInput";
            picInput.Size = new Size(502, 422);
            picInput.SizeMode = PictureBoxSizeMode.Zoom;
            picInput.TabIndex = 1;
            picInput.TabStop = false;
            // 
            // lblInputTitle
            // 
            lblInputTitle.Dock = DockStyle.Top;
            lblInputTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInputTitle.Location = new Point(12, 12);
            lblInputTitle.Name = "lblInputTitle";
            lblInputTitle.Size = new Size(502, 24);
            lblInputTitle.TabIndex = 0;
            lblInputTitle.Text = "INPUT SLIKA";
            // 
            // panelOutput
            // 
            panelOutput.Controls.Add(picOutput);
            panelOutput.Controls.Add(lblOutputTitle);
            panelOutput.Dock = DockStyle.Fill;
            panelOutput.Location = new Point(562, 12);
            panelOutput.Margin = new Padding(12);
            panelOutput.Name = "panelOutput";
            panelOutput.Padding = new Padding(12);
            panelOutput.Size = new Size(526, 470);
            panelOutput.TabIndex = 1;
            // 
            // picOutput
            // 
            picOutput.Dock = DockStyle.Fill;
            picOutput.Location = new Point(12, 36);
            picOutput.Name = "picOutput";
            picOutput.Size = new Size(502, 422);
            picOutput.SizeMode = PictureBoxSizeMode.Zoom;
            picOutput.TabIndex = 1;
            picOutput.TabStop = false;
            // 
            // lblOutputTitle
            // 
            lblOutputTitle.Dock = DockStyle.Top;
            lblOutputTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOutputTitle.Location = new Point(12, 12);
            lblOutputTitle.Name = "lblOutputTitle";
            lblOutputTitle.Size = new Size(502, 24);
            lblOutputTitle.TabIndex = 0;
            lblOutputTitle.Text = "OUTPUT SLIKA";
            // 
            // panelLog
            // 
            body.SetColumnSpan(panelLog, 2);
            panelLog.Controls.Add(txtLog);
            panelLog.Controls.Add(panelStatus);
            panelLog.Dock = DockStyle.Fill;
            panelLog.Location = new Point(12, 506);
            panelLog.Margin = new Padding(12);
            panelLog.Name = "panelLog";
            panelLog.Padding = new Padding(12);
            panelLog.Size = new Size(1076, 116);
            panelLog.TabIndex = 2;
            // 
            // txtLog
            // 
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(12, 12);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1052, 52);
            txtLog.TabIndex = 0;
            // 
            // panelStatus
            // 
            panelStatus.Controls.Add(lblStatus);
            panelStatus.Controls.Add(progress);
            panelStatus.Dock = DockStyle.Bottom;
            panelStatus.Location = new Point(12, 64);
            panelStatus.Name = "panelStatus";
            panelStatus.Padding = new Padding(0, 6, 0, 0);
            panelStatus.Size = new Size(1052, 40);
            panelStatus.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Location = new Point(0, 12);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(4, 0, 0, 0);
            lblStatus.Size = new Size(1052, 28);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Status";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progress
            // 
            progress.Dock = DockStyle.Top;
            progress.Location = new Point(0, 6);
            progress.Name = "progress";
            progress.Size = new Size(1052, 6);
            progress.TabIndex = 1;
            // 
            // Form11
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(body);
            Controls.Add(panelTop);
            Name = "Form11";
            Text = "AutoFix";
            panelTop.ResumeLayout(false);
            flowTop.ResumeLayout(false);
            flowTop.PerformLayout();
            body.ResumeLayout(false);
            panelInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picInput).EndInit();
            panelOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picOutput).EndInit();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            panelStatus.ResumeLayout(false);
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}
