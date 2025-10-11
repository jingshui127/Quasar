﻿﻿﻿﻿﻿﻿﻿﻿﻿namespace Quasar.Server.Forms
{
    partial class FrmSettings
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.btnSave = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.ncPort = new System.Windows.Forms.NumericUpDown();
            this.chkAutoListen = new System.Windows.Forms.CheckBox();
            this.chkPopup = new System.Windows.Forms.CheckBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkUseUpnp = new System.Windows.Forms.CheckBox();
            this.chkShowTooltip = new System.Windows.Forms.CheckBox();
            this.chkNoIPIntegration = new System.Windows.Forms.CheckBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtNoIPPass = new System.Windows.Forms.TextBox();
            this.txtNoIPUser = new System.Windows.Forms.TextBox();
            this.txtNoIPHost = new System.Windows.Forms.TextBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.chkIPv6Support = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ncPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(340, 447);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 34);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(18, 16);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(82, 23);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "监听端口:";
            // 
            // ncPort
            // 
            this.ncPort.Location = new System.Drawing.Point(166, 10);
            this.ncPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ncPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ncPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ncPort.Name = "ncPort";
            this.ncPort.Size = new System.Drawing.Size(112, 29);
            this.ncPort.TabIndex = 1;
            this.ncPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAutoListen
            // 
            this.chkAutoListen.AutoSize = true;
            this.chkAutoListen.Location = new System.Drawing.Point(18, 102);
            this.chkAutoListen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoListen.Name = "chkAutoListen";
            this.chkAutoListen.Size = new System.Drawing.Size(172, 27);
            this.chkAutoListen.TabIndex = 6;
            this.chkAutoListen.Text = "启动时监听新连接";
            this.chkAutoListen.UseVisualStyleBackColor = true;
            // 
            // chkPopup
            // 
            this.chkPopup.AutoSize = true;
            this.chkPopup.Location = new System.Drawing.Point(18, 136);
            this.chkPopup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPopup.Name = "chkPopup";
            this.chkPopup.Size = new System.Drawing.Size(206, 27);
            this.chkPopup.TabIndex = 7;
            this.chkPopup.Text = "新连接时显示弹出通知";
            this.chkPopup.UseVisualStyleBackColor = true;
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(288, 9);
            this.btnListen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(165, 34);
            this.btnListen.TabIndex = 2;
            this.btnListen.Text = "开始监听";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(219, 447);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 34);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkUseUpnp
            // 
            this.chkUseUpnp.AutoSize = true;
            this.chkUseUpnp.Location = new System.Drawing.Point(18, 171);
            this.chkUseUpnp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseUpnp.Name = "chkUseUpnp";
            this.chkUseUpnp.Size = new System.Drawing.Size(229, 27);
            this.chkUseUpnp.TabIndex = 8;
            this.chkUseUpnp.Text = "尝试自动转发端口 (UPnP)";
            this.chkUseUpnp.UseVisualStyleBackColor = true;
            // 
            // chkShowTooltip
            // 
            this.chkShowTooltip.AutoSize = true;
            this.chkShowTooltip.Location = new System.Drawing.Point(18, 206);
            this.chkShowTooltip.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowTooltip.Name = "chkShowTooltip";
            this.chkShowTooltip.Size = new System.Drawing.Size(291, 27);
            this.chkShowTooltip.TabIndex = 9;
            this.chkShowTooltip.Text = "在客户端上显示系统信息工具提示";
            this.chkShowTooltip.UseVisualStyleBackColor = true;
            // 
            // chkNoIPIntegration
            // 
            this.chkNoIPIntegration.AutoSize = true;
            this.chkNoIPIntegration.Location = new System.Drawing.Point(18, 266);
            this.chkNoIPIntegration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkNoIPIntegration.Name = "chkNoIPIntegration";
            this.chkNoIPIntegration.Size = new System.Drawing.Size(252, 27);
            this.chkNoIPIntegration.TabIndex = 10;
            this.chkNoIPIntegration.Text = "启用 No-Ip.com DNS 更新器";
            this.chkNoIPIntegration.UseVisualStyleBackColor = true;
            this.chkNoIPIntegration.CheckedChanged += new System.EventHandler(this.chkNoIPIntegration_CheckedChanged);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Enabled = false;
            this.lblHost.Location = new System.Drawing.Point(45, 304);
            this.lblHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(48, 23);
            this.lblHost.TabIndex = 11;
            this.lblHost.Text = "主机:";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Enabled = false;
            this.lblPass.Location = new System.Drawing.Point(250, 346);
            this.lblPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(48, 23);
            this.lblPass.TabIndex = 15;
            this.lblPass.Text = "密码:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Enabled = false;
            this.lblUser.Location = new System.Drawing.Point(45, 346);
            this.lblUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(48, 23);
            this.lblUser.TabIndex = 13;
            this.lblUser.Text = "邮箱:";
            // 
            // txtNoIPPass
            // 
            this.txtNoIPPass.Enabled = false;
            this.txtNoIPPass.Location = new System.Drawing.Point(298, 342);
            this.txtNoIPPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNoIPPass.Name = "txtNoIPPass";
            this.txtNoIPPass.Size = new System.Drawing.Size(148, 29);
            this.txtNoIPPass.TabIndex = 16;
            // 
            // txtNoIPUser
            // 
            this.txtNoIPUser.Enabled = false;
            this.txtNoIPUser.Location = new System.Drawing.Point(105, 342);
            this.txtNoIPUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNoIPUser.Name = "txtNoIPUser";
            this.txtNoIPUser.Size = new System.Drawing.Size(134, 29);
            this.txtNoIPUser.TabIndex = 14;
            // 
            // txtNoIPHost
            // 
            this.txtNoIPHost.Enabled = false;
            this.txtNoIPHost.Location = new System.Drawing.Point(105, 300);
            this.txtNoIPHost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNoIPHost.Name = "txtNoIPHost";
            this.txtNoIPHost.Size = new System.Drawing.Size(342, 29);
            this.txtNoIPHost.TabIndex = 12;
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Enabled = false;
            this.chkShowPassword.Location = new System.Drawing.Point(288, 384);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(104, 27);
            this.chkShowPassword.TabIndex = 17;
            this.chkShowPassword.Text = "显示密码";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // chkIPv6Support
            // 
            this.chkIPv6Support.AutoSize = true;
            this.chkIPv6Support.Location = new System.Drawing.Point(18, 68);
            this.chkIPv6Support.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIPv6Support.Name = "chkIPv6Support";
            this.chkIPv6Support.Size = new System.Drawing.Size(146, 27);
            this.chkIPv6Support.TabIndex = 5;
            this.chkIPv6Support.Text = "启用 IPv6 支持";
            this.chkIPv6Support.UseVisualStyleBackColor = true;
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(471, 500);
            this.Controls.Add(this.chkIPv6Support);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.txtNoIPHost);
            this.Controls.Add(this.txtNoIPUser);
            this.Controls.Add(this.txtNoIPPass);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.chkNoIPIntegration);
            this.Controls.Add(this.chkShowTooltip);
            this.Controls.Add(this.chkUseUpnp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.chkPopup);
            this.Controls.Add(this.chkAutoListen);
            this.Controls.Add(this.ncPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ncPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown ncPort;
        private System.Windows.Forms.CheckBox chkAutoListen;
        private System.Windows.Forms.CheckBox chkPopup;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkUseUpnp;
        private System.Windows.Forms.CheckBox chkShowTooltip;
        private System.Windows.Forms.CheckBox chkNoIPIntegration;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtNoIPPass;
        private System.Windows.Forms.TextBox txtNoIPUser;
        private System.Windows.Forms.TextBox txtNoIPHost;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.CheckBox chkIPv6Support;
    }
}