namespace SignToolGUI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonBrowseSignTool = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelTimeStampServer = new System.Windows.Forms.Label();
            this.groupBoxFiles = new System.Windows.Forms.GroupBox();
            this.ResetJob = new System.Windows.Forms.Button();
            this.checkBoxShowOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.checkBoxSubdirectories = new System.Windows.Forms.CheckBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonAddDirectory = new System.Windows.Forms.Button();
            this.buttonAddFiles = new System.Windows.Forms.Button();
            this.checkedListBoxFiles = new System.Windows.Forms.CheckedListBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.splitButtonSign = new wyDay.Controls.SplitButton();
            this.contextMenuSignSwitches = new System.Windows.Forms.ContextMenu();
            this.menuItemSignVerbose = new System.Windows.Forms.MenuItem();
            this.menuItemSignDebug = new System.Windows.Forms.MenuItem();
            this.textBoxSignToolPath = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelCertificateInformation = new System.Windows.Forms.Label();
            this.comboBoxCertificateStore = new System.Windows.Forms.ComboBox();
            this.labelCertificateStore = new System.Windows.Forms.Label();
            this.checkBoxShowExpiredCertificates = new System.Windows.Forms.CheckBox();
            this.comboBoxCertificatesInStore = new System.Windows.Forms.ComboBox();
            this.buttonShowSigninigCertificateStore = new System.Windows.Forms.Button();
            this.groupBoxCertificateInformation = new System.Windows.Forms.GroupBox();
            this.labelSelectTheDigitalCertificate = new System.Windows.Forms.Label();
            this.PFXFilePassword = new System.Windows.Forms.Label();
            this.buttonSelectPFXCertificate = new System.Windows.Forms.Button();
            this.textBoxPFXPassword = new System.Windows.Forms.TextBox();
            this.textBoxPFXFile = new System.Windows.Forms.TextBox();
            this.radioButtonPFXCertificate = new System.Windows.Forms.RadioButton();
            this.radioButtonWindowsCertificateStore = new System.Windows.Forms.RadioButton();
            this.groupBoxPFXCertificate = new System.Windows.Forms.GroupBox();
            this.buttonShowSigninigCertificatePFX = new System.Windows.Forms.Button();
            this.groupBoxWindowsCertificateStore = new System.Windows.Forms.GroupBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonSaveLog = new System.Windows.Forms.Button();
            this.buttonShowAllCertDataPopup = new System.Windows.Forms.Button();
            this.groupBoxTimestamp = new System.Windows.Forms.GroupBox();
            this.txtTimestampProviderURL = new System.Windows.Forms.TextBox();
            this.labelTimestampProvider = new System.Windows.Forms.Label();
            this.comboBoxTimestampProviders = new System.Windows.Forms.ComboBox();
            this.groupBoxSignTool = new System.Windows.Forms.GroupBox();
            this.groupBoxTrustedSigningMetadata = new System.Windows.Forms.GroupBox();
            this.labelCorrelationId = new System.Windows.Forms.Label();
            this.labelCertificateProfileName = new System.Windows.Forms.Label();
            this.labelCodeSigningAccountName = new System.Windows.Forms.Label();
            this.textBoxCorrelationId = new System.Windows.Forms.TextBox();
            this.textBoxCertificateProfileName = new System.Windows.Forms.TextBox();
            this.textBoxCodeSigningAccountName = new System.Windows.Forms.TextBox();
            this.radioButtonTrustedSigning = new System.Windows.Forms.RadioButton();
            this.labelSignedBuildState = new System.Windows.Forms.Label();
            this.linkLabelOpenTrustedSigningPortal = new System.Windows.Forms.LinkLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxTimestamp = new System.Windows.Forms.CheckBox();
            this.groupBoxFiles.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.groupBoxCertificateInformation.SuspendLayout();
            this.groupBoxPFXCertificate.SuspendLayout();
            this.groupBoxWindowsCertificateStore.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.groupBoxTimestamp.SuspendLayout();
            this.groupBoxSignTool.SuspendLayout();
            this.groupBoxTrustedSigningMetadata.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonBrowseSignTool
            // 
            this.buttonBrowseSignTool.Location = new System.Drawing.Point(315, 17);
            this.buttonBrowseSignTool.Name = "buttonBrowseSignTool";
            this.buttonBrowseSignTool.Size = new System.Drawing.Size(52, 22);
            this.buttonBrowseSignTool.TabIndex = 2;
            this.buttonBrowseSignTool.Text = "Browse...";
            this.buttonBrowseSignTool.UseVisualStyleBackColor = true;
            this.buttonBrowseSignTool.Click += new System.EventHandler(this.buttonBrowseSignTool_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = "%programfiles%";
            // 
            // labelTimeStampServer
            // 
            this.labelTimeStampServer.AutoSize = true;
            this.labelTimeStampServer.Location = new System.Drawing.Point(5, 50);
            this.labelTimeStampServer.Name = "labelTimeStampServer";
            this.labelTimeStampServer.Size = new System.Drawing.Size(86, 13);
            this.labelTimeStampServer.TabIndex = 3;
            this.labelTimeStampServer.Text = "Timestamp URL:";
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFiles.Controls.Add(this.ResetJob);
            this.groupBoxFiles.Controls.Add(this.checkBoxShowOutput);
            this.groupBoxFiles.Controls.Add(this.checkBoxAll);
            this.groupBoxFiles.Controls.Add(this.checkBoxSubdirectories);
            this.groupBoxFiles.Controls.Add(this.buttonClear);
            this.groupBoxFiles.Controls.Add(this.buttonAddDirectory);
            this.groupBoxFiles.Controls.Add(this.buttonAddFiles);
            this.groupBoxFiles.Controls.Add(this.checkedListBoxFiles);
            this.groupBoxFiles.Location = new System.Drawing.Point(7, 441);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Size = new System.Drawing.Size(667, 212);
            this.groupBoxFiles.TabIndex = 1;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "Files to digital signing";
            // 
            // ResetJob
            // 
            this.ResetJob.Location = new System.Drawing.Point(337, 242);
            this.ResetJob.Margin = new System.Windows.Forms.Padding(2);
            this.ResetJob.Name = "ResetJob";
            this.ResetJob.Size = new System.Drawing.Size(75, 23);
            this.ResetJob.TabIndex = 9;
            this.ResetJob.Text = "Reset job";
            this.ResetJob.UseVisualStyleBackColor = true;
            this.ResetJob.Click += new System.EventHandler(this.ResetJob_Click);
            // 
            // checkBoxShowOutput
            // 
            this.checkBoxShowOutput.AutoSize = true;
            this.checkBoxShowOutput.Location = new System.Drawing.Point(82, 186);
            this.checkBoxShowOutput.Name = "checkBoxShowOutput";
            this.checkBoxShowOutput.Size = new System.Drawing.Size(99, 17);
            this.checkBoxShowOutput.TabIndex = 6;
            this.checkBoxShowOutput.Text = "Show all output";
            this.checkBoxShowOutput.UseVisualStyleBackColor = true;
            this.checkBoxShowOutput.CheckedChanged += new System.EventHandler(this.checkBoxShowOutput_CheckedChanged);
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Checked = true;
            this.checkBoxAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAll.Location = new System.Drawing.Point(82, 246);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(58, 17);
            this.checkBoxAll.TabIndex = 5;
            this.checkBoxAll.Text = "All files";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.CheckBoxAll_CheckedChanged);
            // 
            // checkBoxSubdirectories
            // 
            this.checkBoxSubdirectories.AutoSize = true;
            this.checkBoxSubdirectories.Checked = true;
            this.checkBoxSubdirectories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSubdirectories.Location = new System.Drawing.Point(140, 246);
            this.checkBoxSubdirectories.Name = "checkBoxSubdirectories";
            this.checkBoxSubdirectories.Size = new System.Drawing.Size(93, 17);
            this.checkBoxSubdirectories.TabIndex = 4;
            this.checkBoxSubdirectories.Text = "Subdirectories";
            this.checkBoxSubdirectories.UseVisualStyleBackColor = true;
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClear.Location = new System.Drawing.Point(6, 182);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(70, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear list";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // buttonAddDirectory
            // 
            this.buttonAddDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddDirectory.Location = new System.Drawing.Point(493, 182);
            this.buttonAddDirectory.Name = "buttonAddDirectory";
            this.buttonAddDirectory.Size = new System.Drawing.Size(85, 23);
            this.buttonAddDirectory.TabIndex = 2;
            this.buttonAddDirectory.Text = "Add directory";
            this.buttonAddDirectory.UseVisualStyleBackColor = true;
            this.buttonAddDirectory.Click += new System.EventHandler(this.ButtonAddDirectory_Click);
            // 
            // buttonAddFiles
            // 
            this.buttonAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddFiles.Location = new System.Drawing.Point(584, 182);
            this.buttonAddFiles.Name = "buttonAddFiles";
            this.buttonAddFiles.Size = new System.Drawing.Size(77, 23);
            this.buttonAddFiles.TabIndex = 1;
            this.buttonAddFiles.Text = "Add files...";
            this.buttonAddFiles.UseVisualStyleBackColor = true;
            this.buttonAddFiles.Click += new System.EventHandler(this.ButtonAddFiles_Click);
            // 
            // checkedListBoxFiles
            // 
            this.checkedListBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxFiles.FormattingEnabled = true;
            this.checkedListBoxFiles.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxFiles.Name = "checkedListBoxFiles";
            this.checkedListBoxFiles.Size = new System.Drawing.Size(655, 154);
            this.checkedListBoxFiles.TabIndex = 0;
            this.checkedListBoxFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckedListBoxFiles_KeyDown);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BackColor = System.Drawing.Color.White;
            this.textBoxOutput.Location = new System.Drawing.Point(7, 658);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(667, 168);
            this.textBoxOutput.TabIndex = 2;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select a folder that contains the assemblies";
            // 
            // splitButtonSign
            // 
            this.splitButtonSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.splitButtonSign.AutoSize = true;
            this.splitButtonSign.Location = new System.Drawing.Point(599, 830);
            this.splitButtonSign.Name = "splitButtonSign";
            this.splitButtonSign.Size = new System.Drawing.Size(75, 23);
            this.splitButtonSign.SplitMenu = this.contextMenuSignSwitches;
            this.splitButtonSign.TabIndex = 3;
            this.splitButtonSign.Text = "Sign...";
            this.splitButtonSign.UseVisualStyleBackColor = true;
            this.splitButtonSign.Click += new System.EventHandler(this.SplitButtonSign_Click);
            // 
            // contextMenuSignSwitches
            // 
            this.contextMenuSignSwitches.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSignVerbose,
            this.menuItemSignDebug});
            // 
            // menuItemSignVerbose
            // 
            this.menuItemSignVerbose.Enabled = false;
            this.menuItemSignVerbose.Index = 0;
            this.menuItemSignVerbose.RadioCheck = true;
            this.menuItemSignVerbose.Text = "Verbose";
            this.menuItemSignVerbose.Click += new System.EventHandler(this.ContextMenuItemSignOption_Click);
            // 
            // menuItemSignDebug
            // 
            this.menuItemSignDebug.Enabled = false;
            this.menuItemSignDebug.Index = 1;
            this.menuItemSignDebug.RadioCheck = true;
            this.menuItemSignDebug.Text = "Debug";
            this.menuItemSignDebug.Click += new System.EventHandler(this.ContextMenuItemSignOption_Click);
            // 
            // textBoxSignToolPath
            // 
            this.textBoxSignToolPath.Location = new System.Drawing.Point(120, 18);
            this.textBoxSignToolPath.Name = "textBoxSignToolPath";
            this.textBoxSignToolPath.Size = new System.Drawing.Size(189, 20);
            this.textBoxSignToolPath.TabIndex = 5;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(680, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changelogToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // labelCertificateInformation
            // 
            this.labelCertificateInformation.Location = new System.Drawing.Point(4, 17);
            this.labelCertificateInformation.Name = "labelCertificateInformation";
            this.labelCertificateInformation.Size = new System.Drawing.Size(273, 215);
            this.labelCertificateInformation.TabIndex = 0;
            this.labelCertificateInformation.Text = "Information...";
            // 
            // comboBoxCertificateStore
            // 
            this.comboBoxCertificateStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCertificateStore.FormattingEnabled = true;
            this.comboBoxCertificateStore.Items.AddRange(new object[] {
            "Current User",
            "Local Machine"});
            this.comboBoxCertificateStore.Location = new System.Drawing.Point(100, 22);
            this.comboBoxCertificateStore.Name = "comboBoxCertificateStore";
            this.comboBoxCertificateStore.Size = new System.Drawing.Size(94, 21);
            this.comboBoxCertificateStore.TabIndex = 1;
            this.comboBoxCertificateStore.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCertificateStore_SelectedIndexChanged);
            // 
            // labelCertificateStore
            // 
            this.labelCertificateStore.AutoSize = true;
            this.labelCertificateStore.Location = new System.Drawing.Point(9, 25);
            this.labelCertificateStore.Name = "labelCertificateStore";
            this.labelCertificateStore.Size = new System.Drawing.Size(85, 13);
            this.labelCertificateStore.TabIndex = 0;
            this.labelCertificateStore.Text = "Certificate Store:";
            // 
            // checkBoxShowExpiredCertificates
            // 
            this.checkBoxShowExpiredCertificates.AutoSize = true;
            this.checkBoxShowExpiredCertificates.Location = new System.Drawing.Point(208, 25);
            this.checkBoxShowExpiredCertificates.Name = "checkBoxShowExpiredCertificates";
            this.checkBoxShowExpiredCertificates.Size = new System.Drawing.Size(144, 17);
            this.checkBoxShowExpiredCertificates.TabIndex = 2;
            this.checkBoxShowExpiredCertificates.Text = "Show expired certificates";
            this.checkBoxShowExpiredCertificates.UseVisualStyleBackColor = true;
            this.checkBoxShowExpiredCertificates.CheckedChanged += new System.EventHandler(this.ComboBoxCertificateStore_SelectedIndexChanged);
            // 
            // comboBoxCertificatesInStore
            // 
            this.comboBoxCertificatesInStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCertificatesInStore.FormattingEnabled = true;
            this.comboBoxCertificatesInStore.Location = new System.Drawing.Point(9, 52);
            this.comboBoxCertificatesInStore.Name = "comboBoxCertificatesInStore";
            this.comboBoxCertificatesInStore.Size = new System.Drawing.Size(291, 21);
            this.comboBoxCertificatesInStore.TabIndex = 3;
            this.comboBoxCertificatesInStore.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCertificates_SelectedIndexChanged);
            // 
            // buttonShowSigninigCertificateStore
            // 
            this.buttonShowSigninigCertificateStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShowSigninigCertificateStore.Location = new System.Drawing.Point(302, 50);
            this.buttonShowSigninigCertificateStore.Name = "buttonShowSigninigCertificateStore";
            this.buttonShowSigninigCertificateStore.Size = new System.Drawing.Size(65, 23);
            this.buttonShowSigninigCertificateStore.TabIndex = 4;
            this.buttonShowSigninigCertificateStore.Text = "Show";
            this.buttonShowSigninigCertificateStore.UseVisualStyleBackColor = true;
            this.buttonShowSigninigCertificateStore.Click += new System.EventHandler(this.ButtonShowSigningCertificate_Click);
            // 
            // groupBoxCertificateInformation
            // 
            this.groupBoxCertificateInformation.Controls.Add(this.labelCertificateInformation);
            this.groupBoxCertificateInformation.Location = new System.Drawing.Point(386, 55);
            this.groupBoxCertificateInformation.Name = "groupBoxCertificateInformation";
            this.groupBoxCertificateInformation.Size = new System.Drawing.Size(288, 235);
            this.groupBoxCertificateInformation.TabIndex = 14;
            this.groupBoxCertificateInformation.TabStop = false;
            this.groupBoxCertificateInformation.Text = "Certificate Information";
            // 
            // labelSelectTheDigitalCertificate
            // 
            this.labelSelectTheDigitalCertificate.AutoSize = true;
            this.labelSelectTheDigitalCertificate.Location = new System.Drawing.Point(218, 34);
            this.labelSelectTheDigitalCertificate.Name = "labelSelectTheDigitalCertificate";
            this.labelSelectTheDigitalCertificate.Size = new System.Drawing.Size(251, 13);
            this.labelSelectTheDigitalCertificate.TabIndex = 9;
            this.labelSelectTheDigitalCertificate.Text = "Select the digital certificate used for digital signature";
            // 
            // PFXFilePassword
            // 
            this.PFXFilePassword.AutoSize = true;
            this.PFXFilePassword.Location = new System.Drawing.Point(6, 56);
            this.PFXFilePassword.Name = "PFXFilePassword";
            this.PFXFilePassword.Size = new System.Drawing.Size(56, 13);
            this.PFXFilePassword.TabIndex = 2;
            this.PFXFilePassword.Text = "Password:";
            // 
            // buttonSelectPFXCertificate
            // 
            this.buttonSelectPFXCertificate.Location = new System.Drawing.Point(8, 22);
            this.buttonSelectPFXCertificate.Name = "buttonSelectPFXCertificate";
            this.buttonSelectPFXCertificate.Size = new System.Drawing.Size(32, 20);
            this.buttonSelectPFXCertificate.TabIndex = 0;
            this.buttonSelectPFXCertificate.Text = "...";
            this.buttonSelectPFXCertificate.UseVisualStyleBackColor = true;
            this.buttonSelectPFXCertificate.Click += new System.EventHandler(this.ButtonSelectPFXCertificate_Click);
            // 
            // textBoxPFXPassword
            // 
            this.textBoxPFXPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPFXPassword.Location = new System.Drawing.Point(70, 52);
            this.textBoxPFXPassword.Name = "textBoxPFXPassword";
            this.textBoxPFXPassword.Size = new System.Drawing.Size(163, 20);
            this.textBoxPFXPassword.TabIndex = 3;
            this.textBoxPFXPassword.UseSystemPasswordChar = true;
            this.textBoxPFXPassword.TextChanged += new System.EventHandler(this.textBoxPFXFile_TextChanged);
            // 
            // textBoxPFXFile
            // 
            this.textBoxPFXFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPFXFile.Location = new System.Drawing.Point(46, 22);
            this.textBoxPFXFile.Name = "textBoxPFXFile";
            this.textBoxPFXFile.Size = new System.Drawing.Size(321, 20);
            this.textBoxPFXFile.TabIndex = 1;
            this.textBoxPFXFile.TextChanged += new System.EventHandler(this.textBoxPFXFile_TextChanged);
            // 
            // radioButtonPFXCertificate
            // 
            this.radioButtonPFXCertificate.AutoSize = true;
            this.radioButtonPFXCertificate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonPFXCertificate.Location = new System.Drawing.Point(19, 30);
            this.radioButtonPFXCertificate.Name = "radioButtonPFXCertificate";
            this.radioButtonPFXCertificate.Size = new System.Drawing.Size(176, 17);
            this.radioButtonPFXCertificate.TabIndex = 12;
            this.radioButtonPFXCertificate.Text = "Digital certificate file (.pfx)";
            this.radioButtonPFXCertificate.UseVisualStyleBackColor = true;
            this.radioButtonPFXCertificate.CheckedChanged += new System.EventHandler(this.RadioButtonSelectCertificateLocation_CheckedChanged);
            // 
            // radioButtonWindowsCertificateStore
            // 
            this.radioButtonWindowsCertificateStore.AutoSize = true;
            this.radioButtonWindowsCertificateStore.Checked = true;
            this.radioButtonWindowsCertificateStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonWindowsCertificateStore.Location = new System.Drawing.Point(15, 148);
            this.radioButtonWindowsCertificateStore.Name = "radioButtonWindowsCertificateStore";
            this.radioButtonWindowsCertificateStore.Size = new System.Drawing.Size(172, 17);
            this.radioButtonWindowsCertificateStore.TabIndex = 10;
            this.radioButtonWindowsCertificateStore.TabStop = true;
            this.radioButtonWindowsCertificateStore.Text = "Windows Certificate Store";
            this.radioButtonWindowsCertificateStore.UseVisualStyleBackColor = true;
            this.radioButtonWindowsCertificateStore.Click += new System.EventHandler(this.RadioButtonSelectCertificateLocation_CheckedChanged);
            // 
            // groupBoxPFXCertificate
            // 
            this.groupBoxPFXCertificate.Controls.Add(this.PFXFilePassword);
            this.groupBoxPFXCertificate.Controls.Add(this.buttonSelectPFXCertificate);
            this.groupBoxPFXCertificate.Controls.Add(this.textBoxPFXPassword);
            this.groupBoxPFXCertificate.Controls.Add(this.textBoxPFXFile);
            this.groupBoxPFXCertificate.Controls.Add(this.buttonShowSigninigCertificatePFX);
            this.groupBoxPFXCertificate.Enabled = false;
            this.groupBoxPFXCertificate.Location = new System.Drawing.Point(7, 55);
            this.groupBoxPFXCertificate.Name = "groupBoxPFXCertificate";
            this.groupBoxPFXCertificate.Size = new System.Drawing.Size(373, 87);
            this.groupBoxPFXCertificate.TabIndex = 13;
            this.groupBoxPFXCertificate.TabStop = false;
            this.groupBoxPFXCertificate.Text = "Certificate File";
            // 
            // buttonShowSigninigCertificatePFX
            // 
            this.buttonShowSigninigCertificatePFX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShowSigninigCertificatePFX.Location = new System.Drawing.Point(241, 50);
            this.buttonShowSigninigCertificatePFX.Name = "buttonShowSigninigCertificatePFX";
            this.buttonShowSigninigCertificatePFX.Size = new System.Drawing.Size(126, 23);
            this.buttonShowSigninigCertificatePFX.TabIndex = 4;
            this.buttonShowSigninigCertificatePFX.Text = "Show/check certificate";
            this.buttonShowSigninigCertificatePFX.UseVisualStyleBackColor = true;
            this.buttonShowSigninigCertificatePFX.Click += new System.EventHandler(this.ButtonShowSigningCertificate_Click);
            // 
            // groupBoxWindowsCertificateStore
            // 
            this.groupBoxWindowsCertificateStore.Controls.Add(this.comboBoxCertificateStore);
            this.groupBoxWindowsCertificateStore.Controls.Add(this.labelCertificateStore);
            this.groupBoxWindowsCertificateStore.Controls.Add(this.checkBoxShowExpiredCertificates);
            this.groupBoxWindowsCertificateStore.Controls.Add(this.comboBoxCertificatesInStore);
            this.groupBoxWindowsCertificateStore.Controls.Add(this.buttonShowSigninigCertificateStore);
            this.groupBoxWindowsCertificateStore.Location = new System.Drawing.Point(7, 173);
            this.groupBoxWindowsCertificateStore.Name = "groupBoxWindowsCertificateStore";
            this.groupBoxWindowsCertificateStore.Size = new System.Drawing.Size(373, 81);
            this.groupBoxWindowsCertificateStore.TabIndex = 11;
            this.groupBoxWindowsCertificateStore.TabStop = false;
            this.groupBoxWindowsCertificateStore.Text = "Code Sign certificates available in Windows Certificate Store";
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.White;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.statusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 837);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(680, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 15;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(66, 17);
            this.statusLabel.Text = "statusLabel";
            // 
            // buttonSaveLog
            // 
            this.buttonSaveLog.Location = new System.Drawing.Point(515, 830);
            this.buttonSaveLog.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveLog.Name = "buttonSaveLog";
            this.buttonSaveLog.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveLog.TabIndex = 17;
            this.buttonSaveLog.Text = "Save Log";
            this.buttonSaveLog.UseVisualStyleBackColor = true;
            this.buttonSaveLog.Click += new System.EventHandler(this.buttonSaveLog_Click);
            // 
            // buttonShowAllCertDataPopup
            // 
            this.buttonShowAllCertDataPopup.Location = new System.Drawing.Point(548, 34);
            this.buttonShowAllCertDataPopup.Name = "buttonShowAllCertDataPopup";
            this.buttonShowAllCertDataPopup.Size = new System.Drawing.Size(126, 23);
            this.buttonShowAllCertDataPopup.TabIndex = 18;
            this.buttonShowAllCertDataPopup.Text = "Show all data in pop-up";
            this.buttonShowAllCertDataPopup.UseVisualStyleBackColor = true;
            this.buttonShowAllCertDataPopup.Click += new System.EventHandler(this.buttonShowAllCertDataPopup_Click);
            // 
            // groupBoxTimestamp
            // 
            this.groupBoxTimestamp.Controls.Add(this.checkBoxTimestamp);
            this.groupBoxTimestamp.Controls.Add(this.txtTimestampProviderURL);
            this.groupBoxTimestamp.Controls.Add(this.labelTimestampProvider);
            this.groupBoxTimestamp.Controls.Add(this.comboBoxTimestampProviders);
            this.groupBoxTimestamp.Controls.Add(this.labelTimeStampServer);
            this.groupBoxTimestamp.Location = new System.Drawing.Point(386, 296);
            this.groupBoxTimestamp.Name = "groupBoxTimestamp";
            this.groupBoxTimestamp.Size = new System.Drawing.Size(288, 96);
            this.groupBoxTimestamp.TabIndex = 19;
            this.groupBoxTimestamp.TabStop = false;
            this.groupBoxTimestamp.Text = "Timestamp";
            // 
            // txtTimestampProviderURL
            // 
            this.txtTimestampProviderURL.Location = new System.Drawing.Point(97, 47);
            this.txtTimestampProviderURL.Name = "txtTimestampProviderURL";
            this.txtTimestampProviderURL.ReadOnly = true;
            this.txtTimestampProviderURL.Size = new System.Drawing.Size(173, 20);
            this.txtTimestampProviderURL.TabIndex = 20;
            // 
            // labelTimestampProvider
            // 
            this.labelTimestampProvider.AutoSize = true;
            this.labelTimestampProvider.Location = new System.Drawing.Point(5, 22);
            this.labelTimestampProvider.Name = "labelTimestampProvider";
            this.labelTimestampProvider.Size = new System.Drawing.Size(49, 13);
            this.labelTimestampProvider.TabIndex = 21;
            this.labelTimestampProvider.Text = "Provider:";
            // 
            // comboBoxTimestampProviders
            // 
            this.comboBoxTimestampProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTimestampProviders.FormattingEnabled = true;
            this.comboBoxTimestampProviders.Items.AddRange(new object[] {
            "Sectigo (Comodo)",
            "DigiCert",
            "GlobalSign (1)",
            "Globalsign (2)",
            "Certum",
            "Custom Provider"});
            this.comboBoxTimestampProviders.Location = new System.Drawing.Point(97, 19);
            this.comboBoxTimestampProviders.Name = "comboBoxTimestampProviders";
            this.comboBoxTimestampProviders.Size = new System.Drawing.Size(173, 21);
            this.comboBoxTimestampProviders.TabIndex = 20;
            this.comboBoxTimestampProviders.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimestampProviders_SelectedIndexChanged);
            // 
            // groupBoxSignTool
            // 
            this.groupBoxSignTool.Controls.Add(this.label1);
            this.groupBoxSignTool.Controls.Add(this.buttonBrowseSignTool);
            this.groupBoxSignTool.Controls.Add(this.textBoxSignToolPath);
            this.groupBoxSignTool.Location = new System.Drawing.Point(7, 394);
            this.groupBoxSignTool.Name = "groupBoxSignTool";
            this.groupBoxSignTool.Size = new System.Drawing.Size(373, 47);
            this.groupBoxSignTool.TabIndex = 20;
            this.groupBoxSignTool.TabStop = false;
            this.groupBoxSignTool.Text = "Signtool";
            // 
            // groupBoxTrustedSigningMetadata
            // 
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.labelCorrelationId);
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.labelCertificateProfileName);
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.labelCodeSigningAccountName);
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.textBoxCorrelationId);
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.textBoxCertificateProfileName);
            this.groupBoxTrustedSigningMetadata.Controls.Add(this.textBoxCodeSigningAccountName);
            this.groupBoxTrustedSigningMetadata.Enabled = false;
            this.groupBoxTrustedSigningMetadata.Location = new System.Drawing.Point(7, 286);
            this.groupBoxTrustedSigningMetadata.Name = "groupBoxTrustedSigningMetadata";
            this.groupBoxTrustedSigningMetadata.Size = new System.Drawing.Size(373, 106);
            this.groupBoxTrustedSigningMetadata.TabIndex = 23;
            this.groupBoxTrustedSigningMetadata.TabStop = false;
            this.groupBoxTrustedSigningMetadata.Text = "Trusted Signing account";
            // 
            // labelCorrelationId
            // 
            this.labelCorrelationId.AutoSize = true;
            this.labelCorrelationId.Location = new System.Drawing.Point(6, 72);
            this.labelCorrelationId.Name = "labelCorrelationId";
            this.labelCorrelationId.Size = new System.Drawing.Size(69, 13);
            this.labelCorrelationId.TabIndex = 5;
            this.labelCorrelationId.Text = "CorrelationId:";
            // 
            // labelCertificateProfileName
            // 
            this.labelCertificateProfileName.AutoSize = true;
            this.labelCertificateProfileName.Location = new System.Drawing.Point(6, 46);
            this.labelCertificateProfileName.Name = "labelCertificateProfileName";
            this.labelCertificateProfileName.Size = new System.Drawing.Size(89, 13);
            this.labelCertificateProfileName.TabIndex = 4;
            this.labelCertificateProfileName.Text = "Certificate Profile:";
            // 
            // labelCodeSigningAccountName
            // 
            this.labelCodeSigningAccountName.AutoSize = true;
            this.labelCodeSigningAccountName.Location = new System.Drawing.Point(6, 20);
            this.labelCodeSigningAccountName.Name = "labelCodeSigningAccountName";
            this.labelCodeSigningAccountName.Size = new System.Drawing.Size(119, 13);
            this.labelCodeSigningAccountName.TabIndex = 3;
            this.labelCodeSigningAccountName.Text = "Signing Account Name:";
            // 
            // textBoxCorrelationId
            // 
            this.textBoxCorrelationId.Location = new System.Drawing.Point(127, 69);
            this.textBoxCorrelationId.Name = "textBoxCorrelationId";
            this.textBoxCorrelationId.Size = new System.Drawing.Size(240, 20);
            this.textBoxCorrelationId.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxCorrelationId, "(Optional) Enter a unique identifier to track and link related requests");
            // 
            // textBoxCertificateProfileName
            // 
            this.textBoxCertificateProfileName.Location = new System.Drawing.Point(127, 43);
            this.textBoxCertificateProfileName.Name = "textBoxCertificateProfileName";
            this.textBoxCertificateProfileName.Size = new System.Drawing.Size(240, 20);
            this.textBoxCertificateProfileName.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxCertificateProfileName, "The name of the Certificate Profile in the Trusted Signing Account to be used (re" +
        "quires the Trusted Signing Certificate Profile Signer role)");
            // 
            // textBoxCodeSigningAccountName
            // 
            this.textBoxCodeSigningAccountName.Location = new System.Drawing.Point(127, 17);
            this.textBoxCodeSigningAccountName.Name = "textBoxCodeSigningAccountName";
            this.textBoxCodeSigningAccountName.Size = new System.Drawing.Size(240, 20);
            this.textBoxCodeSigningAccountName.TabIndex = 0;
            this.toolTip.SetToolTip(this.textBoxCodeSigningAccountName, "The name of the Trusted Signing Account that will be used for signing");
            // 
            // radioButtonTrustedSigning
            // 
            this.radioButtonTrustedSigning.AutoSize = true;
            this.radioButtonTrustedSigning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTrustedSigning.Location = new System.Drawing.Point(15, 260);
            this.radioButtonTrustedSigning.Name = "radioButtonTrustedSigning";
            this.radioButtonTrustedSigning.Size = new System.Drawing.Size(114, 17);
            this.radioButtonTrustedSigning.TabIndex = 24;
            this.radioButtonTrustedSigning.Text = "Trusted Signing";
            this.radioButtonTrustedSigning.UseVisualStyleBackColor = true;
            this.radioButtonTrustedSigning.CheckedChanged += new System.EventHandler(this.RadioButtonSelectCertificateLocation_CheckedChanged);
            // 
            // labelSignedBuildState
            // 
            this.labelSignedBuildState.Location = new System.Drawing.Point(472, 2);
            this.labelSignedBuildState.Name = "labelSignedBuildState";
            this.labelSignedBuildState.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelSignedBuildState.Size = new System.Drawing.Size(209, 16);
            this.labelSignedBuildState.TabIndex = 25;
            this.labelSignedBuildState.Text = "SignedBuild";
            // 
            // linkLabelOpenTrustedSigningPortal
            // 
            this.linkLabelOpenTrustedSigningPortal.AutoSize = true;
            this.linkLabelOpenTrustedSigningPortal.Enabled = false;
            this.linkLabelOpenTrustedSigningPortal.Location = new System.Drawing.Point(135, 264);
            this.linkLabelOpenTrustedSigningPortal.Name = "linkLabelOpenTrustedSigningPortal";
            this.linkLabelOpenTrustedSigningPortal.Size = new System.Drawing.Size(176, 13);
            this.linkLabelOpenTrustedSigningPortal.TabIndex = 27;
            this.linkLabelOpenTrustedSigningPortal.TabStop = true;
            this.linkLabelOpenTrustedSigningPortal.Text = "Open Azure Portal (Trusted Signing)";
            this.toolTip.SetToolTip(this.linkLabelOpenTrustedSigningPortal, "Click here to open the Azure Portal, where you can see the diffrent Trusted Signi" +
        "ng accounts in your tenant you have.");
            this.linkLabelOpenTrustedSigningPortal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenTrustedSigningPortal_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Signtool.exe location:";
            // 
            // checkBoxTimestamp
            // 
            this.checkBoxTimestamp.AutoSize = true;
            this.checkBoxTimestamp.Checked = true;
            this.checkBoxTimestamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTimestamp.Location = new System.Drawing.Point(8, 73);
            this.checkBoxTimestamp.Name = "checkBoxTimestamp";
            this.checkBoxTimestamp.Size = new System.Drawing.Size(142, 17);
            this.checkBoxTimestamp.TabIndex = 22;
            this.checkBoxTimestamp.Text = "Timestamp when signing";
            this.checkBoxTimestamp.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(680, 859);
            this.Controls.Add(this.linkLabelOpenTrustedSigningPortal);
            this.Controls.Add(this.labelSignedBuildState);
            this.Controls.Add(this.radioButtonTrustedSigning);
            this.Controls.Add(this.radioButtonPFXCertificate);
            this.Controls.Add(this.groupBoxSignTool);
            this.Controls.Add(this.groupBoxWindowsCertificateStore);
            this.Controls.Add(this.groupBoxTrustedSigningMetadata);
            this.Controls.Add(this.groupBoxTimestamp);
            this.Controls.Add(this.buttonSaveLog);
            this.Controls.Add(this.groupBoxPFXCertificate);
            this.Controls.Add(this.buttonShowAllCertDataPopup);
            this.Controls.Add(this.splitButtonSign);
            this.Controls.Add(this.radioButtonWindowsCertificateStore);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.labelSelectTheDigitalCertificate);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.groupBoxCertificateInformation);
            this.Controls.Add(this.groupBoxFiles);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Signtool GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxFiles.ResumeLayout(false);
            this.groupBoxFiles.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxCertificateInformation.ResumeLayout(false);
            this.groupBoxPFXCertificate.ResumeLayout(false);
            this.groupBoxPFXCertificate.PerformLayout();
            this.groupBoxWindowsCertificateStore.ResumeLayout(false);
            this.groupBoxWindowsCertificateStore.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.groupBoxTimestamp.ResumeLayout(false);
            this.groupBoxTimestamp.PerformLayout();
            this.groupBoxSignTool.ResumeLayout(false);
            this.groupBoxSignTool.PerformLayout();
            this.groupBoxTrustedSigningMetadata.ResumeLayout(false);
            this.groupBoxTrustedSigningMetadata.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxSignToolPath;
        private System.Windows.Forms.Button buttonBrowseSignTool;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelTimeStampServer;
        private System.Windows.Forms.GroupBox groupBoxFiles;
        private System.Windows.Forms.Button buttonAddDirectory;
        private System.Windows.Forms.Button buttonAddFiles;
        private System.Windows.Forms.CheckedListBox checkedListBoxFiles;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button buttonClear;
        private wyDay.Controls.SplitButton splitButtonSign;
        private System.Windows.Forms.ContextMenu contextMenuSignSwitches;
        private System.Windows.Forms.MenuItem menuItemSignVerbose;
        private System.Windows.Forms.MenuItem menuItemSignDebug;
        private System.Windows.Forms.CheckBox checkBoxSubdirectories;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.CheckBox checkBoxShowOutput;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.Label labelCertificateInformation;
        private System.Windows.Forms.ComboBox comboBoxCertificateStore;
        private System.Windows.Forms.Label labelCertificateStore;
        private System.Windows.Forms.CheckBox checkBoxShowExpiredCertificates;
        private System.Windows.Forms.ComboBox comboBoxCertificatesInStore;
        private System.Windows.Forms.Button buttonShowSigninigCertificateStore;
        private System.Windows.Forms.GroupBox groupBoxCertificateInformation;
        private System.Windows.Forms.Label labelSelectTheDigitalCertificate;
        private System.Windows.Forms.Label PFXFilePassword;
        private System.Windows.Forms.Button buttonSelectPFXCertificate;
        private System.Windows.Forms.TextBox textBoxPFXPassword;
        private System.Windows.Forms.TextBox textBoxPFXFile;
        private System.Windows.Forms.RadioButton radioButtonPFXCertificate;
        private System.Windows.Forms.RadioButton radioButtonWindowsCertificateStore;
        private System.Windows.Forms.GroupBox groupBoxPFXCertificate;
        private System.Windows.Forms.Button buttonShowSigninigCertificatePFX;
        private System.Windows.Forms.GroupBox groupBoxWindowsCertificateStore;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button ResetJob;
        private System.Windows.Forms.Button buttonSaveLog;
        private System.Windows.Forms.Button buttonShowAllCertDataPopup;
        private System.Windows.Forms.GroupBox groupBoxTimestamp;
        private System.Windows.Forms.TextBox txtTimestampProviderURL;
        private System.Windows.Forms.Label labelTimestampProvider;
        private System.Windows.Forms.ComboBox comboBoxTimestampProviders;
        private System.Windows.Forms.GroupBox groupBoxSignTool;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxTrustedSigningMetadata;
        private System.Windows.Forms.Label labelCodeSigningAccountName;
        private System.Windows.Forms.TextBox textBoxCorrelationId;
        private System.Windows.Forms.TextBox textBoxCertificateProfileName;
        private System.Windows.Forms.TextBox textBoxCodeSigningAccountName;
        private System.Windows.Forms.Label labelCorrelationId;
        private System.Windows.Forms.Label labelCertificateProfileName;
        private System.Windows.Forms.RadioButton radioButtonTrustedSigning;
        private System.Windows.Forms.Label labelSignedBuildState;
        private System.Windows.Forms.LinkLabel linkLabelOpenTrustedSigningPortal;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxTimestamp;
    }
}

