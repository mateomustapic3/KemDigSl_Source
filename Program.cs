namespace Project
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Force a 100% layout baseline and let Windows scale the final surface on high-DPI displays.
            // This avoids DPI-specific control reflow issues on laptops set to 125%+ scaling.
            Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CrashLogger.Init();
            Application.Run(new MainMenuForm());
        }
    }
}
