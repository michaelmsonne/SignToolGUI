using SignToolGUI.Class;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SignToolGUI.Forms
{
    public partial class CertificateStatusForm : Form
    {
        public CertificateStatusForm()
        {
            InitializeComponent();
        }

        public void LoadCertificateStatus(System.Security.Cryptography.X509Certificates.X509Certificate2Collection certificates)
        {
            listViewCertificates.Items.Clear();

            var monitor = new CertificateMonitor();
            var alerts = monitor.CheckCertificateExpiry(certificates);

            // Add certificates with alerts
            foreach (var alert in alerts)
            {
                var item = new ListViewItem();

                // Status column with color coding
                switch (alert.Level)
                {
                    case CertificateMonitor.AlertLevel.Expired:
                        item.Text = "EXPIRED";
                        item.BackColor = Color.LightCoral;
                        item.ForeColor = Color.DarkRed;
                        break;
                    case CertificateMonitor.AlertLevel.Critical:
                        item.Text = "CRITICAL";
                        item.BackColor = Color.Orange;
                        item.ForeColor = Color.DarkRed;
                        break;
                    case CertificateMonitor.AlertLevel.Warning:
                        item.Text = "WARNING";
                        item.BackColor = Color.LightYellow;
                        item.ForeColor = Color.DarkOrange;
                        break;
                    default:
                        item.Text = "OK";
                        item.BackColor = Color.LightGreen;
                        break;
                }

                item.SubItems.Add(alert.CertificateName);
                item.SubItems.Add(alert.Certificate.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true));
                item.SubItems.Add(alert.Certificate.NotAfter.ToShortDateString());
                item.SubItems.Add(alert.DaysUntilExpiry.ToString());
                item.SubItems.Add(alert.Certificate.Thumbprint);
                item.Tag = alert.Certificate;

                listViewCertificates.Items.Add(item);
            }

            // Add valid certificates (not expiring soon)
            foreach (System.Security.Cryptography.X509Certificates.X509Certificate2 cert in certificates)
            {
                if (alerts.Any(a => a.Certificate.Thumbprint == cert.Thumbprint)) continue;

                var currentDate = DateTime.Now;
                var daysUntilExpiry = (cert.NotAfter - currentDate).Days;

                var item = new ListViewItem("OK");
                item.BackColor = Color.LightGreen;
                item.ForeColor = Color.DarkGreen;
                item.SubItems.Add(cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false) ?? "Unknown");
                item.SubItems.Add(cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true));
                item.SubItems.Add(cert.NotAfter.ToShortDateString());
                item.SubItems.Add(daysUntilExpiry.ToString());
                item.SubItems.Add(cert.Thumbprint);
                item.Tag = cert;

                listViewCertificates.Items.Add(item);
            }

            // Auto-resize columns
            foreach (ColumnHeader column in listViewCertificates.Columns)
            {
                column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            // This will be called from the main form to refresh the data
            if (Owner is MainForm mainForm)
            {
                LoadCertificateStatus(mainForm.GetCurrentCertificates());
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
