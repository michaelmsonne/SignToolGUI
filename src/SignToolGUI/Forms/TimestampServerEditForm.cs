using SignToolGUI.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignToolGUI.Forms
{
    public partial class TimestampServerEditForm : Form
    {
        private TimestampServer _originalServer;

        public TimestampServerEditForm(TimestampServer server = null)
        {
            _originalServer = server;
            InitializeComponent();

            if (server != null)
            {
                LoadServerData(server);
                this.Text = "Edit Timestamp Server";
            }
            else
            {
                this.Text = "Add Timestamp Server";
            }
        }

        private void LoadServerData(TimestampServer server)
        {
            textBoxDisplayName.Text = server.DisplayName;
            textBoxUrl.Text = server.Url;
            checkBoxEnabled.Checked = server.IsEnabled;
            numericTimeout.Value = server.TimeoutSeconds;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDisplayName.Text))
            {
                MessageBox.Show("Please enter a display name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxUrl.Text))
            {
                MessageBox.Show("Please enter a URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var uri = new Uri(textBoxUrl.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        public TimestampServer GetTimestampServer()
        {
            return new TimestampServer(
                textBoxDisplayName.Text.Trim(),
                textBoxUrl.Text.Trim(),
                checkBoxEnabled.Checked,
                _originalServer?.Priority ?? 0,
                (int)numericTimeout.Value
            );
        }
    }
}
