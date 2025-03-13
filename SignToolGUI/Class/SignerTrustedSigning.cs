using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SignToolGUI.Class
{
    internal sealed class SignerTrustedSigning
    {
        public string SignToolExe { get; set; }
        public string DlibPath { get; set; }
        public string DmdfPath { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool Timestamp { get; set; }

        public delegate void StatusReport(string message);
        public event StatusReport OnSignToolOutput;

        private readonly string _timestampServer;
        private readonly string _codeSigningAccountName;
        private readonly string _certificateProfileName;
        private readonly string _correlationIdData;
        private readonly string _endpointServer;

        private bool VerifyFileExists()
        {
            return File.Exists(SignToolExe);
        }

        public SignerTrustedSigning(string executable, string timestampServer, string dlibPath, string codeSigningAccountName, string certificateProfileName, string correlationIdData, string endpointServer)
        {
            SignToolExe = executable;
            _timestampServer = timestampServer;
            DlibPath = dlibPath;
            _codeSigningAccountName = codeSigningAccountName;
            _certificateProfileName = certificateProfileName;
            _correlationIdData = correlationIdData;
            _endpointServer = endpointServer;
            DmdfPath = CreateTempJsonFile();
        }

        private string CreateTempJsonFile()
        {
            // Create a JSON file with the required parameters
            var jsonContent = new
            {
                Endpoint = _endpointServer,
                CodeSigningAccountName = _codeSigningAccountName,
                CertificateProfileName = _certificateProfileName,
                CorrelationIdData = _correlationIdData
                // You can add "CorrelationId" here if needed
            };

            // Serialize the JSON content
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // This will format the JSON with indentation and new lines
            };

            // Create a temporary file with the JSON content
            string tempFilePath = Path.GetTempFileName();
            string jsonFilePath = Path.ChangeExtension(tempFilePath, ".json");

            // Write the JSON content to the file
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jsonContent, options));

            // Return the path to the JSON file
            return jsonFilePath;
        }

        public void Sign(string targetAssembly)
        {
            // Sign the target assembly asynchronously
            SignAsync(targetAssembly);
        }
        
        private void SignAsync(object targetAssembly)
        {
            // Check if the SignTool.exe exists
            if (!VerifyFileExists())
            {
                OnSignToolOutput?.Invoke(@"SignTool.exe can't be found!");
                return;
            }
            // Check if the timestamp server URL is set
            if (string.IsNullOrEmpty(_timestampServer))
            {
                OnSignToolOutput?.Invoke("Timestamp server URL is not set!");
                return;
            }
            // Check if the Dlib path is set
            if (string.IsNullOrEmpty(DlibPath))
            {
                OnSignToolOutput?.Invoke("Dlib path is not set!");
                return;
            }
            // Check if the Dmdf path is set
            if (string.IsNullOrEmpty(DmdfPath))
            {
                OnSignToolOutput?.Invoke("Dmdf path is not set!");
                return;
            }
            
            // Parse data needed to sign the target assembly
            var processSha256 = new Process
            {
                StartInfo = new ProcessStartInfo(SignToolExe)
                {
                    Arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /tr ""{_timestampServer}"" /td sha256 /dlib ""{DlibPath}"" /dmdf ""{DmdfPath}"" ""{targetAssembly}""",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            // Handle the output and error data received from the process
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