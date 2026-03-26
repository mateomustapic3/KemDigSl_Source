using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    partial class WelcomeForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelRoot;
        private FlowLayoutPanel flowContent;
        private Label lblWelcome;
        private TableLayoutPanel tableSupport;
        private PictureBox pictureMainLogo;
        private Label lblSupport;

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
            panelRoot = new Panel();
            flowContent = new FlowLayoutPanel();
            lblWelcome = new Label();
            tableSupport = new TableLayoutPanel();
            pictureMainLogo = new PictureBox();
            lblSupport = new Label();
            panelRoot.SuspendLayout();
            flowContent.SuspendLayout();
            tableSupport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureMainLogo).BeginInit();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.AutoScroll = true;
            panelRoot.Controls.Add(flowContent);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(0);
            panelRoot.TabIndex = 0;
            // 
            // flowContent
            // 
            flowContent.AutoSize = true;
            flowContent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowContent.Controls.Add(lblWelcome);
            flowContent.Controls.Add(tableSupport);
            flowContent.FlowDirection = FlowDirection.TopDown;
            flowContent.Margin = new Padding(0);
            flowContent.Name = "flowContent";
            flowContent.TabIndex = 0;
            flowContent.WrapContents = false;
            // 
            // lblWelcome
            // 
            lblWelcome.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Margin = new Padding(0, 0, 0, 28);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Text = "Dobrodošli u KemDigSl aplikaciju za AI transformaciju slika. Molimo izaberite jedan od izbornika s lijeve strane za početak.";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableSupport
            // 
            tableSupport.AutoSize = true;
            tableSupport.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableSupport.ColumnCount = 2;
            tableSupport.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableSupport.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableSupport.Controls.Add(pictureMainLogo, 0, 0);
            tableSupport.Controls.Add(lblSupport, 1, 0);
            tableSupport.Margin = new Padding(0);
            tableSupport.Name = "tableSupport";
            tableSupport.RowCount = 1;
            tableSupport.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableSupport.TabIndex = 1;
            // 
            // pictureMainLogo
            // 
            pictureMainLogo.Margin = new Padding(0, 0, 24, 0);
            pictureMainLogo.Name = "pictureMainLogo";
            pictureMainLogo.Size = new Size(240, 300);
            pictureMainLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureMainLogo.TabStop = false;
            // 
            // lblSupport
            // 
            lblSupport.Font = new Font("Segoe UI", 11F);
            lblSupport.ForeColor = Color.White;
            lblSupport.Margin = new Padding(0);
            lblSupport.Name = "lblSupport";
            lblSupport.Text = "Ovaj rad nastao je uz potporu projekta Kulturna slika Dubrovnika kroz transformacije tiskarske baštine od prvih tiskanih izdanja do digitalnih formata (KulTisk), koji se provodi na Sveučilištu u Dubrovniku i financira sredstvima fonda NextGenerationEU.";
            lblSupport.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(900, 600);
            Controls.Add(panelRoot);
            Name = "WelcomeForm";
            Text = "Welcome";
            panelRoot.ResumeLayout(false);
            panelRoot.PerformLayout();
            flowContent.ResumeLayout(false);
            flowContent.PerformLayout();
            tableSupport.ResumeLayout(false);
            tableSupport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureMainLogo).EndInit();
            ResumeLayout(false);
        }
    }
}
