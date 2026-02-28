using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    public partial class MainMenuForm : Form
    {
        private Form? currentChild;

        public MainMenuForm()
        {
            InitializeComponent();

            // Izbjegni runtime inicijalizaciju u design modu
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 900);
            DoubleBuffered = true;

            BackColor = Color.FromArgb(15, 23, 42);
            panelNav.BackColor = Color.FromArgb(17, 24, 39);
            panelHeader.BackColor = Color.FromArgb(23, 31, 50);
            panelContent.BackColor = Color.FromArgb(236, 239, 244);

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
            InitNavLogo();

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

            TryLoadChild(() => new WelcomeForm(), "Welcome");
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
        }

        private static void StyleNavButton(Button btn, bool isExit = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = isExit ? Color.FromArgb(185, 28, 28) : Color.FromArgb(37, 47, 73);
            btn.ForeColor = Color.White;
            btn.Margin = new Padding(0, 0, 0, 6);
            btn.Cursor = Cursors.Hand;
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
