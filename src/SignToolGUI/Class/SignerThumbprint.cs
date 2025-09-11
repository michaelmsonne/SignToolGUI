using System;

namespace SignToolGUI.Class
{
    internal sealed class SignerThumbprint : SignerBase
    {
        public string ThumbprintFromCertToSign { get; set; }

        public SignerThumbprint(string executable, string thumbprint = null, TimestampManager timestampManager = null)
            : base(executable, timestampManager)
        {
            ThumbprintFromCertToSign = thumbprint;
        }

        protected override string BuildSigningArguments(string targetAssembly, string timestampUrl = null)
        {
            var arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /sha1 ""{ThumbprintFromCertToSign}"" ""{targetAssembly}""";

            if (Timestamp && !string.IsNullOrEmpty(timestampUrl))
            {
                arguments = $@"sign {GlobalOptionSwitches()} /tr ""{timestampUrl}"" /td sha256 /fd sha256 /sha1 ""{ThumbprintFromCertToSign}"" ""{targetAssembly}""";
            }

            return arguments;
        }
    }
}