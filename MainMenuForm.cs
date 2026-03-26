using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    public partial class MainMenuForm : Form
    {
        private const int BaseMinWindowWidth = 900;
        private const int BaseMinWindowHeight = 650;
        private const int BaseHeaderHeight = 45;
        private const int BaseNavButtonHeight = 52;
        private const int BaseExitButtonHeight = 45;
        private const int BaseNavGap = 6;
        private const int BaseLogoHeight = 180;
        private const int BaseLogoPadding = 16;

        private Form? currentChild;
        private readonly Panel panelNavMenu = new();
        private readonly Panel panelNavFooter = new();
        private Button[] navButtons = Array.Empty<Button>();
        private bool scaledLayoutPending;

        public MainMenuForm()
        {
            InitializeComponent();

            // Izbjegni runtime inicijalizaciju u design modu
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;

            BackColor = Color.FromArgb(15, 23, 42);
            panelNav.BackColor = Color.FromArgb(17, 24, 39);
            panelHeader.BackColor = Color.FromArgb(23, 31, 50);
            panelContent.BackColor = Color.FromArgb(236, 239, 244);

            navButtons = new[]
            {
                btnBasicTransforms,
                btnSuperResolution,
                btnOption3,
                btnDenoise,
                btnColorize,
                btnStyleTransfer,
                btnObjectDetection,
                btnCartoonify,
                btnFilmRestore,
                btnObjectRemoval,
                btnAutoFix
            };

            StyleNavButton(btnBasicTransforms);
            StyleNavButton(btnSuperResolution);
            StyleNavButton(btnOption3);
            StyleNavButton(btnDenoise);
            StyleNavButton(btnColorize);
            StyleNavButton(btnStyleTransfer);
            StyleNavButton(btnObjectDetection);
            StyleNavButton(btnCartoonify);
            StyleNavButton(btnFilmRestore);
            StyleNavButton(btnObjectRemoval);
            StyleNavButton(btnAutoFix);
            StyleNavButton(btnExit, true);

            lblTitle.ForeColor = Color.White;
            ConfigureNavigationLayout();
            InitNavLogo();
            UpdateScaledLayout();

            Resize += (_, __) => QueueScaledLayout();
            DpiChanged += (_, __) => QueueScaledLayout();
            Shown += (_, __) => BeginInvoke((Action)InitializeStartupLayout);

            btnBasicTransforms.Click += (s, e) => TryLoadChild(() => new Form1(), "Basic Transforms");
            btnSuperResolution.Click += (s, e) => TryLoadChild(() => new Form2(), "Super Resolution");
            btnOption3.Click += (s, e) => TryLoadChild(() => new Form3(), "Face Restore (GFPGAN)");
            btnDenoise.Click += (s, e) => TryLoadChild(() => new Form4(), "Denoise/Restore (CodeFormer)");
            btnColorize.Click += (s, e) => TryLoadChild(() => new Form5(), "Colorize (DDColor)");
            btnStyleTransfer.Click += (s, e) => TryLoadChild(() => new Form6(), "Style Transfer (AdaIN)");
            btnObjectDetection.Click += (s, e) => TryLoadChild(() => new Form7(), "Object Detection (YOLO)");
            btnCartoonify.Click += (s, e) => TryLoadChild(() => new Form8(), "Cartoonify (AnimeGAN)");
            btnFilmRestore.Click += (s, e) => TryLoadChild(() => new Form9(), "Film Restore");
            btnObjectRemoval.Click += (s, e) => TryLoadChild(() => new Form10(), "Object Removal");
            btnAutoFix.Click += (s, e) => TryLoadChild(() => new Form11(), "AutoFix");
            btnExit.Click += (s, e) => Application.Exit();

        }

        private void InitializeStartupLayout()
        {
            UpdateScaledLayout();
            TryLoadChild(() => new WelcomeForm(), "Welcome");
            QueueScaledLayout();
        }

        private void ConfigureNavigationLayout()
        {
            panelNavMenu.BackColor = panelNav.BackColor;
            panelNavMenu.Dock = DockStyle.Fill;
            panelNavMenu.AutoScroll = true;
            panelNavMenu.Margin = new Padding(0);
            panelNavMenu.Padding = new Padding(0);

            panelNavFooter.BackColor = panelNav.BackColor;
            panelNavFooter.Dock = DockStyle.Bottom;
            panelNavFooter.Margin = new Padding(0);
            panelNavFooter.Padding = new Padding(0);

            panelNavLogo.BackColor = panelNav.BackColor;
            panelNavLogo.Dock = DockStyle.Fill;
            panelNavLogo.Margin = new Padding(0);

            pictureNavLogo.Dock = DockStyle.Fill;
            pictureNavLogo.Location = Point.Empty;
            pictureNavLogo.Margin = new Padding(0);

            btnExit.Dock = DockStyle.Bottom;
            btnExit.Margin = new Padding(0);

            panelNav.SuspendLayout();
            panelNavMenu.SuspendLayout();
            panelNavFooter.SuspendLayout();

            panelNav.Controls.Clear();
            panelNavMenu.Controls.Clear();
            panelNavFooter.Controls.Clear();

            foreach (Button button in navButtons)
            {
                button.Dock = DockStyle.None;
                button.Margin = Padding.Empty;
                panelNavMenu.Controls.Add(button);
            }

            panelNavFooter.Controls.Add(panelNavLogo);
            panelNavFooter.Controls.Add(btnExit);

            panelNav.Controls.Add(panelNavMenu);
            panelNav.Controls.Add(panelNavFooter);
            panelNav.Controls.Add(panelHeader);

            panelNavFooter.ResumeLayout(false);
            panelNavMenu.ResumeLayout(false);
            panelNav.ResumeLayout(false);
        }

        private void InitNavLogo()
        {
            panelNavLogo.BackColor = panelNav.BackColor;
            pictureNavLogo.BackColor = panelNav.BackColor;

            var logo = LogoAssets.CreateMiniLogo();
            if (logo != null)
            {
                pictureNavLogo.Image = logo;
            }
            else
            {
                panelNavLogo.Visible = false;
            }

            QueueScaledLayout();
        }

        private static void StyleNavButton(Button btn, bool isExit = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = isExit ? Color.FromArgb(185, 28, 28) : Color.FromArgb(37, 47, 73);
            btn.ForeColor = Color.White;
            btn.AutoEllipsis = true;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Cursor = Cursors.Hand;
        }

        private void UpdateScaledLayout()
        {
            if (!IsHandleCreated)
                return;

            MinimumSize = new Size(BaseMinWindowWidth, BaseMinWindowHeight);

            panelNav.SuspendLayout();
            panelNavFooter.SuspendLayout();

            panelHeader.Height = Math.Max(ScaleValue(BaseHeaderHeight), lblTitle.PreferredHeight + ScaleValue(16));

            btnExit.Padding = new Padding(ScaleValue(12), ScaleValue(6), ScaleValue(12), ScaleValue(6));
            btnExit.Height = MeasureButtonHeight(btnExit, panelNav.ClientSize.Width, BaseExitButtonHeight);

            int logoHeight = 0;
            if (panelNavLogo.Visible)
            {
                panelNavLogo.Padding = new Padding(ScaleValue(BaseLogoPadding));
                logoHeight = Math.Max(
                    ScaleValue(96),
                    Math.Min(ScaleValue(BaseLogoHeight), Math.Max(0, panelNav.ClientSize.Height / 4)));
            }

            panelNavFooter.Height = btnExit.Height + logoHeight;

            panelNavFooter.ResumeLayout(true);
            panelNav.ResumeLayout(true);
            panelNav.PerformLayout();

            int gap = ScaleValue(BaseNavGap);
            int navWidth = Math.Max(ScaleValue(160), panelNavMenu.ClientSize.Width);
            int totalHeight = LayoutNavigationButtons(navWidth, gap);

            if (totalHeight > panelNavMenu.ClientSize.Height)
            {
                int scrollAwareWidth = Math.Max(ScaleValue(160), navWidth - SystemInformation.VerticalScrollBarWidth);
                totalHeight = LayoutNavigationButtons(scrollAwareWidth, gap);
            }

            panelNavMenu.AutoScrollMinSize = new Size(0, Math.Max(0, totalHeight - gap));
        }

        private int MeasureButtonHeight(Button button, int width, int baseHeight)
        {
            int textWidth = Math.Max(1, width - (button.Padding.Horizontal + ScaleValue(12)));
            Size textSize = TextRenderer.MeasureText(
                button.Text,
                button.Font,
                new Size(textWidth, int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter);

            return Math.Max(ScaleValue(baseHeight), textSize.Height + button.Padding.Vertical + ScaleValue(8));
        }

        private int LayoutNavigationButtons(int buttonWidth, int gap)
        {
            int y = 0;

            foreach (Button button in navButtons)
            {
                button.Padding = new Padding(ScaleValue(12), ScaleValue(6), ScaleValue(12), ScaleValue(6));
                button.Width = buttonWidth;
                button.Height = MeasureButtonHeight(button, buttonWidth, BaseNavButtonHeight);
                button.Location = new Point(0, y);
                y += button.Height + gap;
            }

            return y;
        }

        private int ScaleValue(int value)
        {
            float scale = DeviceDpi > 0 ? DeviceDpi / 96f : 1f;
            return Math.Max(1, (int)Math.Round(value * scale));
        }

        private void QueueScaledLayout()
        {
            if (!IsHandleCreated || scaledLayoutPending)
                return;

            scaledLayoutPending = true;
            BeginInvoke((Action)(() =>
            {
                scaledLayoutPending = false;
                UpdateScaledLayout();
            }));
        }

        private void LoadChild(Form form)
        {
            currentChild?.Close();
            currentChild?.Dispose();

            currentChild = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
            QueueScaledLayout();
        }

        private void TryLoadChild(Func<Form> createForm, string featureName)
        {
            try
            {
                var form = createForm();
                LoadChild(form);
            }
            catch (Exception ex)
            {
                CrashLogger.Log(ex, $"MainMenuForm.TryLoadChild ({featureName})");
                MessageBox.Show(
                    $"Ne mogu otvoriti modul: {featureName}\n\n{ex.GetType().Name}: {ex.Message}\n\nDetalji su zapisani u log.",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private void btnObjectDetection_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnFilmRestore_Click(object sender, EventArgs e)
        {

        }
    }
}
