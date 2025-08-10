using System.Diagnostics;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    public class SigningValidator
    {
        public static string GetSigningTimestampFromFile(string filePath, string signToolExe)
        {
            var psi = new ProcessStartInfo
            {
                FileName = signToolExe,
                Arguments = $"verify /v /all \"{filePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Try to match several possible timestamp lines
                var match1 = System.Text.RegularExpressions.Regex.Match(output, @"Timestamp:\s*(.+)");
                if (match1.Success)
                    return match1.Groups[1].Value.Trim();

                var match2 = System.Text.RegularExpressions.Regex.Match(output, @"Signed Time:\s*(.+)");
                if (match2.Success)
                    return match2.Groups[1].Value.Trim();

                var match3 = System.Text.RegularExpressions.Regex.Match(output, @"The signature is timestamped:\s*(.+)");
                if (match3.Success)
                    return match3.Groups[1].Value.Trim();

                // Optionally, log the output for debugging
                Message("SignTool verify output for timestamp: " + output, EventType.Error, 9999);

                return "N/A";
            }
        }
    }
}
