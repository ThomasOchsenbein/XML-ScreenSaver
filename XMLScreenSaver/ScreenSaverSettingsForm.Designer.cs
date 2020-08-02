namespace XMLScreenSaver
{
    partial class ScreenSaverSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenSaverSettingsForm));
            this.picTopLeft = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDownloadSampleXML = new System.Windows.Forms.Button();
            this.btnAboutInstructions = new System.Windows.Forms.Button();
            this.tlyForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlyHeading = new System.Windows.Forms.TableLayoutPanel();
            this.tbcAboutInstructions = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtLicense = new System.Windows.Forms.TextBox();
            this.lblLicenseHeader = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picTopLeft)).BeginInit();
            this.tlyForm.SuspendLayout();
            this.tlyHeading.SuspendLayout();
            this.tbcAboutInstructions.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picTopLeft
            // 
            this.picTopLeft.BackColor = System.Drawing.Color.Transparent;
            this.picTopLeft.Image = ((System.Drawing.Image)(resources.GetObject("picTopLeft.Image")));
            this.picTopLeft.Location = new System.Drawing.Point(0, 0);
            this.picTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.picTopLeft.Name = "picTopLeft";
            this.tlyHeading.SetRowSpan(this.picTopLeft, 3);
            this.picTopLeft.Size = new System.Drawing.Size(128, 128);
            this.picTopLeft.TabIndex = 0;
            this.picTopLeft.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Trebuchet MS", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(163, 23);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(411, 61);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "XML ScreenSaver";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.Location = new System.Drawing.Point(163, 99);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(3);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCopyright.Size = new System.Drawing.Size(303, 22);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "Copyright © 2020 Thomas Ochsenbein";
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.BackColor = System.Drawing.Color.Transparent;
            this.lblInstructions.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.Location = new System.Drawing.Point(3, 3);
            this.lblInstructions.Margin = new System.Windows.Forms.Padding(3);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(490, 16);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "Instructions will go here. Extra text for helping create the layout - needs to sp" +
    "an the 3 columns.";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Location = new System.Drawing.Point(565, 432);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(128, 22);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDownloadSampleXML
            // 
            this.btnDownloadSampleXML.BackColor = System.Drawing.Color.Transparent;
            this.btnDownloadSampleXML.Location = new System.Drawing.Point(11, 432);
            this.btnDownloadSampleXML.Name = "btnDownloadSampleXML";
            this.btnDownloadSampleXML.Size = new System.Drawing.Size(128, 22);
            this.btnDownloadSampleXML.TabIndex = 2;
            this.btnDownloadSampleXML.Text = "Download Sample XML";
            this.btnDownloadSampleXML.UseVisualStyleBackColor = false;
            this.btnDownloadSampleXML.Click += new System.EventHandler(this.btnDownloadSampleXML_Click);
            // 
            // btnAboutInstructions
            // 
            this.btnAboutInstructions.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAboutInstructions.BackColor = System.Drawing.Color.Transparent;
            this.btnAboutInstructions.Location = new System.Drawing.Point(287, 432);
            this.btnAboutInstructions.Name = "btnAboutInstructions";
            this.btnAboutInstructions.Size = new System.Drawing.Size(128, 22);
            this.btnAboutInstructions.TabIndex = 3;
            this.btnAboutInstructions.Text = "About";
            this.btnAboutInstructions.UseVisualStyleBackColor = false;
            this.btnAboutInstructions.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // tlyForm
            // 
            this.tlyForm.ColumnCount = 3;
            this.tlyForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tlyForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.4F));
            this.tlyForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tlyForm.Controls.Add(this.btnDownloadSampleXML, 0, 3);
            this.tlyForm.Controls.Add(this.btnAboutInstructions, 1, 3);
            this.tlyForm.Controls.Add(this.btnOK, 2, 3);
            this.tlyForm.Controls.Add(this.tlyHeading, 0, 0);
            this.tlyForm.Controls.Add(this.tbcAboutInstructions, 0, 1);
            this.tlyForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlyForm.Location = new System.Drawing.Point(0, 0);
            this.tlyForm.Name = "tlyForm";
            this.tlyForm.Padding = new System.Windows.Forms.Padding(8);
            this.tlyForm.RowCount = 4;
            this.tlyForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tlyForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlyForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlyForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlyForm.Size = new System.Drawing.Size(704, 465);
            this.tlyForm.TabIndex = 0;
            // 
            // tlyHeading
            // 
            this.tlyHeading.ColumnCount = 2;
            this.tlyForm.SetColumnSpan(this.tlyHeading, 3);
            this.tlyHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tlyHeading.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlyHeading.Controls.Add(this.picTopLeft, 0, 0);
            this.tlyHeading.Controls.Add(this.lblTitle, 1, 1);
            this.tlyHeading.Controls.Add(this.lblCopyright, 1, 2);
            this.tlyHeading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlyHeading.Location = new System.Drawing.Point(11, 11);
            this.tlyHeading.Name = "tlyHeading";
            this.tlyHeading.RowCount = 3;
            this.tlyHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlyHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlyHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlyHeading.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlyHeading.Size = new System.Drawing.Size(682, 138);
            this.tlyHeading.TabIndex = 0;
            // 
            // tbcAboutInstructions
            // 
            this.tbcAboutInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcAboutInstructions.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tlyForm.SetColumnSpan(this.tbcAboutInstructions, 3);
            this.tbcAboutInstructions.Controls.Add(this.tabPage1);
            this.tbcAboutInstructions.Controls.Add(this.tabPage2);
            this.tbcAboutInstructions.ItemSize = new System.Drawing.Size(0, 1);
            this.tbcAboutInstructions.Location = new System.Drawing.Point(11, 155);
            this.tbcAboutInstructions.Name = "tbcAboutInstructions";
            this.tbcAboutInstructions.SelectedIndex = 0;
            this.tbcAboutInstructions.Size = new System.Drawing.Size(682, 263);
            this.tbcAboutInstructions.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbcAboutInstructions.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblInstructions);
            this.tabPage1.Location = new System.Drawing.Point(4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(674, 254);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtLicense);
            this.tabPage2.Controls.Add(this.lblLicenseHeader);
            this.tabPage2.Controls.Add(this.lblVersion);
            this.tabPage2.Location = new System.Drawing.Point(4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(674, 254);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtLicense
            // 
            this.txtLicense.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicense.Location = new System.Drawing.Point(6, 63);
            this.txtLicense.Multiline = true;
            this.txtLicense.Name = "txtLicense";
            this.txtLicense.ReadOnly = true;
            this.txtLicense.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLicense.Size = new System.Drawing.Size(662, 185);
            this.txtLicense.TabIndex = 0;
            this.txtLicense.TabStop = false;
            this.txtLicense.Text = resources.GetString("txtLicense.Text");
            // 
            // lblLicenseHeader
            // 
            this.lblLicenseHeader.AutoSize = true;
            this.lblLicenseHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblLicenseHeader.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicenseHeader.Location = new System.Drawing.Point(3, 40);
            this.lblLicenseHeader.Margin = new System.Windows.Forms.Padding(3);
            this.lblLicenseHeader.Name = "lblLicenseHeader";
            this.lblLicenseHeader.Size = new System.Drawing.Size(459, 16);
            this.lblLicenseHeader.TabIndex = 0;
            this.lblLicenseHeader.Text = "Use of XML ScreenSaver is licensed under GPL 3.0. Please see full text of the lic" +
    "ense below.";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(3, 3);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(505, 16);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Version number will go here. Extra text for helping create the layout - needs to " +
    "span the 3 columns.";
            // 
            // ScreenSaverSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(704, 465);
            this.Controls.Add(this.tlyForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ScreenSaverSettingsForm";
            this.Text = "XML ScreenSaver";
            this.Load += new System.EventHandler(this.ScreenSaverSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picTopLeft)).EndInit();
            this.tlyForm.ResumeLayout(false);
            this.tlyHeading.ResumeLayout(false);
            this.tlyHeading.PerformLayout();
            this.tbcAboutInstructions.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTopLeft;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAboutInstructions;
        private System.Windows.Forms.Button btnDownloadSampleXML;
        private System.Windows.Forms.TableLayoutPanel tlyForm;
        private System.Windows.Forms.TableLayoutPanel tlyHeading;
        private System.Windows.Forms.TabControl tbcAboutInstructions;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtLicense;
        private System.Windows.Forms.Label lblLicenseHeader;
        private System.Windows.Forms.Label lblVersion;
    }
}