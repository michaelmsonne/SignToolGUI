using System;

namespace SignToolGUI.Class
{
    internal sealed class SignerPfx : SignerBase
    {
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }

        public SignerPfx(string executable, string certPath, string certPasswrd = null, TimestampManager timestampManager = null)
            : base(executable, timestampManager)
        {
            CertificatePath = certPath;
            CertificatePassword = certPasswrd;
        }

        protected override string BuildSigningArguments(string targetAssembly, string timestampUrl = null)
        {
            if (string.IsNullOrEmpty(CertificatePath))
            {
                throw new InvalidOperationException("Certificate path is not set!");
            }

            if (string.IsNullOrEmpty(CertificatePassword))
            {
                throw new InvalidOperationException("Certificate password is not set!");
            }

            var arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /f ""{CertificatePath}"" /p ""{CertificatePassword}"" /a ""{targetAssembly}""";

            if (Timestamp && !string.IsNullOrEmpty(timestampUrl))
            {
                arguments = $@"sign {GlobalOptionSwitches()} /fd sha256 /tr ""{timestampUrl}"" /td sha256 /f ""{CertificatePath}"" /p ""{CertificatePassword}"" /a ""{targetAssembly}""";
            }

            return arguments;
        }
    }
}