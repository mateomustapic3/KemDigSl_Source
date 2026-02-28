using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    public partial class WelcomeForm : Form
    {
        private const int SidePadding = 28;
        private const int MinContentWidth = 320;
        private const int MaxContentWidth = 1100;
        private const int SupportGap = 24;
        private bool supportStacked;

        public WelcomeForm()
        {
            InitializeComponent();

            BackColor = Theme.Background;
            panelRoot.BackColor = Theme.Background;
            flowContent.BackColor = Theme.Background;
            tableSupport.BackColor = Theme.Background;

            lblWelcome.ForeColor = Theme.Text;
            lblSupport.ForeColor = Theme.Text;

            var logo = LogoAssets.CreateMainLogo();
            if (logo != null)
            {
                pictureMainLogo.Image = logo;
            }
            else
            {
                pictureMainLogo.Visible = false;
            }

            UpdateLayout();
            Resize += (_, __) => UpdateLayout();
        }

        private void UpdateLayout()
        {
            int available = Math.Max(MinContentWidth, ClientSize.Width - (SidePadding * 2));
            int contentWidth = Math.Min(MaxContentWidth, available);

            lblWelcome.MaximumSize = new Size(contentWidth, 0);

            bool stackSupport = contentWidth < 650;
            SetSupportLayout(stackSupport);

            if (pictureMainLogo.Visible)
            {
                int baseMaxWidth = stackSupport ? Math.Min(280, contentWidth) : Math.Min(260, contentWidth / 3);
                int logoMaxWidth = Math.Max(stackSupport ? 160 : 140, baseMaxWidth);
                int logoMaxHeight = Math.Min(360, (int)Math.Round(logoMaxWidth * 1.25));
                pictureMainLogo.Size = LogoAssets.GetMainLogoFitSize(logoMaxWidth, logoMaxHeight);
            }

            int logoBlock = pictureMainLogo.Visible ? pictureMainLogo.Width + SupportGap : 0;
            int supportWidth = stackSupport ? contentWidth : Math.Max(160, contentWidth - logoBlock);
            lblSupport.MaximumSize = new Size(supportWidth, 0);

            flowContent.MaximumSize = new Size(contentWidth, 0);
            flowContent.PerformLayout();

            int x = (ClientSize.Width - flowContent.Width) / 2;
            x = Math.Max(SidePadding, x);
            int y = (ClientSize.Height - flowContent.Height) / 2;
            y = Math.Max(SidePadding, y);
            flowContent.Location = new Point(x, y);
        }

        private void SetSupportLayout(bool stacked)
        {
            if (supportStacked == stacked)
                return;

            supportStacked = stacked;

            tableSupport.SuspendLayout();
            tableSupport.Controls.Clear();
            tableSupport.ColumnStyles.Clear();
            tableSupport.RowStyles.Clear();

            if (stacked)
            {
                tableSupport.ColumnCount = 1;
                tableSupport.RowCount = 2;
                tableSupport.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tableSupport.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableSupport.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                pictureMainLogo.Margin = new Padding(0, 0, 0, 16);
                lblSupport.TextAlign = ContentAlignment.TopCenter;
                tableSupport.Controls.Add(pictureMainLogo, 0, 0);
                tableSupport.Controls.Add(lblSupport, 0, 1);
            }
            else
            {
                tableSupport.ColumnCount = 2;
                tableSupport.RowCount = 1;
                tableSupport.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tableSupport.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tableSupport.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                pictureMainLogo.Margin = new Padding(0, 0, SupportGap, 0);
                lblSupport.TextAlign = ContentAlignment.MiddleLeft;
                tableSupport.Controls.Add(pictureMainLogo, 0, 0);
                tableSupport.Controls.Add(lblSupport, 1, 0);
            }

            tableSupport.ResumeLayout(true);
        }
    }
}
