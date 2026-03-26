using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Project
{
    internal static class AppPaths
    {
        internal static string[] CandidateRoots()
        {
            string startup = Application.StartupPath;
            string[] roots =
            {
                startup,
                Path.GetFullPath(Path.Combine(startup, "..")),
                Path.GetFullPath(Path.Combine(startup, "..", "..")),
                Path.GetFullPath(Path.Combine(startup, "..", "..", "..")),
                Path.GetFullPath(Path.Combine(startup, "..", "..", "..", ".."))
            };

            return roots
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        internal static string FindAppRoot()
        {
            foreach (var root in CandidateRoots().Reverse())
            {
                if (File.Exists(Path.Combine(root, "PythonRuntime", "python.exe")))
                    return root;

                if (Directory.Exists(Path.Combine(root, "PythonRuntime")))
                    return root;

                if (Directory.Exists(Path.Combine(root, "python")))
                    return root;
            }

            return Application.StartupPath;
        }

        internal static string FindPythonExe()
        {
            foreach (var root in CandidateRoots().Reverse())
            {
                string embedded = Path.Combine(root, "PythonRuntime", "python.exe");
                if (File.Exists(embedded))
                    return embedded;

                string alt = Path.Combine(root, "python", "python.exe");
                if (File.Exists(alt))
                    return alt;
            }

            return "python.exe";
        }

        internal static string ResolveFile(params string[] relativeParts)
        {
            foreach (var root in CandidateRoots().Reverse())
            {
                string candidate = Path.Combine(root, Path.Combine(relativeParts));
                if (File.Exists(candidate))
                    return candidate;
            }

            return string.Empty;
        }

        internal static string ResolveDirectory(params string[] relativeParts)
        {
            foreach (var root in CandidateRoots().Reverse())
            {
                string candidate = Path.Combine(root, Path.Combine(relativeParts));
                if (Directory.Exists(candidate))
                    return candidate;
            }

            return string.Empty;
        }

        internal static string GetWritableWorkDirectory(params string[] relativeParts)
        {
            string[] allParts = new[] { Path.GetTempPath(), "WindowsFormsApp" }
                .Concat(relativeParts ?? Array.Empty<string>())
                .ToArray();

            string path = Path.Combine(allParts);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
