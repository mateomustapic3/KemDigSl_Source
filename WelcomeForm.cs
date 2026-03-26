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
        private static readonly TextFormatFlags MultilineTextFlags = TextFormatFlags.WordBreak;
        private bool supportStacked;
        private bool layoutUpdatePending;

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
            panelRoot.Resize += (_, __) => QueueLayoutUpdate();
            Resize += (_, __) => QueueLayoutUpdate();
            FontChanged += (_, __) => QueueLayoutUpdate();
            Shown += (_, __) => QueueLayoutUpdate();
            VisibleChanged += (_, __) =>
            {
                if (Visible && IsHandleCreated)
                    QueueLayoutUpdate();
            };
        }

        private void UpdateLayout()
        {
            Size viewport = panelRoot.ClientSize;
            if (viewport.Width <= 0 || viewport.Height <= 0)
                return;

            int available = Math.Max(MinContentWidth, viewport.Width - (SidePadding * 2));
            int contentWidth = Math.Min(MaxContentWidth, available);

            ApplyLabelLayout(lblWelcome, contentWidth);

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
            ApplyLabelLayout(lblSupport, supportWidth);

            flowContent.MaximumSize = new Size(contentWidth, 0);
            tableSupport.MaximumSize = new Size(contentWidth, 0);
            tableSupport.PerformLayout();
            flowContent.PerformLayout();

            panelRoot.AutoScrollMinSize = new Size(0, flowContent.Height + (SidePadding * 2));

            int x = (viewport.Width - flowContent.Width) / 2;
            x = Math.Max(SidePadding, x);
            int y = flowContent.Height + (SidePadding * 2) <= viewport.Height
                ? Math.Max(SidePadding, (viewport.Height - flowContent.Height) / 2)
                : SidePadding;
            flowContent.Location = new Point(x, y);
        }

        private void QueueLayoutUpdate()
        {
            if (!IsHandleCreated || layoutUpdatePending)
                return;

            layoutUpdatePending = true;
            BeginInvoke((Action)(() =>
            {
                layoutUpdatePending = false;
                UpdateLayout();
            }));
        }

        private static void ApplyLabelLayout(Label label, int width)
        {
            width = Math.Max(1, width);
            Size measured = TextRenderer.MeasureText(
                label.Text,
                label.Font,
                new Size(width, int.MaxValue),
                MultilineTextFlags);

            label.MaximumSize = new Size(width, 0);
            label.MinimumSize = Size.Empty;
            label.Size = new Size(width, Math.Max(label.Font.Height + 4, measured.Height));
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
