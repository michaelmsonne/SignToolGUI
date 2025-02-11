using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using SignToolGUI.Class;
using Application = System.Windows.Forms.Application;

// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast

namespace SignToolGUI.Forms
{
    public partial class MainForm : Form
    {
        public static string ConfigIniPath = Files.ConfigIniPath;
        private X509Certificate2Collection _signingCerts = new X509Certificate2Collection();
        public string ThumbprintFromCertToSign;
        public new string Text;
        private string _previousSignToolPath; // Class-level variable to store the previous textBoxSignToolPath value
        private int _totalJob; //total files in job
        private int _totalSigned; //number of files signed total
        private int _jobSigned; //number of files in job signed
        public int Signerrors; //number of errors to sign
        public string SignToolExe; //path to signtool.exe
        //private bool _isSignErrorShowed;

        #region Main functions

        public MainForm()
        {
            InitializeComponent();

            // Initialize the certificate check asynchronously and update GUI accordingly
            InitializeAsyncCertificateCheck();
        }

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

        private bool FindSignToolExe()
        {
            // Check if SignToolExe path is already defined; if yes, verify its existence
            if (!string.IsNullOrEmpty(SignToolExe))
                return File.Exists(SignToolExe);

            // Define an array of potential paths where signtool.exe might exist
            // Define the base directories to search
            var baseDirectories = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            };

            // Define the versions and architectures to search
            var versions = new[]
            {
                "10.0.22621.0", // Add other versions as needed...
                "10.0.17134.0",
                "10.0.16299.0",
                "10.0.15063.0",
                "10.0.14393.0",
                "8.1",
                "8.0"
            };

            var architectures = new[]
            {
                "x64" // Add other architectures as needed
            };

            // Define the name of the executable to search for
            const string executableName = "signtool.exe";

            // List to hold the paths
            var signToolPaths = new List<string>();

            // Loop through each base directory, version, and architecture
            foreach (var baseDirectory in baseDirectories)
            {
                foreach (var version in versions)
                {
                    foreach (var architecture in architectures)
                    {
                        // Construct the potential path
                        var potentialPath = version.StartsWith("10")
                            ? Path.Combine(baseDirectory, "Windows Kits", "10", "bin", version, architecture, executableName)
                            : Path.Combine(baseDirectory, "Windows Kits", version, "bin", architecture, executableName);

                        // Check if the file exists at the potential path
                        if (File.Exists(potentialPath))
                        {
                            signToolPaths.Add(potentialPath);
                            break; // Found the executable, so exit the loop
                        }
                    }
                }
            }

            // If the executable was found, use it; otherwise, handle the situation
            if (signToolPaths.Any())
            {
                var signToolPath = signToolPaths.First(); // Get the first found path
                //Console.WriteLine($"Found signtool.exe at: {signToolPath}");
                SignToolExe = signToolPath;
            }
            else
            {
                SignToolExe = "N/A";
            }

            // Find the first existing SignToolExe path from the array
            //SignToolExe = signToolExeArray.FirstOrDefault(File.Exists);

            // Return true if SignToolExe path is not empty and exists
            return !string.IsNullOrEmpty(SignToolExe) && File.Exists(SignToolExe);
        }

        // Custom class to hold the display name and URL
        public class TimestampProvider
        {
            public string DisplayName { get; set; }
            public string Url { get; set; }

            public TimestampProvider(string displayName, string url)
            {
                DisplayName = displayName;
                Url = url;
            }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        // Method to populate the ComboBox with items
        private void PopulateComboBox()
        {
            comboBoxTimestampProviders.Items.Clear();

            if (radioButtonTrustedSigning.Checked)
            {
                // Add items for the checked state
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("West Europe", "https://weu.codesigning.azure.net"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("North Europe", "https://neu.codesigning.azure.net"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("West US 2", "https://wus2.codesigning.azure.net"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("West Central US", "https://wcus.codesigning.azure.net"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("East US", "https://eus.codesigning.azure.net"));

                groupBoxTimestamp.Text = @"Trusted Signing Endpoint";
                labelTimestampProvider.Text = @"Endpoint region:";
                txtTimestampProviderURL.Text = @"http://timestamp.acs.microsoft.com";
                labelTimeStampServer.Text = @"Endpoint URL:";
            }
            else
            {
                // Add items for the unchecked state
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("Sectigo (Comodo)", "http://timestamp.sectigo.com"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("DigiCert", "http://timestamp.digicert.com"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("GlobalSign (1)", "http://timestamp.globalsign.com/tsa/r6advanced1"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("GlobalSign (2)", "http://timestamp.globalsign.com/?signature=sha2"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("Certum", "http://time.certum.pl"));
                comboBoxTimestampProviders.Items.Add(new TimestampProvider("Custom Provider", "N/A"));

                groupBoxTimestamp.Text = @"Timestamp";
                labelTimestampProvider.Text = @"Provider:";
                txtTimestampProviderURL.Text = "";
                labelTimeStampServer.Text = @"Timestamp URL:";
            }

            // Optionally, set a default selected item
            if (comboBoxTimestampProviders.Items.Count > 0)
            {
                comboBoxTimestampProviders.SelectedIndex = 0;
            }
        }

        private void DisableForm(bool disable)
        {
            // Disable the form's controls if the disable parameter is true
            groupBoxFiles.Enabled = !disable;
            splitButtonSign.Enabled = !disable;
            menuToolStripMenuItem.Enabled = !disable;
        }

        private void ToggleDisabledForm(bool disable)
        {
            //groupBoxDetails.Enabled = !disable;
            groupBoxFiles.Enabled = !disable;
            splitButtonSign.Enabled = !disable;
        }

        private void SetInitialValues()
        {
            try
            {
                // Set the default selected index for the comboBoxCertificateStore
                comboBoxCertificateStore.SelectedIndex = 0;
            }
            catch
            {
                // ignored
            }
        }

        private void InterfaceCheck()
        {
            // Check for UI changes and apply them
            PopulateComboBox();

            try
            {
                if (radioButtonWindowsCertificateStore.Checked)
                {
                    radioButtonPFXCertificate.Checked = false;
                    radioButtonTrustedSigning.Checked = false;
                }
                else if (radioButtonPFXCertificate.Checked)
                {
                    radioButtonWindowsCertificateStore.Checked = false;
                    radioButtonTrustedSigning.Checked = false;
                }
                else if (radioButtonTrustedSigning.Checked)
                {
                    radioButtonWindowsCertificateStore.Checked = false;
                    radioButtonPFXCertificate.Checked = false;
                }

                groupBoxWindowsCertificateStore.Enabled = radioButtonWindowsCertificateStore.Checked;
                groupBoxPFXCertificate.Enabled = radioButtonPFXCertificate.Checked;
                groupBoxTrustedSigningMetadata.Enabled = radioButtonTrustedSigning.Checked;
            }
            catch
            {
                // ignored
            }
        }

        private void RadioButtonSelectCertificateLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTrustedSigning.Checked)
            {
                // Store the current textBoxSignToolPath value before changing
                _previousSignToolPath = textBoxSignToolPath.Text;

                // Update SignToolExe and textBoxSignToolPath to the local signtool.exe path
                string localSignToolPath = Path.Combine(Application.StartupPath, "Tools", "signtool.exe");

                // Set the SignToolExe to the local signtool.exe path
                SignToolExe = localSignToolPath;
                textBoxSignToolPath.Text = localSignToolPath;
            }
            else
            {
                // Restore the previous user-defined path if radioButtonTrustedSigning is unchecked
                SignToolExe = _previousSignToolPath;
                textBoxSignToolPath.Text = _previousSignToolPath;
            }

            // Check if the the rest checkboxes are enabled and set handling correctly for signing type
            try
            {
                // Perform an interface check to ensure the application is in a correct state.
                InterfaceCheck();

                // Check if the Windows Certificate Store radio button is selected.
                if (radioButtonWindowsCertificateStore.Checked)
                {
                    // Display certificates from the specified certificate store.
                    ShowCertificatesFromStore(comboBoxCertificateStore.Text);
                }
                else if (radioButtonTrustedSigning.Checked)
                {
                    labelCertificateInformation.Text = @"Trusted Signing Certificate - set details in Trusted Signing account";
                }
                else
                {
                    // Retrieve the certificate from the PFX file.
                    var certificate = GetCertificateFromPfx();

                    // If the label for certificate information is not null, update it with the certificate info.
                    if (labelCertificateInformation != null)
                    {
                        if (certificate != null)
                        {
                            // If the certificate is successfully retrieved, update the label with its info.
                            labelCertificateInformation.Text = GetCertificateInfo(certificate);
                        }
                        else
                        {
                            // If the certificate could not be retrieved, set the label to indicate this.
                            labelCertificateInformation.Text = Globals.DigitalCertificates.CertificateInfoCouldNotBeRetrieved;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ComboBoxCertificates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Check if the label for certificate information is not null.
                if (labelCertificateInformation != null)
                {
                    // Update the label with the certificate info if a certificate is selected in the combo box.
                    // If no certificate is selected, set the label to indicate that certificate info is not available.
                    labelCertificateInformation.Text = comboBoxCertificatesInStore.SelectedIndex > 0
                        ? GetCertificateInfo(_signingCerts[comboBoxCertificatesInStore.SelectedIndex - 1])
                        : Globals.DigitalCertificates.CertificateInfoIsNotAvailable;

                    // Check if the PFX Certificate radio button is selected.
                    if (radioButtonPFXCertificate.Checked)
                    {
                        // Retrieve the certificate from the PFX file.
                        var certificate = GetCertificateFromPfx();

                        // If the certificate is successfully retrieved, update the label with its info.
                        // Otherwise, indicate that the certificate information could not be retrieved.
                        if (certificate != null)
                        {
                            labelCertificateInformation.Text = GetCertificateInfo(certificate);
                        }
                        else
                        {
                            labelCertificateInformation.Text = @"Certificate information could not be retrieved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ComboBoxCertificateStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the Windows Certificate Store radio button is selected.
            try
            {
                // Get certificates from the selected store
                ShowCertificatesFromStore(comboBoxCertificateStore.Text);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void textBoxPFXFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Deactivated for now, as else getting a bug when starting up the application
                //labelCertificateInformation.Text = GetCertificateInfo(GetCertificateFromPfx());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void comboBoxTimestampProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected timestamp provider from the combo box.
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // Get the selected timestamp provider from the combo box.
            TimestampProvider selectedProvider = comboBox.SelectedItem as TimestampProvider;
            if (selectedProvider == null) return;

            // Update the timestamp provider URL text box with the selected provider's URL.
            txtTimestampProviderURL.Text = selectedProvider.Url;

            // Get the selected index from the timestamp providers combo box.
            var index = comboBoxTimestampProviders.SelectedIndex;

            // If the selected index is not 5, make the timestamp provider URL text box read-only.
            if (index != 5)
            {
                // Make the timestamp provider URL text box read-only.
                txtTimestampProviderURL.ReadOnly = true;
            }
            else
            {
                // If the selected index is 5, clear the timestamp provider URL text box and make it editable.
                txtTimestampProviderURL.Clear();
                txtTimestampProviderURL.ReadOnly = false;
            }
        }

        #endregion Main functions

        #region Form actions

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set the status label's Text property to the application's name and version
            statusLabel.Text = @"[INFO] Welcome to " + Application.ProductName + @" v." + Application.ProductVersion;

            // Perform an interface check to ensure the application is in a correct state.
            InterfaceCheck();
            
            // Set the Text property of the form to the application's name and version
            try
            {
                SetInitialValues();
            }
            catch (Exception ex)
            {
                var num = (int)MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            // Check folder for configuration file and create it if it does not exist
            if (!Directory.Exists(Files.ProgramDataFilePath))
            {
                try
                {
                    // Create the directory
                    Directory.CreateDirectory(Files.ProgramDataFilePath);
                }
                catch (Exception exception)
                {
                    // Show an error message if the directory could not be created
                    MessageBox.Show(exception.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Get data for signtool.exe and timestamp provider from configuration file
            try
            {
                // Get data from configuration file and set the values to the form's controls
                var iniFile = new IniFile(ConfigIniPath);
                var settingBoxSignToolPath = iniFile.GetString("Program", "SignToolPath", "");

                // Check if signtool.exe exists on the computer and set the path to it
                if (!FindSignToolExe())
                {
                    // Show a message box if signtool.exe was not found and use the path from the configuration file if it exists or a default path if it does not exist
                    MessageBox.Show(
                        @"Could not find SignTool.exe installed on this computer - will use information from configuration file.",
                        @"SignTool.exe not found", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                    // Check if the path to signtool.exe is set in the configuration file
                    if (settingBoxSignToolPath != "")
                    {
                        // Set the path to signtool.exe from the configuration file
                        textBoxSignToolPath.Text = settingBoxSignToolPath;
                    }
                    else
                    {
                        // Set the path to signtool.exe from the configuration file or a default path if it does not exist (default path) with the application install path
                        textBoxSignToolPath.Text = @"Tools\signtool.exe";
                    }
                }
                else
                {
                    // Set the path to signtool.exe from the configuration file or a default path if it does not exist (default path) with the application install path
                    textBoxSignToolPath.Text = SignToolExe;
                }

                // If comboBoxTimestampProviders is not 5, read from config file
                // Check if the timestamp provider is set in the configuration file
                var settingTimestampProvider = iniFile.GetString("Program", "TimestampProvider", "");
                if (!string.IsNullOrEmpty(settingTimestampProvider) && int.TryParse(settingTimestampProvider, out int selectedIndex))
                {
                    comboBoxTimestampProviders.SelectedIndex = selectedIndex;
                }

                // If set to custom provider, read from config file and set the URL
                if (comboBoxTimestampProviders.SelectedIndex == 5)
                {
                    // Check if the timestamp URL is set in the configuration file
                    var settingTimestampUrl = iniFile.GetString("Program", "TimestampURL", "");

                    // Set the timestamp URL from the configuration file if it exists
                    if (settingTimestampUrl != "")
                    {
                        txtTimestampProviderURL.Text = settingTimestampUrl;
                    }
                }

                // Check if the certificate path is set in the configuration file
                var settingCertificatePath = iniFile.GetString("Program", "CertificatePath", "");

                // Set the certificate path from the configuration file if it exists
                if (File.Exists(settingCertificatePath))
                {
                    try
                    {
                        textBoxPFXFile.Text = settingCertificatePath;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }

                // Check if the certificate password is set in the configuration file
                var settingCertificatePasswordEncrypted = iniFile.GetString("Program", "CertificatePassword", "");

                // Set the certificate password from the configuration file if it exists and decrypt it
                if (settingCertificatePasswordEncrypted != "")
                {
                    // TODO - Decrypt the certificate password based on hardware hash
                    textBoxPFXPassword.Text = StringCipher.Decrypt(settingCertificatePasswordEncrypted, "pMmInS?m24Caae#?2EySvsFUgDsUG06Qzz8R0X8F8WUNn04#g%mP02*36datrZka?cQh/Q2E/Oc4/21%");
                }
                else
                {
                    // Clear the certificate password text box if it is not set in the configuration file
                    textBoxPFXPassword.Text = "";
                }

                // Initialize _previousSignToolPath with the current text box value or a default path
                _previousSignToolPath = textBoxSignToolPath.Text;
            }
            catch (Exception ex)
            {
                // Show an error message if the configuration file could not be read
                MessageBox.Show(ex.Message, @"Data.ini", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!radioButtonPFXCertificate.Checked) return;

            // Get the certificate information from the .PFX file
            try
            {
                var certificate = GetCertificateFromPfx();
                if (certificate != null)
                {
                    if (labelCertificateInformation != null)
                        labelCertificateInformation.Text = GetCertificateInfo(certificate);
                }
                else
                {
                    // Handle the null case, for example, log an error or show a message to the user
                    if (labelCertificateInformation != null)
                        labelCertificateInformation.Text = @"Certificate information could not be retrieved.";
                }
            }
            catch (Exception exception)
            {
                if (labelCertificateInformation != null)
                    labelCertificateInformation.Text = @"An error occurred while retrieving certificate information.";
                Console.WriteLine(exception);
                throw;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the configuration to the configuration file when the form is closing
            var iniFile = new IniFile(ConfigIniPath);

            // Check if the certificate password is set in the GUI and if it is, ask the user if they want to save it to the configuration file
            if (textBoxPFXPassword.Text != "")
            {
                // Ask the user if they want to save the certificate password to the configuration file
                var msgresult =
                    MessageBox.Show(@"Do you want to save the .PFX password for the next time you use this program?",
                        @"Be careful not to store highly confidential information.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch (msgresult)
                {
                    case DialogResult.Yes:
                        // Save the certificate password to the configuration file if the user clicks Yes
                        try
                        {
                            // Save the sign tool path to the configuration file
                            iniFile.WriteValue("Program", "SignToolPath", textBoxSignToolPath.Text);

                            // Save timestamp provider to the configuration file
                            iniFile.WriteValue("Program", "TimestampProvider", comboBoxTimestampProviders.SelectedIndex);
                            
                            // Save the timestamp URL to the configuration file
                            iniFile.WriteValue("Program", "TimestampURL", txtTimestampProviderURL.Text);

                            // Save the certificate path to the configuration file if the file exists
                            if (File.Exists(textBoxPFXFile.Text))
                            {
                                try
                                {
                                    // Save the certificate path to the configuration file
                                    iniFile.WriteValue("Program", "CertificatePath", textBoxPFXFile.Text);
                                }
                                catch (Exception exception)
                                {
                                    // Show an error message if the certificate path could not be saved to the configuration file
                                    Console.WriteLine(exception);
                                    throw;
                                }
                            }

                            // Encrypt the certificate password and save it to the configuration file
                            var encryptedstring = StringCipher.Encrypt(textBoxPFXPassword.Text, "pMmInS?m24Caae#?2EySvsFUgDsUG06Qzz8R0X8F8WUNn04#g%mP02*36datrZka?cQh/Q2E/Oc4/21%");

                            // Save the encrypted certificate password to the configuration file
                            iniFile.WriteValue("Program", "CertificatePassword", encryptedstring);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Application.ExitThread();
                        break;
                    case DialogResult.No:
                        // Do not save the certificate password to the configuration file if the user clicks No
                        iniFile.WriteValue("Program", "SignToolPath", textBoxSignToolPath.Text);
                        iniFile.WriteValue("Program", "TimestampURL", txtTimestampProviderURL.Text);

                        // Save the certificate path to the configuration file if the file exists
                        if (File.Exists(textBoxPFXFile.Text))
                        {
                            try
                            {
                                // Save the certificate path to the configuration file
                                iniFile.WriteValue("Program", "CertificatePath", textBoxPFXFile.Text);
                            }
                            catch (Exception exception)
                            {
                                // Show an error message if the certificate path could not be saved to the configuration file
                                Console.WriteLine(exception);
                                throw;
                            }

                            try
                            {
                                // Save the certificate password as null to the configuration file if the file exists
                                iniFile.WriteValue("Program", "CertificatePassword", "");
                            }
                            catch (Exception exception)
                            {
                                // Show an error message if the certificate path could not be saved to the configuration file
                                Console.WriteLine(exception);
                                throw;
                            }
                        }
                        // Close the application if the user clicks No
                        Application.ExitThread();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                // Save the configuration to the configuration file when the form is closing
                iniFile.WriteValue("Program", "SignToolPath", textBoxSignToolPath.Text);

                // Save timestamp provider to the configuration file
                iniFile.WriteValue("Program", "TimestampProvider", comboBoxTimestampProviders.SelectedIndex);


                // Save the timestamp URL to the configuration file
                iniFile.WriteValue("Program", "TimestampURL", txtTimestampProviderURL.Text);

                // Save the certificate path to the configuration file if the file exists
                if (File.Exists(textBoxPFXFile.Text))
                {
                    try
                    {
                        iniFile.WriteValue("Program", "CertificatePath", textBoxPFXFile.Text);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }

                // Save the certificate password as null to the configuration file if the file exists
                iniFile.WriteValue("Program", "CertificatePassword", "");
            }
        }

        private void buttonBrowseSignTool_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result for SignTool.exe path
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = @"Executables|*.exe";
            openFileDialog.FileName = "SignTool.exe";

            // Check if the dialog result is OK and set the text box value to the selected path
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSignToolPath.Text = openFileDialog.FileName;
            }
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            // Get the number of files currently in the list
            var filesDeletedCount = checkedListBoxFiles.Items.Count;

            // Clear the list of files to sign
            checkedListBoxFiles.Items.Clear();

            // Set the status label's Text property to a message indicating that the list of files to sign has been cleared
            statusLabel.Text = $@"[INFO] {filesDeletedCount} file(s) deleted from the list. Ready to Sign, Add a folder or file(s) to the list";
        }

        private void SplitButtonSign_Click(object sender, EventArgs e)
        {
            // API sign // TODO
            // Utilities.SignTool.SignWithCert(txtExecutableFilePath.Text, txtCertificatePath.Text, txtCertificatePassword.Text, txtTimestampProviderURL.Text);

            // Verify the configuration before signing the files from the list
            if (!VerifyConfiguration())
            {
                return; // Exit the method if verification fails
            }

            // prepares variables and environment for signing
            _totalJob = checkedListBoxFiles.CheckedItems.Count;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = _totalJob;
            toolStripProgressBar.Value = 0;

            // Disables the form's controls while signing the files
            DisableForm(true);

            // Show the output textbox the job has started
            textBoxOutput.AppendText("[JOB] Initiating Job of " + _totalJob + " Files" + Environment.NewLine);

            // Sign the files from the list selected - if 0 files are selected, show a message box and return
            if (checkedListBoxFiles.CheckedItems.Count == 0)
            {
                MessageBox.Show(@"No files selected to sign.

Please select one or more binaries into the list above to proceed!", @"No files to sign selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Question);
                DisableForm(false);
                return;
            }

            // Re-enable the form's controls after the signing operation
            DisableForm(false);

            // Sign the files from the list selected is more then 0 files - start the signing process and show the output in the textbox
            if (radioButtonWindowsCertificateStore.Checked)
            {
                try
                {
                    // Disables the form's controls while signing the files
                    ToggleDisabledForm(true);

                    // Clear the output textbox
                    textBoxOutput.Clear();

                    // Create a new instance of the SignerThumbprint class
                    SignerThumbprint signer = new SignerThumbprint(textBoxSignToolPath.Text, ThumbprintFromCertToSign, txtTimestampProviderURL.Text)
                    {
                        Verbose = menuItemSignVerbose.Checked,
                        Debug = menuItemSignDebug.Checked
                    };

                    // Add an event handler to the OnSignToolOutput event
                    signer.OnSignToolOutput += message =>
                    {
                        // Check if the message is null or empty and return if it is
                        if (string.IsNullOrEmpty(message)) return;

                        // Check if the ShowOutput checkbox is unchecked and if the message contains any of the specified strings
                        if (checkBoxShowOutput.Checked == false)
                        {
                            if (new[]
                            {
                        "Number of", "Done Adding Additional Store", "The following certificate was selected:",
                        "Signing file", "hash:", "Issued to:", "Issued by:", "Expires:",
                        "The following additional certificates will be attached:",
                        "The following certificates were considered:", "After EKU filter", "After Private Key filter,",
                        "The following additional certificates will be attached:", "After expiry filter",

                            }.Any(message.Contains))
                            {
                            }
                            else
                            {
                                // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                                if (textBoxOutput.InvokeRequired) // safe cross-thread call
                                {
                                    textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                        message + Environment.NewLine);
                                }
                                else
                                {
                                    // Append the message to the output textbox
                                    textBoxOutput.AppendText(message);
                                }
                            }
                        }
                        else
                        {
                            // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                            if (textBoxOutput.InvokeRequired) // safe cross-thread call
                            {
                                textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                    message + Environment.NewLine);
                            }
                            else
                            {
                                // Append the message to the output textbox
                                textBoxOutput.AppendText(message);
                            }
                        }
                    };

                    foreach (var file in checkedListBoxFiles.CheckedItems)
                    {
                        // Show process
                        var filename = Path.GetFileName(file.ToString());
                        textBoxOutput.AppendText($"Signing file: '{filename}'...{Environment.NewLine}");

                        // Sign file with SignerThumbprint
                        signer.Sign(file.ToString());

                        // Show process
                        textBoxOutput.AppendText("[" + (_jobSigned + 1) + "] '" + file + "'..." + Environment.NewLine);

                        // When done
                        _jobSigned += 1;
                        toolStripProgressBar.Step = 1;
                        toolStripProgressBar.PerformStep();
                        //statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" File(s)";
                    }

                    // Enable the form's controls after signing the files again and show the output in the textbox
                    ToggleDisabledForm(false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
            if (radioButtonPFXCertificate.Checked)
            {
                try
                {
                    ToggleDisabledForm(true);
                    textBoxOutput.Clear();
                    SignerPfx signer = new SignerPfx(textBoxSignToolPath.Text, textBoxPFXFile.Text, textBoxPFXPassword.Text,
                        txtTimestampProviderURL.Text)
                    {
                        Verbose = menuItemSignVerbose.Checked,
                        Debug = menuItemSignDebug.Checked
                    };

                    signer.OnSignToolOutput += message =>
                    {
                        // Check if the message is null or empty and return if it is
                        if (string.IsNullOrEmpty(message)) return;

                        // Check if the ShowOutput checkbox is unchecked and if the message contains any of the specified strings
                        if (checkBoxShowOutput.Checked == false)
                        {
                            if (new[]
                            {
                        "Number of", "Done Adding Additional Store", "The following certificate was selected:",
                        "Signing file", "hash:", "Issued to:", "Issued by:", "Expires:",
                        "The following additional certificates will be attached:",
                        "The following certificates were considered:", "After EKU filter", "After Private Key filter,",
                        "The following additional certificates will be attached:", "After expiry filter",

                            }.Any(message.Contains))
                            {
                            }
                            else
                            {
                                // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                                if (textBoxOutput.InvokeRequired) // safe cross-thread call
                                {
                                    textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                        message + Environment.NewLine);
                                }
                                else
                                {
                                    // Append the message to the output textbox
                                    textBoxOutput.AppendText(message);
                                }
                            }
                        }
                        else
                        {
                            // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                            if (textBoxOutput.InvokeRequired) // safe cross-thread call
                            {
                                textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                    message + Environment.NewLine);
                            }
                            else
                            {
                                // Append the message to the output textbox
                                textBoxOutput.AppendText(message);
                            }
                        }
                    };

                    foreach (var file in checkedListBoxFiles.CheckedItems)
                    {
                        // Show process
                        var filename = Path.GetFileName(file.ToString());
                        textBoxOutput.AppendText($"Signing file: '{filename}'...{Environment.NewLine}");

                        // Sign file with SignerPfx class
                        signer.Sign(file.ToString());

                        // Show process
                        textBoxOutput.AppendText("[" + (_jobSigned + 1) + "] " + file + "..." + Environment.NewLine);

                        // When done
                        _jobSigned += 1;
                        toolStripProgressBar.Step = 1;
                        toolStripProgressBar.PerformStep();
                        //statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" File(s)";
                    }

                    ToggleDisabledForm(false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
            if (radioButtonTrustedSigning.Checked)
            {
                try
                {
                    // Get the values from the form's controls for the SignerTrustedSigning class
                    var signToolExe = textBoxSignToolPath.Text;
                    var timeStampServer = "http://timestamp.acs.microsoft.com";
                    var endpointServer = txtTimestampProviderURL.Text;
                    var dlibPath = @".\Tools\Azure.CodeSigning.Dlib.dll";
                    var codeSigningAccountName = textBoxCodeSigningAccountName.Text;
                    var certificateProfileName = textBoxCertificateProfileName.Text;

                    // Disable the form's controls while signing the files
                    ToggleDisabledForm(true);

                    // Clear the output textbox
                    textBoxOutput.Clear();

                    // Create a new instance of the SignerTrustedSigning class
                    SignerTrustedSigning signer = new SignerTrustedSigning(signToolExe, timeStampServer, dlibPath, codeSigningAccountName, certificateProfileName, endpointServer)
                    {
                        Verbose = menuItemSignVerbose.Checked,
                        Debug = menuItemSignDebug.Checked
                    };

                    signer.OnSignToolOutput += message =>
                    {
                        // Check if the message is null or empty and return if it is
                        if (string.IsNullOrEmpty(message)) return;

                        // Check if the ShowOutput checkbox is unchecked and if the message contains any of the specified strings
                        if (checkBoxShowOutput.Checked == false)
                        {
                            if (new[]
                            {
                        "Number of", "Done Adding Additional Store", "The following certificate was selected:",
                        "Signing file", "hash:", "Issued to:", "Issued by:", "Expires:",
                        "The following additional certificates will be attached:",
                        "The following certificates were considered:", "After EKU filter", "After Private Key filter,",
                        "The following additional certificates will be attached:", "After expiry filter","Trusted Signing",
                        "Submitting digest for signing","OperationId","Version: 1.0.60","ExcludeCredentials","CertificateProfileName",
                        "CertificateProfileName","CodeSigningAccountName","Endpoint","Metadata","}","{"

                            }.Any(message.Contains))
                            {
                            }
                            else
                            {
                                // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                                if (textBoxOutput.InvokeRequired) // safe cross-thread call
                                {
                                    textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                        message + Environment.NewLine);
                                }
                                else
                                {
                                    // Append the message to the output textbox
                                    textBoxOutput.AppendText(message);
                                }
                            }
                        }
                        else
                        {
                            // Check if the output textbox's InvokeRequired property is true and if it is, append the message to the textbox
                            if (textBoxOutput.InvokeRequired) // safe cross-thread call
                            {
                                textBoxOutput.Invoke(new Action<string>(textBoxOutput.AppendText),
                                    message + Environment.NewLine);
                            }
                            else
                            {
                                // Append the message to the output textbox
                                textBoxOutput.AppendText(message);
                            }
                        }
                    };

                    // Sign the files from the list selected
                    foreach (var file in checkedListBoxFiles.CheckedItems)
                    {
                        // Show process
                        var filename = Path.GetFileName(file.ToString());
                        textBoxOutput.AppendText($"Signing file: '{filename}'...{Environment.NewLine}");

                        // Sign file with SignerTrustedSigning class
                        signer.Sign(file.ToString());

                        // Show process
                        textBoxOutput.AppendText("[" + (_jobSigned + 1) + "] " + file + "..." + Environment.NewLine);

                        // When done
                        _jobSigned += 1;
                        toolStripProgressBar.Step = 1;
                        toolStripProgressBar.PerformStep();
                        //statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" File(s)";
                    }

                    ToggleDisabledForm(false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }

            // count errors when sign files in use

            //WaitForNewText("An error occurred while attempting to sign:");

            /*#if DEBUG
                        MessageBox.Show(isSignErrorShowed.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                    if (isSignErrorShowed)
                        {
                            jobSigned = - _signerrors;
                            // runs after signing job completes
                            statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" Files";

                            MessageBox.Show(@"Job complete with errors" + Environment.NewLine + @"Signed " + jobSigned + @" of " + totalJob + @" File(s)", @"Job Report", 0, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // runs after signing job completes
                            statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" Files";

                            MessageBox.Show(@"Job complete" + Environment.NewLine + @"Signed " + jobSigned + @" of " + totalJob + @" File(s)", @"Job Report", 0, MessageBoxIcon.Information);
                        }*/

            // // runs after signing job completes //TODO
            //statusLabel.Text = @"[JOB] Signed " + jobSigned + @" of " + totalJob + @" Files";
            //
            //MessageBox.Show(@"Job complete" + Environment.NewLine + @"Signed " + jobSigned + @" of " + totalJob + @" File(s)", @"Job Report", 0, MessageBoxIcon.Information);

            // cleans up environment after job
            toolStripProgressBar.Value = 0;
            _totalSigned += _jobSigned;
            _jobSigned = 0;

            //statusLabel.Text = @"[JOB] Signed " + totalSigned + @" File(s) in total";
            statusLabel.Text = @"[JOB] Done - see log for more information";

            // Enable the form's controls after signing the files again and show the output in the textbox
            DisableForm(false);
        }

        private void buttonShowAllCertDataPopup_Click(object sender, EventArgs e)
        {
            // Retrieving the text from the label
            var certificateInfo = labelCertificateInformation.Text;

            // Displaying the data in a MessageBox
            MessageBox.Show(certificateInfo, @"Certificate Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSaveLog_Click(object sender, EventArgs e)
        {
            // Show the SaveFileDialog and save the output to a .txt file
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = @"Text Files|*.txt"
            };

            // If the user selects a file and clicks OK, save the output to the file
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save the output to the file
                try
                {
                    // Write the output to the file
                    File.WriteAllText(sfd.FileName, textBoxOutput.Text);
                }
                catch (Exception exception)
                {
                    // Show an error message if the file could not be saved
                    MessageBox.Show(exception.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void ResetJob_Click(object sender, EventArgs e)
        {
            var resetJob = MessageBox.Show(
                @"Do you cant to reset?" + Environment.NewLine + _totalSigned + @" File(s) Signed in Job" +
                Environment.NewLine + checkedListBoxFiles.Items.Count + @" File(s) Imported to File List", @"Reset Job",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resetJob == DialogResult.Yes)
            {
                // Reset the job and clear the GUI
                _totalSigned = 0;
                _totalJob = 0;
                _jobSigned = 0;
                toolStripProgressBar.Value = 0;
                textBoxOutput.Clear();
                checkedListBoxFiles.Items.Clear();
                statusLabel.Text = @"[INFO] Ready to Sign, Add a Folder or File to List";
            }
        }
        
        private void checkBoxShowOutput_CheckedChanged(object sender, EventArgs e)
        {
            // Enable or disable the Debug and Verbose menu items based on the ShowOutput checkbox's Checked property
            if (checkBoxShowOutput.Checked)
            {
                menuItemSignDebug.Enabled = true;
                menuItemSignVerbose.Enabled = true;
            }
            switch (checkBoxShowOutput.Checked)
            {
                case false:
                    menuItemSignDebug.Enabled = false;
                    menuItemSignVerbose.Enabled = false;
                    menuItemSignDebug.Checked = false;
                    menuItemSignVerbose.Checked = false;
                    break;
            }
        }

        private bool VerifyConfiguration()
        {
            try
            {
                if (radioButtonWindowsCertificateStore.Checked)
                {
                    if (comboBoxCertificatesInStore.SelectedIndex == 0)
                    {
                        MessageBox.Show(@"Select a digital certificate from Windows Certificate Store.", @"Select a Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Indicate failure
                    }
                    if (!_signingCerts[comboBoxCertificatesInStore.SelectedIndex - 1].HasPrivateKey)
                    {
                        MessageBox.Show(
                            @"The private key of the selected certificate cannot be found.
The certificate cannot be used for digital signature operations.

Select another digital certificate.",
                            Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return false; // Indicate failure
                    }
                }

                if (string.IsNullOrEmpty(txtTimestampProviderURL.Text))
                {
                    MessageBox.Show(@"Please enter the URL of the timestamp provider.", @"SignTool GUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // Indicate failure
                }

                if (radioButtonPFXCertificate.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxPFXFile.Text))
                    {
                        MessageBox.Show(@"No .pfx/.p12 file selected.
Please provide a valid file to use for signing!
Use the ... button above and select the code signing certificate to use!", @"No .pfx/.p12 file selected", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        return false; // Indicate failure
                    }

                    if (!File.Exists(textBoxPFXFile.Text))
                    {
                        MessageBox.Show(@"The specified .pfx/.p12 file does not exist or access to it failed.", @"Selected file or path does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Indicate failure
                    }

                    if (string.IsNullOrEmpty(textBoxPFXPassword.Text))
                    {
                        MessageBox.Show(@".pfx/.p12 password cannot be empty.", @"Missing password", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        return false; // Indicate failure
                    }

                    try
                    {
                        if (!new X509Certificate2(File.ReadAllBytes(textBoxPFXFile.Text), textBoxPFXPassword.Text).HasPrivateKey)
                        {
                            MessageBox.Show(@"Error obtaining the private key from the .pfx/.p12 file. The certificate cannot be used to create digital signatures.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false; // Indicate failure
                        }
                    }
                    catch
                    {
                        MessageBox.Show(@"Error obtaining the certificate from the .pfx/.p12 file. Probably .pfx/.p12 password is not correct or the .pfx/.p12 file is invalid.", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        labelCertificateInformation.Text = Globals.DigitalCertificates.CertificateInfoIsNotAvailable;
                        return false; // Indicate failure
                    }
                }
            }
            catch (Exception exception)
            {
                // Show an error message
                MessageBox.Show(exception.ToString(), @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Indicate failure
            }

            return true; // Indicate success
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm f2 = new AboutForm();
            f2.ShowDialog();
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangelogForm f2 = new ChangelogForm();
            f2.ShowDialog();
        }
        
#endregion Form actions
        
        #region Sign options - GUI

        private void CheckedListBoxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            int deletedFilesCount = 0;
            // Check if the Delete key on the keyboard is pressed
            if (e.KeyCode != Keys.Delete)
                return; // If not, do nothing and exit the method

            // Get the index of the currently selected item in the CheckedListBox
            var selectedIndex = checkedListBoxFiles.SelectedIndex;

            // If an item is selected (index is not -1)
            if (selectedIndex >= 0)
            {
                // Remove the selected item from the CheckedListBox
                checkedListBoxFiles.Items.RemoveAt(selectedIndex);
                deletedFilesCount++; // Increment the deleted files counter

                // If there are still items left
                if (checkedListBoxFiles.Items.Count > 0)
                {
                    // Set the selected index to the previous item
                    checkedListBoxFiles.SelectedIndex = selectedIndex == 0 ? -1 : selectedIndex - 1;
                    // If the removed item was not the first one, select the previous item;
                    // Otherwise, if it was the first item, deselect all items (-1).
                }

                // Update the status label with the count of deleted files
                statusLabel.Text = $@"[INFO] {deletedFilesCount} file(s) deleted from the file list";
            }
        }

        private void ButtonAddFiles_Click(object sender, EventArgs e)
        {
            // Show the OpenFileDialog and add the selected files to the list
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = @"Executables and scripts (*.exe;*.dll;*.sys;*.ocx;*.ps1;*.msi;*.cat;*.cab;*.appx;*.appxbundle;*.msix;*.msixbundle)|*.exe;*.dll;*.sys;*.ocx;*.ps1;*.msi;*.cat;*.cab;*.appx;*.appxbundle;*.msix;*.msixbundle|All Files (*.*)|*.*";
            openFileDialog.FileName = string.Empty;

            // Check if the dialog result is OK and add the selected files to the list
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // count files before add selected files
                var currentFiles = checkedListBoxFiles.Items.Count;
                int totalFiles;

                // Loop through all files
                if (checkedListBoxFiles != null)
                {
                    // Add the selected files to the CheckedListBox
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        checkedListBoxFiles.Items.Add(fileName, true);
                    }

                    // Count the new number of files
                    totalFiles = checkedListBoxFiles.Items.Count;
                }
                else
                {
                    // Handle the case where checkedListBoxFiles is null
                    totalFiles = 0; // Or handle it according to your application's needs
                }

                // calc added files
                var addedFiles = currentFiles - totalFiles;

                // show status
                statusLabel.Text = @"[INFO] " + addedFiles + @" file(s) imported to File List";
            }

            if (checkedListBoxFiles != null)
            {
                // Check all files in the list
                CheckAllFiles();
            }
        }

        private void ButtonAddDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // count files before add selected files
                var currentFiles = checkedListBoxFiles.Items.Count;
                try
                {
                    var searchOption = SearchOption.TopDirectoryOnly;

                    if (checkBoxSubdirectories.Checked)
                    {
                        searchOption = SearchOption.AllDirectories;
                    }

                    // Set filter for file extension and add all files from selected folder
                    var files = GetFiles(folderBrowserDialog.SelectedPath, "*.exe;*.dll;*.sys;*.ocx;*.ps1", searchOption);

                    // loop in all files
                    foreach (var file in files)
                    {
                        checkedListBoxFiles.Items.Add(file);
                    }

                    // calc added files
                    var totalFiles = checkedListBoxFiles.Items.Count;
                    var addedFiles = currentFiles - totalFiles;

                    // show status
                    statusLabel.Text = @"[INFO] " + addedFiles + @" file(s) imported to File List from folder";
                }
                catch
                {
                    // ignored
                }
            }

            // Check all files in the list
            if (checkedListBoxFiles?.Items != null)
            {
                // Check all files in the list after adding files
                CheckAllFiles();
            }
        }

        public void CheckAllFiles()
        {
            // Check all files in the list
            for (var i = 0; i < checkedListBoxFiles.Items.Count; i++)
            {
                checkedListBoxFiles.SetItemChecked(i, true);
            }
        }

        private void ButtonSelectPFXCertificate_Click(object sender, EventArgs e)
        {
            // Open the OpenFileDialog to select a PFX file for signing
            try
            {
                // Create a new instance of the OpenFileDialog class.
                var fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = @"Personal Information Exchange (*.pfx)|*.pfx|Personal Information Exchange (*.p12)|*.p12",
                    Title = @"Pfx/P12 Signing File (*.p12 *.pfx) for signing"
                };

                // Check if the dialog result is OK and set the selected file to the text box.
                if (fileDialog.ShowDialog() == DialogResult.OK) textBoxPFXFile.Text = fileDialog.FileName;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonShowSigningCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the Windows Certificate Store radio button is selected.
                if (radioButtonWindowsCertificateStore.Checked)
                {
                    // If no certificate is selected in the combo box, exit the method.
                    if (comboBoxCertificatesInStore.SelectedIndex <= 0)
                        return;

                    // Display the selected certificate from the signing certificates list.
                    X509Certificate2UI.DisplayCertificate(new X509Certificate2((X509Certificate)_signingCerts[comboBoxCertificatesInStore.SelectedIndex - 1]));
                }
                else
                {
                    // Verify the configuration for PFX certificate usage.
                    VerifyConfiguration();

                    // Display the certificate obtained from the PFX file.
                    X509Certificate2UI.DisplayCertificate(GetCertificateFromPfx());
                }

                // Check if the PFX Certificate radio button is selected.
                if (radioButtonPFXCertificate.Checked)
                {
                    // If the label for certificate information is not null, update it with the certificate info.
                    if (labelCertificateInformation != null)
                        labelCertificateInformation.Text = GetCertificateInfo(GetCertificateFromPfx());
                }
            }
            catch (Exception)
            {
                //int num = (int)MessageBox.Show(ex.Message, Txt.MsgBox.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion Sign options - GUI

        #region Sign files - GUI

        private static string[] GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            // Return all files from the source folder that match the filters
            return filters.Split(';').SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }

        private void ContextMenuItemSignOption_Click(object sender, EventArgs e)
        {
            // Get the MenuItem that was clicked
            var menuItem = (MenuItem)sender;
            menuItem.Checked = !menuItem.Checked;
        }

        private void CheckBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            // Get the current checked state of the "Select All" checkbox.
            var isChecked = checkBoxAll.Checked;

            // Get the total number of items in the checked list box.
            var count = checkedListBoxFiles.Items.Count;

            // Loop through each item in the checked list box.
            for (var i = 0; i < count; i++)
            {
                // Set the checked state of each item to match the state of the "Select All" checkbox.
                checkedListBoxFiles.SetItemChecked(i, isChecked);
            }
        }

        #endregion Sign files - GUI

        #region Certificate info

        private X509Certificate2Collection GetCertificatesFromStore(StoreLocation store)
        {
            // Create a new instance of the X509Store class, specifying the store name and location.
            using (var x509Store = new X509Store(StoreName.My, store))
            {
                try
                {
                    // Open the X509 store, only if it already exists.
                    x509Store.Open(OpenFlags.OpenExistingOnly);

                    // Check if the "Show Expired Certificates" checkbox is checked.
                    // If it is, find certificates by issuer name without checking the validity period.
                    // Otherwise, find certificates that are valid at the current date and time.
                    return checkBoxShowExpiredCertificates.Checked
                        ? x509Store.Certificates.Find(X509FindType.FindByIssuerName, string.Empty, false)
                        : x509Store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                }
                finally
                {
                    // Ensure the store is closed even if an exception occurs.
                    x509Store.Close();
                }
            }
        }

        private static string GetCspName(X509Certificate2 cert)
        {
            // Initialize an empty string variable to hold the provider name.
            var str = string.Empty;

            try
            {
                // Attempt to cast the certificate's private key to an RSACryptoServiceProvider
                // and retrieve the provider name from the CspKeyContainerInfo.
                str = (cert.PrivateKey as RSACryptoServiceProvider)?.CspKeyContainerInfo.ProviderName;
            }
            catch
            {
                // If an exception occurs, ignore it and proceed.
            }

            // Return the provider name or an empty string if an exception was caught.
            return str;

        }

        private string GetCertificateInfo(X509Certificate2 cert)
        {
            try
            {
                // Get the Certificate Service Provider (CSP) name
                var strCsp = GetCspName(cert);

                // Add Certificate Service Provider information to the string
                if (strCsp != string.Empty)
                {
                    strCsp = "Certificate Service Provider: " + strCsp;
                }
                else
                {
                    strCsp = "Certificate Service Provider: N/A";
                }

                // Extract Extended Key Usage information
                string extendedKeyUsageInfo = "Extended Key Usage: " + Environment.NewLine;

                // Retrieve the Extended Key Usage extension from the certificate
                X509Extension ext = cert.Extensions["2.5.29.37"]; // OID for Extended Key Usage
                if (ext != null)
                {
                    // Parse the extension and retrieve the Enhanced Key Usage information
                    var extendedKeyUsage = new X509EnhancedKeyUsageExtension(ext, true);
                    foreach (var oid in extendedKeyUsage.EnhancedKeyUsages)
                    {
                        extendedKeyUsageInfo += "- " + oid.FriendlyName + Environment.NewLine;
                    }
                }
                else
                {
                    // If no Extended Key Usage extension is found in the certificate
                    extendedKeyUsageInfo += "No Extended Key Usage information available." + Environment.NewLine;
                }

                // Store the Thumbprint of the certificate
                ThumbprintFromCertToSign = cert.Thumbprint;

                // Construct and return a string containing detailed certificate information
                return "Issued to: " + cert.GetNameInfo(X509NameType.SimpleName, false) + Environment.NewLine +
                       "Issued by: " + cert.GetNameInfo(X509NameType.SimpleName, true) + Environment.NewLine +
                       "Valid from: " + cert.NotBefore.ToShortDateString() + Environment.NewLine +
                       "Valid until: " + cert.NotAfter.ToShortDateString() + Environment.NewLine + strCsp + Environment.NewLine +
                       "Thumbprint: " + cert.Thumbprint + Environment.NewLine +
                       "Public Key Algorithm: " + cert.PublicKey.Oid.FriendlyName + Environment.NewLine +
                       "Key Size (in bits): " + cert.PublicKey.Key.KeySize + Environment.NewLine +
                       extendedKeyUsageInfo;
            }
            catch
            {
                // In case of an exception, return a default message indicating certificate information is not available
                return Globals.DigitalCertificates.CertificateInfoIsNotAvailable;
            }
        }

        private X509Certificate2 GetCertificateFromPfx()
        {
            try
            {
                // Load the certificate from the specified PFX file using the provided password.
                var certificate = new X509Certificate2(File.ReadAllBytes(textBoxPFXFile.Text), textBoxPFXPassword.Text);

                // Check if the loaded certificate has a private key.
                if (certificate.HasPrivateKey)
                    // If it has a private key, return the certificate.
                    return certificate;

                // If the certificate does not have a private key, return null.
                return (X509Certificate2)null;
            }
            catch
            {
                // If an exception occurs (e.g., incorrect file path or password), return null.
                return (X509Certificate2)null;
            }
        }

        private void ShowCertificatesFromStore(string loadStore)
        {
            try
            {
                // Load certificates based on the specified store location.
                _signingCerts = loadStore != "Current User"
                    ? GetCertificatesFromStore(StoreLocation.LocalMachine)
                    : GetCertificatesFromStore(StoreLocation.CurrentUser);

                // Filter certificates to only include those with a private key and for code signing.
                _signingCerts = new X509Certificate2Collection(_signingCerts.Cast<X509Certificate2>()
                    .Where(cert => cert.HasPrivateKey && cert.Extensions.OfType<X509EnhancedKeyUsageExtension>()
                    .Any(ext => ext.EnhancedKeyUsages.Cast<Oid>().Any(oid => oid.Value == "1.3.6.1.5.5.7.3.3"))).ToArray());

                // Remember the previously selected certificate, if any.
                string previousSelection = comboBoxCertificatesInStore.SelectedIndex > 0
                    ? comboBoxCertificatesInStore.SelectedItem.ToString()
                    : null;

                // Clear existing items from the combo box.
                comboBoxCertificatesInStore.Items.Clear();

                // Add a default option to the combo box.
                comboBoxCertificatesInStore.Items.Add("<No certificate selected>");

                // Iterate through the loaded certificates and add their simple names to the combo box.
                foreach (X509Certificate2 signingCert in _signingCerts)
                {
                    comboBoxCertificatesInStore.Items.Add(signingCert.GetNameInfo(X509NameType.SimpleName, false) ??
                                                           throw new InvalidOperationException());
                }

                // Attempt to restore the previous selection if it exists and is valid.
                if (previousSelection != null)
                {
                    int newIndex = comboBoxCertificatesInStore.Items.IndexOf(previousSelection);
                    comboBoxCertificatesInStore.SelectedIndex = newIndex >= 0 ? newIndex : 0;
                }
                else if (_signingCerts.Count > 0)
                {
                    // If there was no previous selection, select the first valid certificate by default.
                    comboBoxCertificatesInStore.SelectedIndex = 1;
                }
                else
                {
                    // If no certificates were found, keep the default "<No certificate selected>" selected.
                    comboBoxCertificatesInStore.SelectedIndex = 0;
                    labelCertificateInformation.Text = $@"No certificates were found in the {comboBoxCertificateStore.Text} certificate store.";
                }
            }
            catch (Exception ex)
            {
                // Display an error message box if an exception occurs.
                MessageBox.Show(ex.Message, Globals.MsgBox.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        
        #endregion Certificate info

    }
}