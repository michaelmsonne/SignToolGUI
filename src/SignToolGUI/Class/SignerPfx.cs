using System;
using System.Diagnostics;
using System.IO;

namespace SignToolGUI.Class
{
    internal sealed class SignerPfx
    {
        public string SignToolExe { get; set; }
        public string TimeStampServer { get; set; }
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool Timestamp { get; set; }

        public delegate void StatusReport(string message);
        public event StatusReport OnSignToolOutput;

        public SignerPfx(string executable, string certPath, string certPasswrd = null, string timestampServer = null)
        {
            SignToolExe = executable;
            TimeStampServer = timestampServer;
            CertificatePath = certPath;
            CertificatePassword = certPasswrd;
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
            // Check if the certificate path is set
            if (string.IsNullOrEmpty(CertificatePath))
            {
                OnSignToolOutput?.Invoke("Certificate path is not set!");
                return;
            }
            // Check if the certificate password is set
            if (string.IsNullOrEmpty(CertificatePassword))
            {
                OnSignToolOutput?.Invoke("Certificate password is not set!");
                return;
            }

            // Construct the arguments for the signing process
            var arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /f ""{CertificatePath}"" /p ""{CertificatePassword}"" /a ""{targetAssembly}""";

            if (Timestamp)
            {
                // Include timestamp server argument if Timestamp is true
                arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /tr ""{TimeStampServer}"" /td sha256 /f ""{CertificatePath}"" /p ""{CertificatePassword}"" /a ""{targetAssembly}""";
            }

            // Parse data needed to sign the target assembly
            var processSha256 = new Process
            {
                StartInfo = new ProcessStartInfo(SignToolExe)
                {
                    Arguments = arguments,
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