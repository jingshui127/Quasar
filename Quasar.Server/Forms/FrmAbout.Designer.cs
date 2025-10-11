﻿namespace Quasar.Server.Forms
{
    partial class FrmAbout
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源为 true；否则为 false。</param>
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
        /// 设计器支持所需的方法 - 不要修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.rtxtContent = new System.Windows.Forms.RichTextBox();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lnkGithubPage = new System.Windows.Forms.LinkLabel();
            this.lnkCredits = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Image = global::Quasar.Server.Properties.Resources.远程控制;
            this.picIcon.Location = new System.Drawing.Point(10, 10);
            this.picIcon.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(167, 167);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 0;
            this.picIcon.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.MediumOrchid;
            this.lblTitle.Location = new System.Drawing.Point(262, 10);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 71);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "科控物联";
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(867, 43);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(229, 32);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "%VERSION%";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOkay.Location = new System.Drawing.Point(922, 512);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(188, 58);
            this.btnOkay.TabIndex = 7;
            this.btnOkay.Text = "确定(&O)";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // rtxtContent
            // 
            this.rtxtContent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtContent.Location = new System.Drawing.Point(38, 218);
            this.rtxtContent.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.rtxtContent.Name = "rtxtContent";
            this.rtxtContent.ReadOnly = true;
            this.rtxtContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxtContent.Size = new System.Drawing.Size(1134, 277);
            this.rtxtContent.TabIndex = 6;
            this.rtxtContent.Text = resources.GetString("rtxtContent.Text");
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTitle.Location = new System.Drawing.Point(265, 107);
            this.lblSubTitle.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(230, 45);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "远程管理工具";
            // 
            // lnkGithubPage
            // 
            this.lnkGithubPage.AutoSize = true;
            this.lnkGithubPage.Location = new System.Drawing.Point(900, 92);
            this.lnkGithubPage.Name = "lnkGithubPage";
            this.lnkGithubPage.Size = new System.Drawing.Size(133, 38);
            this.lnkGithubPage.TabIndex = 8;
            this.lnkGithubPage.TabStop = true;
            this.lnkGithubPage.Text = "使用帮助";
            this.lnkGithubPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGithubPage_LinkClicked);
            // 
            // lnkCredits
            // 
            this.lnkCredits.AutoSize = true;
            this.lnkCredits.Location = new System.Drawing.Point(23, 523);
            this.lnkCredits.Name = "lnkCredits";
            this.lnkCredits.Size = new System.Drawing.Size(133, 38);
            this.lnkCredits.TabIndex = 9;
            this.lnkCredits.TabStop = true;
            this.lnkCredits.Text = "其他许可";
            // 
            // FrmAbout
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(1182, 587);
            this.Controls.Add(this.lnkCredits);
            this.Controls.Add(this.lblSubTitle);
            this.Controls.Add(this.rtxtContent);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.lnkGithubPage);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "科控物联远程协助助手 - 关于";
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.RichTextBox rtxtContent;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.LinkLabel lnkGithubPage;
        private System.Windows.Forms.LinkLabel lnkCredits;
    }
}