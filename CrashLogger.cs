using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    internal static class CrashLogger
    {
        private static readonly object Gate = new();
        private static bool initialized;

        internal static void Init()
        {
            lock (Gate)
            {
                if (initialized)
                    return;

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                Application.ThreadException += (_, e) => Handle(e.Exception, "Application.ThreadException");
                AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                {
                    if (e.ExceptionObject is Exception ex)
                        Handle(ex, "AppDomain.CurrentDomain.UnhandledException");
                    else
                        Handle(new Exception("Unknown unhandled exception object."), "AppDomain.CurrentDomain.UnhandledException");
                };

                TaskScheduler.UnobservedTaskException += (_, e) =>
                {
                    Handle(e.Exception, "TaskScheduler.UnobservedTaskException");
                    e.SetObserved();
                };

                initialized = true;
            }
        }

        internal static string Handle(Exception ex, string context)
        {
            string path = Log(ex, context);

            try
            {
                MessageBox.Show(
                    $"Aplikacija je naišla na grešku.\n\n{ex.GetType().Name}: {ex.Message}\n\nLog: {path}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch
            {
                // ignore UI errors while handling a crash
            }

            return path;
        }

        internal static string Log(Exception ex, string context)
        {
            try
            {
                string dir = Path.Combine(Path.GetTempPath(), "WindowsFormsApp", "logs");
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"crash_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.txt");

                var sb = new StringBuilder();
                sb.AppendLine("WindowsFormsApp crash log");
                sb.AppendLine($"Timestamp: {DateTime.Now:O}");
                sb.AppendLine($"Context: {context}");
                sb.AppendLine($"Process: {Process.GetCurrentProcess().ProcessName} ({Environment.ProcessId})");
                sb.AppendLine($"OS: {Environment.OSVersion}");
                sb.AppendLine($".NET: {Environment.Version}");
                sb.AppendLine($"AppBase: {AppDomain.CurrentDomain.BaseDirectory}");
                sb.AppendLine();
                sb.AppendLine(ex.ToString());

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                return path;
            }
            catch
            {
                return "(ne mogu zapisati crash log)";
            }
        }
    }
}

