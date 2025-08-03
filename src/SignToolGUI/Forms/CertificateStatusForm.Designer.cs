using System.Drawing;
using System.Windows.Forms;

namespace SignToolGUI.Forms
{
    partial class CertificateStatusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CertificateStatusForm));
            this.listViewCertificates = new System.Windows.Forms.ListView();
            this.columnStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCertificateName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIssuer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnExpires = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDaysUntilExpire = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnThumprint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewCertificates
            // 
            this.listViewCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCertificates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnStatus,
            this.columnCertificateName,
            this.columnIssuer,
            this.columnExpires,
            this.columnDaysUntilExpire,
            this.columnThumprint});
            this.listViewCertificates.FullRowSelect = true;
            this.listViewCertificates.GridLines = true;
            this.listViewCertificates.HideSelection = false;
            this.listViewCertificates.Location = new System.Drawing.Point(12, 12);
            this.listViewCertificates.Name = "listViewCertificates";
            this.listViewCertificates.Size = new System.Drawing.Size(760, 350);
            this.listViewCertificates.TabIndex = 0;
            this.listViewCertificates.UseCompatibleStateImageBehavior = false;
            this.listViewCertificates.View = System.Windows.Forms.View.Details;
            // 
            // columnStatus
            // 
            this.columnStatus.Text = "Status";
            this.columnStatus.Width = 80;
            // 
            // columnCertificateName
            // 
            this.columnCertificateName.Text = "Certificate Name";
            this.columnCertificateName.Width = 200;
            // 
            // columnIssuer
            // 
            this.columnIssuer.Text = "Issuer";
            this.columnIssuer.Width = 200;
            // 
            // columnExpires
            // 
            this.columnExpires.Text = "ExpireDate";
            this.columnExpires.Width = 100;
            // 
            // columnDaysUntilExpire
            // 
            this.columnDaysUntilExpire.Text = "Daus Until Expiry";
            this.columnDaysUntilExpire.Width = 120;
            // 
            // columnThumprint
            // 
            this.columnThumprint.Text = "Thumprint";
            this.columnThumprint.Width = 160;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(616, 375);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh list";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(697, 375);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CertificateStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 410);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.listViewCertificates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificateStatusForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Certificate Status & Expiry Monitor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCertificates;
        private ColumnHeader columnStatus;
        private ColumnHeader columnCertificateName;
        private ColumnHeader columnIssuer;
        private ColumnHeader columnExpires;
        private ColumnHeader columnDaysUntilExpire;
        private ColumnHeader columnThumprint;
        private Button buttonRefresh;
        private Button buttonClose;
    }
}