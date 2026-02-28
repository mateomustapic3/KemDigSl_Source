using System.Drawing;
using System.Windows.Forms;

namespace Project
{
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(30, 30, 30);
        public static readonly Color Panel = Color.FromArgb(35, 35, 35);
        public static readonly Color PanelAlt = Color.FromArgb(40, 40, 40);
        public static readonly Color SidePanel = Color.FromArgb(25, 25, 25);

        public static readonly Color BtnPrimary = Color.FromArgb(70, 70, 120);       // load
        public static readonly Color BtnReload = Color.FromArgb(70, 120, 70);        // refresh
        public static readonly Color BtnAccent = Color.FromArgb(90, 110, 140);       // add style / misc
        public static readonly Color BtnAction = Color.FromArgb(120, 80, 40);        // apply/transform
        public static readonly Color BtnSave = Color.FromArgb(80, 80, 80);           // save/export
        public static readonly Color BtnClear = Color.FromArgb(90, 60, 110);         // clear/reset

        public static readonly Color Text = Color.White;

        public static void StyleButton(Button button, Color backColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backColor;
            button.ForeColor = Text;
        }

        public static void StyleLabel(Label label)
        {
            label.ForeColor = Text;
        }

        public static void StyleComboBox(ComboBox combo)
        {
            combo.BackColor = SidePanel;
            combo.ForeColor = Text;
            combo.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleTrackBar(TrackBar track)
        {
            track.BackColor = PanelAlt;
        }
    }
}
