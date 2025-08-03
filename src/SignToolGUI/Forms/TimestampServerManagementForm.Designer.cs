namespace SignToolGUI.Forms
{
    partial class TimestampServerManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimestampServerManagementForm));
            this.listViewServers = new System.Windows.Forms.ListView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonTestAll = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.groupBoxServers = new System.Windows.Forms.GroupBox();
            this.groupBoxActions = new System.Windows.Forms.GroupBox();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.columnPriority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTimeout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxServers.SuspendLayout();
            this.groupBoxActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewServers
            // 
            this.listViewServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPriority,
            this.columnDisplayName,
            this.columnURL,
            this.columnEnabled,
            this.columnTimeout});
            this.listViewServers.FullRowSelect = true;
            this.listViewServers.GridLines = true;
            this.listViewServers.HideSelection = false;
            this.listViewServers.Location = new System.Drawing.Point(8, 19);
            this.listViewServers.MultiSelect = false;
            this.listViewServers.Name = "listViewServers";
            this.listViewServers.Size = new System.Drawing.Size(549, 363);
            this.listViewServers.TabIndex = 0;
            this.listViewServers.UseCompatibleStateImageBehavior = false;
            this.listViewServers.View = System.Windows.Forms.View.Details;
            this.listViewServers.SelectedIndexChanged += new System.EventHandler(this.ListViewServers_SelectedIndexChanged);
            this.listViewServers.DoubleClick += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(6, 19);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(6, 48);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(6, 77);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(6, 130);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveUp.TabIndex = 4;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(6, 159);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(75, 23);
            this.buttonMoveDown.TabIndex = 5;
            this.buttonMoveDown.Text = "Move down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(6, 188);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 6;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.ButtonTest_Click);
            // 
            // buttonTestAll
            // 
            this.buttonTestAll.Location = new System.Drawing.Point(6, 217);
            this.buttonTestAll.Name = "buttonTestAll";
            this.buttonTestAll.Size = new System.Drawing.Size(75, 23);
            this.buttonTestAll.TabIndex = 7;
            this.buttonTestAll.Text = "Test all";
            this.buttonTestAll.UseVisualStyleBackColor = true;
            this.buttonTestAll.Click += new System.EventHandler(this.ButtonTestAll_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(6, 259);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 8;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(6, 325);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(6, 287);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.Enabled = false;
            this.buttonApply.Location = new System.Drawing.Point(6, 354);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 11;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ButtonApply_Click);
            // 
            // groupBoxServers
            // 
            this.groupBoxServers.Controls.Add(this.listViewServers);
            this.groupBoxServers.Location = new System.Drawing.Point(12, 12);
            this.groupBoxServers.Name = "groupBoxServers";
            this.groupBoxServers.Size = new System.Drawing.Size(564, 390);
            this.groupBoxServers.TabIndex = 12;
            this.groupBoxServers.TabStop = false;
            this.groupBoxServers.Text = "Timestamp Servers";
            // 
            // groupBoxActions
            // 
            this.groupBoxActions.Controls.Add(this.buttonAdd);
            this.groupBoxActions.Controls.Add(this.buttonEdit);
            this.groupBoxActions.Controls.Add(this.buttonTestAll);
            this.groupBoxActions.Controls.Add(this.buttonReset);
            this.groupBoxActions.Controls.Add(this.buttonTest);
            this.groupBoxActions.Controls.Add(this.buttonCancel);
            this.groupBoxActions.Controls.Add(this.buttonApply);
            this.groupBoxActions.Controls.Add(this.buttonRemove);
            this.groupBoxActions.Controls.Add(this.buttonMoveUp);
            this.groupBoxActions.Controls.Add(this.buttonOK);
            this.groupBoxActions.Controls.Add(this.buttonMoveDown);
            this.groupBoxActions.Location = new System.Drawing.Point(581, 12);
            this.groupBoxActions.Name = "groupBoxActions";
            this.groupBoxActions.Size = new System.Drawing.Size(88, 390);
            this.groupBoxActions.TabIndex = 13;
            this.groupBoxActions.TabStop = false;
            this.groupBoxActions.Text = "Actions";
            // 
            // labelInstructions
            // 
            this.labelInstructions.AutoSize = true;
            this.labelInstructions.Location = new System.Drawing.Point(211, 412);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(465, 13);
            this.labelInstructions.TabIndex = 14;
            this.labelInstructions.Text = "Manage timestamp servers and their priorities. Higher priority servers (lower num" +
    "bers) are tried first.";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 408);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(196, 20);
            this.progressBar.TabIndex = 15;
            this.progressBar.Visible = false;
            // 
            // columnPriority
            // 
            this.columnPriority.Text = "Priority";
            // 
            // columnDisplayName
            // 
            this.columnDisplayName.Text = "Display name";
            this.columnDisplayName.Width = 150;
            // 
            // columnURL
            // 
            this.columnURL.Text = "URL";
            this.columnURL.Width = 200;
            // 
            // columnEnabled
            // 
            this.columnEnabled.Text = "Enabled";
            // 
            // columnTimeout
            // 
            this.columnTimeout.Text = "Timeout";
            this.columnTimeout.Width = 70;
            // 
            // TimestampServerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(677, 439);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelInstructions);
            this.Controls.Add(this.groupBoxServers);
            this.Controls.Add(this.groupBoxActions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimestampServerManagementForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Timestamp Server Management";
            this.groupBoxServers.ResumeLayout(false);
            this.groupBoxActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewServers;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonTestAll;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.GroupBox groupBoxServers;
        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Label labelInstructions;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ColumnHeader columnPriority;
        private System.Windows.Forms.ColumnHeader columnDisplayName;
        private System.Windows.Forms.ColumnHeader columnURL;
        private System.Windows.Forms.ColumnHeader columnEnabled;
        private System.Windows.Forms.ColumnHeader columnTimeout;
    }
}