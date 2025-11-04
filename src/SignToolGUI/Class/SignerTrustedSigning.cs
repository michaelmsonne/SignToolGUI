using System;
using System.IO;
using System.Text.Json;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    internal sealed class SignerTrustedSigning : SignerBase
    {
        public string DlibPath { get; set; }
        public string DmdfPath { get; set; }

        private readonly string _timestampServer; // Always "http://timestamp.acs.microsoft.com"
        private readonly string _codeSigningAccountName;
        private readonly string _certificateProfileName;
        private readonly string _correlationIdData;
        private readonly string _endpointServer; // Regional Azure endpoint for signing

        public SignerTrustedSigning(string executable, string timestampServer, string dlibPath, string codeSigningAccountName, string certificateProfileName, string correlationIdData, string endpointServer, TimestampManager timestampManager = null)
            : base(executable, timestampManager)
        {
            _timestampServer = timestampServer; // Should always be "http://timestamp.acs.microsoft.com"
            DlibPath = dlibPath;
            _codeSigningAccountName = codeSigningAccountName;
            _certificateProfileName = certificateProfileName;
            _correlationIdData = correlationIdData;
            _endpointServer = endpointServer; // Regional endpoint from TimestampManager
            DmdfPath = CreateTempJsonFile();
        }

        // Destructor to clean up the temporary JSON file
        ~SignerTrustedSigning()
        {
            try
            {
                if (!string.IsNullOrEmpty(DmdfPath) && File.Exists(DmdfPath))
                {
                    File.Delete(DmdfPath);
                }
            }
            catch (Exception)
            {
                // Ignore exceptions during cleanup
            }
        }

        private string CreateTempJsonFile()
        {
            // Create a JSON file with the required parameters
            var jsonContent = new
            {
                Endpoint = _endpointServer, // Use the regional endpoint here
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

        protected override string BuildSigningArguments(string targetAssembly, string timestampUrl = null)
        {
            // Check if the Dlib path is set
            if (string.IsNullOrEmpty(DlibPath))
            {
                throw new InvalidOperationException("Dlib path is not set!");
            }

            // Check if the Dlib file exists
            if (!File.Exists(DlibPath))
            {
                throw new InvalidOperationException($"Dlib file not found at: {DlibPath}");
            }

            // Check if the Dmdf path is set
            if (string.IsNullOrEmpty(DmdfPath))
            {
                throw new InvalidOperationException("Dmdf path is not set!");
            }

            // Check if the Dmdf file exists
            if (!File.Exists(DmdfPath))
            {
                throw new InvalidOperationException($"Dmdf file not found at: {DmdfPath}");
            }

            // Resolve absolute paths for logging
            string dlibFullPath = Path.GetFullPath(DlibPath);
            string dmdfFullPath = Path.GetFullPath(DmdfPath);
            string cwd = Directory.GetCurrentDirectory();

            // For Trusted Signing, always use the fixed timestamp server
            // The timestampUrl parameter is ignored because Trusted Signing uses a fixed timestamp URL
            var arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /tr ""{_timestampServer}"" /td sha256 /dlib ""{dlibFullPath}"" /dmdf ""{dmdfFullPath}"" ""{targetAssembly}""";

            // Keep exact call for traceability
            Message($"Calling Trusted Signing via arguments: '{arguments}'", EventType.Information, 3032);

            // Log resolved locations for clarity
            Message($"Resolved DLIB location: '{dlibFullPath}'", EventType.Information, 3033);
            Message($"Resolved DMDF location: '{dmdfFullPath}' | Working directory: '{cwd}'", EventType.Information, 3033);

            return arguments;
        }

        // Override the base method to handle endpoint switching for Trusted Signing
        public void UpdateEndpoint(string newEndpoint)
        {
            // Recreate the JSON file with the new endpoint
            var jsonContent = new
            {
                Endpoint = newEndpoint,
                CodeSigningAccountName = _codeSigningAccountName,
                CertificateProfileName = _certificateProfileName,
                CorrelationIdData = _correlationIdData
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Delete old file if it exists
            if (!string.IsNullOrEmpty(DmdfPath) && File.Exists(DmdfPath))
            {
                File.Delete(DmdfPath);
            }

            // Create new JSON file with updated endpoint
            string tempFilePath = Path.GetTempFileName();
            string jsonFilePath = Path.ChangeExtension(tempFilePath, ".json");
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(jsonContent, options));

            DmdfPath = jsonFilePath;
        }
    }
}