using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static SignToolGUI.Class.FileLogger;

namespace SignToolGUI.Class
{
    public class CertificateMonitor
    {
        private readonly int _warningThresholdDays;
        private readonly int _criticalThresholdDays;

        public enum AlertLevel
        {
            None,
            Warning,    // 30-90 days
            Critical,   // 0-30 days
            Expired     // Already expired
        }

        public class CertificateAlert
        {
            public X509Certificate2 Certificate { get; set; }
            public AlertLevel Level { get; set; }
            public int DaysUntilExpiry { get; set; }
            public string Message { get; set; }
            public string CertificateName { get; set; }
        }        

        public CertificateMonitor(int warningThresholdDays = 90, int criticalThresholdDays = 30)
        {
            _warningThresholdDays = warningThresholdDays;
            _criticalThresholdDays = criticalThresholdDays;
        }

        public List<CertificateAlert> CheckCertificateExpiry(X509Certificate2Collection certificates)
        {
            var alerts = new List<CertificateAlert>();
            var currentDate = DateTime.Now;

            foreach (X509Certificate2 cert in certificates)
            {
                if (cert == null) continue;

                var daysUntilExpiry = (cert.NotAfter - currentDate).Days;
                var alertLevel = GetAlertLevel(daysUntilExpiry);

                if (alertLevel != AlertLevel.None)
                {
                    var alert = new CertificateAlert
                    {
                        Certificate = cert,
                        Level = alertLevel,
                        DaysUntilExpiry = daysUntilExpiry,
                        CertificateName = cert.GetNameInfo(X509NameType.SimpleName, false) ?? "Unknown Certificate",
                        Message = GenerateAlertMessage(cert, daysUntilExpiry, alertLevel)
                    };

                    alerts.Add(alert);

                    // Log the certificate expiry alert
                    var logLevel = alertLevel == AlertLevel.Expired || alertLevel == AlertLevel.Critical
                        ? EventType.Warning
                        : EventType.Information;

                    Message($"Certificate expiry alert: {alert.Message}", logLevel, 2000 + (int)alertLevel);
                }
            }

            return alerts.OrderBy(a => a.DaysUntilExpiry).ToList();
        }

        public CertificateAlert CheckSingleCertificate(X509Certificate2 certificate)
        {
            if (certificate == null) return null;

            var currentDate = DateTime.Now;
            var daysUntilExpiry = (certificate.NotAfter - currentDate).Days;
            var alertLevel = GetAlertLevel(daysUntilExpiry);

            if (alertLevel == AlertLevel.None) return null;

            return new CertificateAlert
            {
                Certificate = certificate,
                Level = alertLevel,
                DaysUntilExpiry = daysUntilExpiry,
                CertificateName = certificate.GetNameInfo(X509NameType.SimpleName, false) ?? "Unknown Certificate",
                Message = GenerateAlertMessage(certificate, daysUntilExpiry, alertLevel)
            };
        }

        private AlertLevel GetAlertLevel(int daysUntilExpiry)
        {
            if (daysUntilExpiry < 0)
                return AlertLevel.Expired;
            else if (daysUntilExpiry <= _criticalThresholdDays)
                return AlertLevel.Critical;
            else if (daysUntilExpiry <= _warningThresholdDays)
                return AlertLevel.Warning;
            else
                return AlertLevel.None;
        }

        private string GenerateAlertMessage(X509Certificate2 cert, int daysUntilExpiry, AlertLevel level)
        {
            var certName = cert.GetNameInfo(X509NameType.SimpleName, false) ?? "Unknown Certificate";
            var expiryDate = cert.NotAfter.ToShortDateString();

            switch (level)
            {
                case AlertLevel.Expired:
                    return $"Certificate '{certName}' has EXPIRED {Math.Abs(daysUntilExpiry)} days ago (expired on {expiryDate})";

                case AlertLevel.Critical:
                    return $"Certificate '{certName}' expires in {daysUntilExpiry} days (on {expiryDate}) - CRITICAL";

                case AlertLevel.Warning:
                    return $"Certificate '{certName}' expires in {daysUntilExpiry} days (on {expiryDate}) - Warning";

                default:
                    return string.Empty;
            }
        }

        public void ShowExpiryAlerts(List<CertificateAlert> alerts, IWin32Window owner = null)
        {
            if (!alerts.Any()) return;

            var expiredCerts = alerts.Where(a => a.Level == AlertLevel.Expired).ToList();
            var criticalCerts = alerts.Where(a => a.Level == AlertLevel.Critical).ToList();
            var warningCerts = alerts.Where(a => a.Level == AlertLevel.Warning).ToList();

            var message = "Certificate Expiry Alerts:\n\n";

            if (expiredCerts.Any())
            {
                message += "🔴 EXPIRED CERTIFICATES:\n";
                foreach (var cert in expiredCerts)
                {
                    message += $"• {cert.Message}\n";
                }
                message += "\n";
            }

            if (criticalCerts.Any())
            {
                message += "🟠 CRITICAL (expires within 30 days):\n";
                foreach (var cert in criticalCerts)
                {
                    message += $"• {cert.Message}\n";
                }
                message += "\n";
            }

            if (warningCerts.Any())
            {
                message += "🟡 WARNING (expires within 90 days):\n";
                foreach (var cert in warningCerts)
                {
                    message += $"• {cert.Message}\n";
                }
            }

            message += "\n⚠️ Please renew or replace expiring certificates before they become unusable for code signing.";

            var icon = expiredCerts.Any() || criticalCerts.Any()
                ? MessageBoxIcon.Warning
                : MessageBoxIcon.Information;

            MessageBox.Show(owner, message, "Certificate Expiry Alerts", MessageBoxButtons.OK, icon);
        }
    }
}