using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using SignToolGUI.Class;

namespace SignToolGUI.Forms
{
    partial class AboutForm : Form
    {
        public async void InitializeAsyncCertificateCheck()
        {
            // TODO MOVE TO CLASS
            // Check the result and update UI accordingly based on the certificate thumbprint fetched from GitHub or the hardcoded one (if offline)

            // Get the path of the current executable
            string filePath = Assembly.GetExecutingAssembly().Location;

            // Initialize WINTRUST_FILE_INFO
            WINTRUST_FILE_INFO fileInfo = new WINTRUST_FILE_INFO
            {
                cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO)),
                pcwszFilePath = Marshal.StringToCoTaskMemUni(filePath)
            };

            // Initialize WINTRUST_DATA
            WINTRUST_DATA winTrustData = new WINTRUST_DATA
            {
                cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA)),
                dwUIChoice = 2, // WTD_UI_NONE
                fdwRevocationChecks = 0, // WTD_REVOKE_NONE
                dwUnionChoice = 1, // WTD_CHOICE_FILE
                dwStateAction = 0, // WTD_STATEACTION_IGNORE
                dwProvFlags = 0x00000080, // WTD_CACHE_ONLY_URL_RETRIEVAL
                pFile = Marshal.AllocCoTaskMem(Marshal.SizeOf(fileInfo))
            };

            // Copy fileInfo into the memory allocated for pFile
            Marshal.StructureToPtr(fileInfo, winTrustData.pFile, false);

            // GUID of the action to verify the file signature
            Guid action = new Guid("00aac56b-cd44-11d0-8cc2-00c04fc295ee");

            // Allocate memory for WINTRUST_DATA and copy the structure
            IntPtr pWinTrustData = Marshal.AllocCoTaskMem(Marshal.SizeOf(winTrustData));
            Marshal.StructureToPtr(winTrustData, pWinTrustData, false);

            // Call WinVerifyTrust
            int result = NativeMethods.WinVerifyTrust(IntPtr.Zero, action, pWinTrustData);

            // Check the result
            if (result == 0)
            {
                try
                {
                    X509Certificate2 certificate = new X509Certificate2(X509Certificate.CreateFromSignedFile(filePath));
                    string currentThumbprint = await Globals.FetchCurrentCertificateThumbprintAsync();
                    
                    // Check if the thumbprint matches the current one from Michael Morten Sonne at GitHub or the hardcoded one (if offline)
                    if (certificate.Thumbprint != null && certificate.Thumbprint.Equals(currentThumbprint, StringComparison.OrdinalIgnoreCase))
                    {
                        labelSignedBuildState.Invoke((MethodInvoker)delegate
                        {
                            labelSignedBuildState.Text = Globals.ToolStates.CodeSignedBuildMichael;
                            labelSignedBuildState.ForeColor = Color.Green;
                        });
                    }
                    else
                    {
                        labelSignedBuildState.Invoke((MethodInvoker)delegate
                        {
                            labelSignedBuildState.Text = Globals.ToolStates.CodeSignedBuild;
                            labelSignedBuildState.ForeColor = Color.Green;
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"An error occurred: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Check if the handle for labelSignedBuildState has been created
                if (labelSignedBuildState.IsHandleCreated)
                {
                    labelSignedBuildState.Invoke((MethodInvoker)delegate
                    {
                        labelSignedBuildState.Text = Globals.ToolStates.NotCodeSignedBuild;
                        labelSignedBuildState.ForeColor = Color.Red;
                    });
                }
                else
                {
                    // Handle the case where the control's handle is not yet created
                    // One approach is to use the Load event of the form to ensure the code runs after the form is fully loaded
                    Load += (sender, e) =>
                    {
                        labelSignedBuildState.Text = Globals.ToolStates.NotCodeSignedBuild;
                        labelSignedBuildState.ForeColor = Color.Red;
                    };
                }
            }

            // Free allocated memory
            Marshal.FreeCoTaskMem(fileInfo.pcwszFilePath);
            Marshal.FreeCoTaskMem(winTrustData.pFile);
            Marshal.FreeCoTaskMem(pWinTrustData);
        }

        public AboutForm()
        {
            InitializeComponent();
            Text = String.Format(@"About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format(@"Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright + @" " + DateTime.Now.Year;
            labelCompanyName.Text = AssemblyCompany;
            //textBoxDescription.Text = AssemblyDescription;

            // Create an instance of the ToolTip class to provide tooltip information for the pictureBoxBuyMeACoffee control
            ToolTip toolTipForPictureBox = new ToolTip();
            toolTipForPictureBox.AutoPopDelay = 5000;  // Tooltip stays open for 5 seconds
            toolTipForPictureBox.InitialDelay = 1000;  // Tooltip appears after 1 second
            toolTipForPictureBox.ReshowDelay = 500;    // Tooltip reappears after half a second when the cursor moves from one control to another
            toolTipForPictureBox.ShowAlways = true;    // Tooltip will show even if the form is not active

            // Set the tooltip text for pictureBoxBuyMeACoffee
            toolTipForPictureBox.SetToolTip(pictureBoxBuyMeACoffee, "Support me on Buy Me a Coffee!");

            // Initialize the certificate check asynchronously and update GUI accordingly
            InitializeAsyncCertificateCheck();
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void linkLabelBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://blog.sonnes.cloud");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"SignTool GUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/michaelmsonne");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"SignTool GUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBoxBuyMeACoffee_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://buymeacoffee.com/sonnes");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"SignTool GUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabelLinkedIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://www.linkedin.com/in/michaelmsonne/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"SignTool GUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
