using System;
using System.Diagnostics;
using System.IO;

namespace SignToolGUI.Class
{
    internal sealed class SignerThumbprint
    {
        public string SignToolExe { get; set; }
        public string TimeStampServer { get; set; }
        public string ThumbprintFromCertToSign { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }

        public delegate void StatusReport(string message);
        public event StatusReport OnSignToolOutput;

        public SignerThumbprint(string executable, string thumbprint = null, string timestampServer = null)
        {
            SignToolExe = executable;
            TimeStampServer = timestampServer;
            ThumbprintFromCertToSign = thumbprint;
        }

        private bool VerifyFileExists()
        {
            return File.Exists(SignToolExe);
        }

        public void Sign(string targetAssembly)
        {
            SignAsync(targetAssembly);
        }

        private void SignAsync(object targetAssembly)
        {
            if (!VerifyFileExists())
            {
                OnSignToolOutput?.Invoke(@"SignTool.exe can't be found!");
                return;
            }
            // Check if the timestamp server URL is set
            if (string.IsNullOrEmpty(TimeStampServer))
            {
                OnSignToolOutput?.Invoke("Timestamp server URL is not set!");
                return;
            }

            // Parse data needed to sign the target assembly
            var processSha256 = new Process
            {
                StartInfo = new ProcessStartInfo(SignToolExe)
                {
                    Arguments = $@"sign {GlobalOptionSwitches()} /tr ""{TimeStampServer}"" /td sha256 /fd sha256 /sha1 ""{ThumbprintFromCertToSign}"" ""{targetAssembly}""",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            processSha256.OutputDataReceived += (sender, e) =>
            {
                OnSignToolOutput?.Invoke(e.Data);
            };
            processSha256.ErrorDataReceived += (sender, e) =>
            {
                OnSignToolOutput?.Invoke(e.Data);
            };
            processSha256.Exited += (sender, e) =>
            {
                OnSignToolOutput?.Invoke("Exited: " + e);
            };

            // Start the process and begin reading the output and error streams
            try
            {
                processSha256.Start();
                processSha256.BeginOutputReadLine();
                processSha256.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                OnSignToolOutput?.Invoke(ex.Message);
            }
        }

        private string GlobalOptionSwitches()
        {
            switch (Verbose)
            {
                case true when Debug:
                    return "/v /debug";
                case true:
                    return "/v";
                default:
                    return Debug ? "/debug" : string.Empty;
            }
        }
    }
}