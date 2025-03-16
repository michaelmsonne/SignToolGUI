using System;
using System.Windows.Forms;

namespace SignToolGUI.Forms
{
    public partial class ChangelogForm : Form
    {
        public ChangelogForm()
        {
            InitializeComponent();
        }

        private void ChangelogForm_Load(object sender, EventArgs e)
        {
            PopulateChangelog();
        }

        // Populate the changelog in the RichTextBox
        private void PopulateChangelog()
        {
            // Changelog content
            var changelogContent = " Version 1.4.0.0 (15-03-2025):\n" +
                                   " - Updated Trusted Signing from v0.1.103.0 to the latest v0.1.108.0\n" +
                                   " - The tool now only displays Code Signing certificates with a private key for selection\n" +
                                   " - Added a direct link to the Azure Portal to help you find your Trusted Signing accounts\n" +
                                   " - New option to enable or disable timestamping when signing (supported for .pfx and\n" +
                                   "   Certificate Store certificates)\n" +
                                   " - Improved error handling and logging\n" +
                                   " - Added support for more versions of the Windows SDK\n" +
                                   " - New \"Select All\" option for bulk selecting/unselecting files to sign\n" +
                                   " - Minor UI improvements for a better user experience\n\n" +
                                   " Version 1.3.0.0 (18-07-2024):\n" +
                                   " - Add support for Microsoft Trusted Signing\n" +
                                   " - Add check for if tool is code signed (via Windows API, valid or valid with my Code Signing\n" +
                                   "   Certificate via Thumbprint hosted on GitHub)\n" +
                                   " - Add multiple timestamp servers" +
                                   " - Add save to logfile\n" +
                                   " - Bug fixes\n" + 
                                   "   > Like Certificate Store certs will reset on every sign\n\n" +
                                   " Version 1.2.2.0 (04-07-2024):\n" +
                                   " - Add code to DPI aware and SignTool via API\n" +
                                   " - Add more status messages to statusstrip for file operations\n" +
                                   " - Performance tweaks\n" +
                                   " - Change arch for default signtool.exe\n" +
                                   " - GUI changes\n" +
                                   " - Bug fixes\n\n" +
                                   " Version 1.2.1.0 (09-08-2023):\n" +
                                   " - Major release\n" +
                                   " - Added feature to find if signtool.exe is installed on the computer\n" +
                                   " - UI updates\n" +
                                   " - Add new feature for reset interface\n" +
                                   " - Add new feature for counting files\n" +
                                   " - Bug fixes like certificate information not showing up if saved cert at startup\n" +
                                   " - Minor changes\n" +
                                   " - Update shipped signtool.exe to last v.\n" +
                                   " - Updated to.net 4.8\n\n" +
                                   " Version 1.2.0.0 (30-06-2022):\n" +
                                   " - Feature additions\n" +
                                   " - Addressed issues\n\n" +
                                   " Version 1.0.4.0 (31-05-2021):\n" +
                                   " - Significant changes of logic and signing\n" +
                                   " - Overhauled GUI\n\n" +
                                   " Version 1.0.3.0 (30-04-2021):\n" +
                                   " - Fixed some bugs when signing multiple files at once from a folder\n" +
                                   " - Fixed issue for ECC SHA512 bug\n" +
                                   " - Performance enhancements\n\n" +
                                   " Version 1.0.2.0 (31-03-2021):\n" +
                                   " - More features added\n" +
                                   " - Several fixes in GUI text\n" +
                                   " - Performance tweaks\n" +
                                   " - UI enhancements\n\n" +
                                   " Version 1.0.1.0 (21-01-2021):\n" +
                                   " - Initial updates\n" +
                                   " - Bug fixes\n\n" +
                                   " Version 1.0.0.0 (11-01-2021):\n" +
                                   " - First release";

            // Set the content in the RichTextBox control
            richTextBoxChangelog.Text = changelogContent;
        }
    }
}