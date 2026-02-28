namespace Project
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnBasicTransforms;
        private Button btnSuperResolution;
        private Button btnOption3;
        private Button btnColorize;
        private Button btnExit;
        private Button btnDenoise;
        private Button btnStyleTransfer;
        private Button btnFilmRestore;
        private Label lblTitle;
        private Panel panelNav;
        private Panel panelContent;
        private Panel panelHeader;
        private Panel panelNavLogo;
        private PictureBox pictureNavLogo;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            btnBasicTransforms = new Button();
            btnSuperResolution = new Button();
            btnOption3 = new Button();
            btnColorize = new Button();
            btnExit = new Button();
            btnStyleTransfer = new Button();
            btnDenoise = new Button();
            lblTitle = new Label();
            panelNav = new Panel();
            btnAutoFix = new Button();
            btnObjectRemoval = new Button();
            btnFilmRestore = new Button();
            btnCartoonify = new Button();
            btnObjectDetection = new Button();
            panelHeader = new Panel();
            panelNavLogo = new Panel();
            pictureNavLogo = new PictureBox();
            panelContent = new Panel();
            panelNav.SuspendLayout();
            panelHeader.SuspendLayout();
            panelNavLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureNavLogo).BeginInit();
            SuspendLayout();
            // 
            // btnBasicTransforms
            // 
            btnBasicTransforms.Dock = DockStyle.Top;
            btnBasicTransforms.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBasicTransforms.Location = new Point(0, 45);
            btnBasicTransforms.Margin = new Padding(3, 2, 3, 2);
            btnBasicTransforms.Name = "btnBasicTransforms";
            btnBasicTransforms.Size = new Size(192, 52);
            btnBasicTransforms.TabIndex = 0;
            btnBasicTransforms.Text = "Osnovne transformacije";
            // 
            // btnSuperResolution
            // 
            btnSuperResolution.Dock = DockStyle.Top;
            btnSuperResolution.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSuperResolution.Location = new Point(0, 97);
            btnSuperResolution.Margin = new Padding(3, 2, 3, 2);
            btnSuperResolution.Name = "btnSuperResolution";
            btnSuperResolution.Size = new Size(192, 52);
            btnSuperResolution.TabIndex = 1;
            btnSuperResolution.Text = "Upscale/Downscale (ESRGAN)";
            // 
            // btnOption3
            // 
            btnOption3.Dock = DockStyle.Top;
            btnOption3.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOption3.Location = new Point(0, 149);
            btnOption3.Margin = new Padding(3, 2, 3, 2);
            btnOption3.Name = "btnOption3";
            btnOption3.Size = new Size(192, 52);
            btnOption3.TabIndex = 2;
            btnOption3.Text = "Obnova lica (GFPGAN)";
            // 
            // btnColorize
            // 
            btnColorize.Dock = DockStyle.Top;
            btnColorize.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnColorize.Location = new Point(0, 253);
            btnColorize.Margin = new Padding(3, 2, 3, 2);
            btnColorize.Name = "btnColorize";
            btnColorize.Size = new Size(192, 52);
            btnColorize.TabIndex = 4;
            btnColorize.Text = "Kolorizacija (DDColor)";
            // 
            // btnExit
            // 
            btnExit.Dock = DockStyle.Bottom;
            btnExit.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(0, 720);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(192, 45);
            btnExit.TabIndex = 5;
            btnExit.Text = "Izlaz";
            // 
            // btnStyleTransfer
            // 
            btnStyleTransfer.Dock = DockStyle.Top;
            btnStyleTransfer.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStyleTransfer.Location = new Point(0, 305);
            btnStyleTransfer.Margin = new Padding(3, 2, 3, 2);
            btnStyleTransfer.Name = "btnStyleTransfer";
            btnStyleTransfer.Size = new Size(192, 52);
            btnStyleTransfer.TabIndex = 5;
            btnStyleTransfer.Text = "Style Transfer (AdaIN)";
            // 
            // btnDenoise
            // 
            btnDenoise.Dock = DockStyle.Top;
            btnDenoise.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDenoise.Location = new Point(0, 201);
            btnDenoise.Margin = new Padding(3, 2, 3, 2);
            btnDenoise.Name = "btnDenoise";
            btnDenoise.Size = new Size(192, 52);
            btnDenoise.TabIndex = 3;
            btnDenoise.Text = "Ukloni šum/blur (CODEFORMER)";
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(192, 45);
            lblTitle.TabIndex = 6;
            lblTitle.Text = "KemDigSl";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Click += lblTitle_Click;
            // 
            // panelNav
            // 
            panelNav.BackColor = Color.WhiteSmoke;
            panelNav.Controls.Add(btnAutoFix);
            panelNav.Controls.Add(btnObjectRemoval);
            panelNav.Controls.Add(btnFilmRestore);
            panelNav.Controls.Add(btnCartoonify);
            panelNav.Controls.Add(btnObjectDetection);
            panelNav.Controls.Add(btnStyleTransfer);
            panelNav.Controls.Add(btnColorize);
            panelNav.Controls.Add(btnDenoise);
            panelNav.Controls.Add(btnOption3);
            panelNav.Controls.Add(btnSuperResolution);
            panelNav.Controls.Add(btnBasicTransforms);
            panelNav.Controls.Add(panelHeader);
            panelNav.Controls.Add(panelNavLogo);
            panelNav.Controls.Add(btnExit);
            panelNav.Dock = DockStyle.Left;
            panelNav.Location = new Point(0, 0);
            panelNav.Margin = new Padding(3, 2, 3, 2);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(192, 765);
            panelNav.TabIndex = 7;
            // 
            // btnAutoFix
            // 
            btnAutoFix.Dock = DockStyle.Top;
            btnAutoFix.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAutoFix.Location = new Point(0, 565);
            btnAutoFix.Margin = new Padding(3, 2, 3, 2);
            btnAutoFix.Name = "btnAutoFix";
            btnAutoFix.Size = new Size(192, 52);
            btnAutoFix.TabIndex = 12;
            btnAutoFix.Text = "AutoFix (Svi modeli)";
            // 
            // btnObjectRemoval
            // 
            btnObjectRemoval.Dock = DockStyle.Top;
            btnObjectRemoval.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnObjectRemoval.Location = new Point(0, 513);
            btnObjectRemoval.Margin = new Padding(3, 2, 3, 2);
            btnObjectRemoval.Name = "btnObjectRemoval";
            btnObjectRemoval.Size = new Size(192, 52);
            btnObjectRemoval.TabIndex = 11;
            btnObjectRemoval.Text = "Object removal (LaMa)";
            // 
            // btnFilmRestore
            // 
            btnFilmRestore.Dock = DockStyle.Top;
            btnFilmRestore.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFilmRestore.Location = new Point(0, 461);
            btnFilmRestore.Margin = new Padding(3, 2, 3, 2);
            btnFilmRestore.Name = "btnFilmRestore";
            btnFilmRestore.Size = new Size(192, 52);
            btnFilmRestore.TabIndex = 10;
            btnFilmRestore.Text = "Film grain && scratch (LaMa)";
            btnFilmRestore.Click += btnFilmRestore_Click;
            // 
            // btnCartoonify
            // 
            btnCartoonify.Dock = DockStyle.Top;
            btnCartoonify.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCartoonify.Location = new Point(0, 409);
            btnCartoonify.Margin = new Padding(3, 2, 3, 2);
            btnCartoonify.Name = "btnCartoonify";
            btnCartoonify.Size = new Size(192, 52);
            btnCartoonify.TabIndex = 9;
            btnCartoonify.Text = "Cartoonify (AnimeGANv2)";
            // 
            // btnObjectDetection
            // 
            btnObjectDetection.Dock = DockStyle.Top;
            btnObjectDetection.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnObjectDetection.Location = new Point(0, 357);
            btnObjectDetection.Margin = new Padding(3, 2, 3, 2);
            btnObjectDetection.Name = "btnObjectDetection";
            btnObjectDetection.Size = new Size(192, 52);
            btnObjectDetection.TabIndex = 8;
            btnObjectDetection.Text = "Object Detection (YOLOv8n)";
            btnObjectDetection.Click += btnObjectDetection_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 2, 3, 2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(192, 45);
            panelHeader.TabIndex = 7;
            // 
            // panelNavLogo
            // 
            panelNavLogo.Controls.Add(pictureNavLogo);
            panelNavLogo.Dock = DockStyle.Bottom;
            panelNavLogo.Location = new Point(0, 540);
            panelNavLogo.Margin = new Padding(0);
            panelNavLogo.Name = "panelNavLogo";
            panelNavLogo.Padding = new Padding(16);
            panelNavLogo.Size = new Size(192, 180);
            panelNavLogo.TabIndex = 13;
            // 
            // pictureNavLogo
            // 
            pictureNavLogo.Location = new Point(13, 0);
            pictureNavLogo.Name = "pictureNavLogo";
            pictureNavLogo.Size = new Size(160, 148);
            pictureNavLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureNavLogo.TabIndex = 0;
            pictureNavLogo.TabStop = false;
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.White;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(192, 0);
            panelContent.Margin = new Padding(3, 2, 3, 2);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(560, 765);
            panelContent.TabIndex = 8;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(752, 765);
            Controls.Add(panelContent);
            Controls.Add(panelNav);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainMenuForm";
            Text = "KemDigSl - Kemijanje Digitalnih Slika";
            panelNav.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelNavLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureNavLogo).EndInit();
            ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private Button btnObjectDetection;
        private Button btnCartoonify;
        private Button btnObjectRemoval;
        private Button btnAutoFix;
    }
}
