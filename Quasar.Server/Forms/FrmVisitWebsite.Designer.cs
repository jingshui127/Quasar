﻿﻿namespace Quasar.Server.Forms
{
    partial class FrmVisitWebsite
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVisitWebsite));
            this.chkVisitHidden = new System.Windows.Forms.CheckBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnVisitWebsite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkVisitHidden
            // 
            this.chkVisitHidden.AutoSize = true;
            this.chkVisitHidden.Checked = true;
            this.chkVisitHidden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVisitHidden.Location = new System.Drawing.Point(48, 38);
            this.chkVisitHidden.Name = "chkVisitHidden";
            this.chkVisitHidden.Size = new System.Drawing.Size(170, 17);
            this.chkVisitHidden.TabIndex = 2;
            this.chkVisitHidden.Text = "隐藏访问（推荐）";
            this.chkVisitHidden.UseVisualStyleBackColor = true;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(12, 9);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(30, 13);
            this.lblURL.TabIndex = 0;
            this.lblURL.Text = "URL:";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(48, 6);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(336, 22);
            this.txtURL.TabIndex = 1;
            // 
            // btnVisitWebsite
            // 
            this.btnVisitWebsite.Location = new System.Drawing.Point(246, 34);
            this.btnVisitWebsite.Name = "btnVisitWebsite";
            this.btnVisitWebsite.Size = new System.Drawing.Size(138, 23);
            this.btnVisitWebsite.TabIndex = 3;
            this.btnVisitWebsite.Text = "访问网站";
            this.btnVisitWebsite.UseVisualStyleBackColor = true;
            this.btnVisitWebsite.Click += new System.EventHandler(this.btnVisitWebsite_Click);
            // 
            // FrmVisitWebsite
            // 
            this.AcceptButton = this.btnVisitWebsite;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(396, 72);
            this.Controls.Add(this.chkVisitHidden);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.btnVisitWebsite);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVisitWebsite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "访问网站 []";
            this.Load += new System.EventHandler(this.FrmVisitWebsite_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkVisitHidden;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnVisitWebsite;
    }
}