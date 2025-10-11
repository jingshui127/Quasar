﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using Quasar.Server.Controls;

namespace Quasar.Server.Forms
{
    partial class FrmBuilder
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBuilder));
            this.btnBuild = new System.Windows.Forms.Button();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.tooltip.BackColor = System.Drawing.Color.LightYellow;
            this.picUAC2 = new System.Windows.Forms.PictureBox();
            this.picUAC1 = new System.Windows.Forms.PictureBox();
            this.rbSystem = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeHostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.builderTabs = new Quasar.Server.Controls.DotNetBarTabControl();
            this.generalPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUnattendedMode = new System.Windows.Forms.CheckBox();
            this.line2 = new Quasar.Server.Controls.Line();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.line6 = new Quasar.Server.Controls.Line();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTag = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTag = new System.Windows.Forms.Label();
            this.txtMutex = new System.Windows.Forms.TextBox();
            this.btnMutex = new System.Windows.Forms.Button();
            this.line5 = new Quasar.Server.Controls.Line();
            this.lblMutex = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionPage = new System.Windows.Forms.TabPage();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.line3 = new Quasar.Server.Controls.Line();
            this.label4 = new System.Windows.Forms.Label();
            this.line1 = new Quasar.Server.Controls.Line();
            this.label1 = new System.Windows.Forms.Label();
            this.lstHosts = new System.Windows.Forms.ListBox();
            this.btnAddHost = new System.Windows.Forms.Button();
            this.lblMS = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.installationPage = new System.Windows.Forms.TabPage();
            this.chkHideSubDirectory = new System.Windows.Forms.CheckBox();
            this.line7 = new Quasar.Server.Controls.Line();
            this.label10 = new System.Windows.Forms.Label();
            this.line4 = new Quasar.Server.Controls.Line();
            this.label5 = new System.Windows.Forms.Label();
            this.chkInstall = new System.Windows.Forms.CheckBox();
            this.lblInstallName = new System.Windows.Forms.Label();
            this.rbProgramFiles = new System.Windows.Forms.RadioButton();
            this.txtInstallName = new System.Windows.Forms.TextBox();
            this.txtRegistryKeyName = new System.Windows.Forms.TextBox();
            this.lblExtension = new System.Windows.Forms.Label();
            this.lblRegistryKeyName = new System.Windows.Forms.Label();
            this.chkStartup = new System.Windows.Forms.CheckBox();
            this.rbAppdata = new System.Windows.Forms.RadioButton();
            this.chkHide = new System.Windows.Forms.CheckBox();
            this.lblInstallDirectory = new System.Windows.Forms.Label();
            this.lblInstallSubDirectory = new System.Windows.Forms.Label();
            this.lblPreviewPath = new System.Windows.Forms.Label();
            this.txtInstallSubDirectory = new System.Windows.Forms.TextBox();
            this.txtPreviewPath = new System.Windows.Forms.TextBox();
            this.assemblyPage = new System.Windows.Forms.TabPage();
            this.iconPreview = new System.Windows.Forms.PictureBox();
            this.btnBrowseIcon = new System.Windows.Forms.Button();
            this.txtIconPath = new System.Windows.Forms.TextBox();
            this.line8 = new Quasar.Server.Controls.Line();
            this.label11 = new System.Windows.Forms.Label();
            this.chkChangeAsmInfo = new System.Windows.Forms.CheckBox();
            this.txtFileVersion = new System.Windows.Forms.TextBox();
            this.line9 = new Quasar.Server.Controls.Line();
            this.lblProductName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkChangeIcon = new System.Windows.Forms.CheckBox();
            this.lblFileVersion = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtProductVersion = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtOriginalFilename = new System.Windows.Forms.TextBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblOriginalFilename = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtTrademarks = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblTrademarks = new System.Windows.Forms.Label();
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.monitoringTab = new System.Windows.Forms.TabPage();
            this.chkHideLogDirectory = new System.Windows.Forms.CheckBox();
            this.txtLogDirectoryName = new System.Windows.Forms.TextBox();
            this.lblLogDirectory = new System.Windows.Forms.Label();
            this.line10 = new Quasar.Server.Controls.Line();
            this.label14 = new System.Windows.Forms.Label();
            this.chkKeylogger = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.builderTabs.SuspendLayout();
            this.generalPage.SuspendLayout();
            this.connectionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.installationPage.SuspendLayout();
            this.assemblyPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPreview)).BeginInit();
            this.monitoringTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Location = new System.Drawing.Point(719, 976);
            this.btnBuild.Margin = new System.Windows.Forms.Padding(8);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(302, 58);
            this.btnBuild.TabIndex = 1;
            this.btnBuild.Text = "生成客户端";
            this.tooltip.SetToolTip(this.btnBuild, "开始构建客户端可执行文件");
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // picUAC2
            // 
            this.picUAC2.Image = global::Quasar.Server.Properties.Resources.uac_shield;
            this.picUAC2.Location = new System.Drawing.Point(738, 218);
            this.picUAC2.Margin = new System.Windows.Forms.Padding(8);
            this.picUAC2.Name = "picUAC2";
            this.picUAC2.Size = new System.Drawing.Size(16, 20);
            this.picUAC2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picUAC2.TabIndex = 32;
            this.picUAC2.TabStop = false;
            this.tooltip.SetToolTip(this.picUAC2, "需要管理员权限才能在系统目录中安装客户端。");
            // 
            // picUAC1
            // 
            this.picUAC1.Image = global::Quasar.Server.Properties.Resources.uac_shield;
            this.picUAC1.Location = new System.Drawing.Point(738, 168);
            this.picUAC1.Margin = new System.Windows.Forms.Padding(8);
            this.picUAC1.Name = "picUAC1";
            this.picUAC1.Size = new System.Drawing.Size(16, 20);
            this.picUAC1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picUAC1.TabIndex = 31;
            this.picUAC1.TabStop = false;
            this.tooltip.SetToolTip(this.picUAC1, "需要管理员权限才能在程序文件中安装客户端。");
            // 
            // rbSystem
            // 
            this.rbSystem.AutoSize = true;
            this.rbSystem.Location = new System.Drawing.Point(432, 226);
            this.rbSystem.Margin = new System.Windows.Forms.Padding(8);
            this.rbSystem.Name = "rbSystem";
            this.rbSystem.Size = new System.Drawing.Size(112, 42);
            this.rbSystem.TabIndex = 5;
            this.rbSystem.TabStop = true;
            this.rbSystem.Text = "系统";
            this.tooltip.SetToolTip(this.rbSystem, "需要管理员权限才能在系统目录中安装客户端。");
            this.rbSystem.UseVisualStyleBackColor = true;
            this.rbSystem.CheckedChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeHostToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.contextMenuStrip.Name = "ctxtMenuHosts";
            this.contextMenuStrip.Size = new System.Drawing.Size(239, 100);
            // 
            // removeHostToolStripMenuItem
            // 
            this.removeHostToolStripMenuItem.Image = global::Quasar.Server.Properties.Resources.delete;
            this.removeHostToolStripMenuItem.Name = "removeHostToolStripMenuItem";
            this.removeHostToolStripMenuItem.Size = new System.Drawing.Size(238, 48);
            this.removeHostToolStripMenuItem.Text = "删除主机";
            this.removeHostToolStripMenuItem.Click += new System.EventHandler(this.removeHostToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::Quasar.Server.Properties.Resources.broom;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(238, 48);
            this.clearToolStripMenuItem.Text = "清除所有";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // builderTabs
            // 
            this.builderTabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.builderTabs.Controls.Add(this.generalPage);
            this.builderTabs.Controls.Add(this.connectionPage);
            this.builderTabs.Controls.Add(this.installationPage);
            this.builderTabs.Controls.Add(this.assemblyPage);
            this.builderTabs.Controls.Add(this.monitoringTab);
            this.builderTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.builderTabs.ItemSize = new System.Drawing.Size(44, 136);
            this.builderTabs.Location = new System.Drawing.Point(0, 0);
            this.builderTabs.Margin = new System.Windows.Forms.Padding(8);
            this.builderTabs.Multiline = true;
            this.builderTabs.Name = "builderTabs";
            this.builderTabs.SelectedIndex = 0;
            this.builderTabs.Size = new System.Drawing.Size(1050, 960);
            this.builderTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.builderTabs.TabIndex = 0;
            // 
            // generalPage
            // 
            this.generalPage.BackColor = System.Drawing.SystemColors.Control;
            this.generalPage.Controls.Add(this.label3);
            this.generalPage.Controls.Add(this.chkUnattendedMode);
            this.generalPage.Controls.Add(this.line2);
            this.generalPage.Controls.Add(this.label2);
            this.generalPage.Controls.Add(this.label9);
            this.generalPage.Controls.Add(this.line6);
            this.generalPage.Controls.Add(this.label8);
            this.generalPage.Controls.Add(this.txtTag);
            this.generalPage.Controls.Add(this.label7);
            this.generalPage.Controls.Add(this.lblTag);
            this.generalPage.Controls.Add(this.txtMutex);
            this.generalPage.Controls.Add(this.btnMutex);
            this.generalPage.Controls.Add(this.line5);
            this.generalPage.Controls.Add(this.lblMutex);
            this.generalPage.Controls.Add(this.label6);
            this.generalPage.Location = new System.Drawing.Point(140, 4);
            this.generalPage.Margin = new System.Windows.Forms.Padding(8);
            this.generalPage.Name = "generalPage";
            this.generalPage.Padding = new System.Windows.Forms.Padding(8);
            this.generalPage.Size = new System.Drawing.Size(906, 952);
            this.generalPage.TabIndex = 4;
            this.generalPage.Text = "基本设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 535);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(626, 76);
            this.label3.TabIndex = 24;
            this.label3.Text = "启用无人值守模式允许在没有用户交互的情况下\r\n远程控制客户端。";
            // 
            // chkUnattendedMode
            // 
            this.chkUnattendedMode.AutoSize = true;
            this.chkUnattendedMode.Location = new System.Drawing.Point(50, 630);
            this.chkUnattendedMode.Margin = new System.Windows.Forms.Padding(8);
            this.chkUnattendedMode.Name = "chkUnattendedMode";
            this.chkUnattendedMode.Size = new System.Drawing.Size(287, 42);
            this.chkUnattendedMode.TabIndex = 23;
            this.chkUnattendedMode.Text = "启用无人值守模式";
            this.tooltip.SetToolTip(this.chkUnattendedMode, "启用后，客户端将在没有用户交互的情况下运行");
            this.chkUnattendedMode.UseVisualStyleBackColor = true;
            this.chkUnattendedMode.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line2
            // 
            this.line2.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line2.Location = new System.Drawing.Point(288, 490);
            this.line2.Margin = new System.Windows.Forms.Padding(8);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(675, 32);
            this.line2.TabIndex = 22;
            this.line2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 490);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 38);
            this.label2.TabIndex = 21;
            this.label2.Text = "无人值守模式";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(42, 235);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(800, 38);
            this.label9.TabIndex = 5;
            this.label9.Text = "唯一的互斥量确保同一系统上只有一个客户端实例正在运行。";
            // 
            // line6
            // 
            this.line6.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line6.Location = new System.Drawing.Point(212, 195);
            this.line6.Margin = new System.Windows.Forms.Padding(8);
            this.line6.Name = "line6";
            this.line6.Size = new System.Drawing.Size(750, 32);
            this.line6.TabIndex = 20;
            this.line6.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 195);
            this.label8.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 38);
            this.label8.TabIndex = 4;
            this.label8.Text = "进程互斥量";
            // 
            // txtTag
            // 
            this.txtTag.Location = new System.Drawing.Point(226, 102);
            this.txtTag.Margin = new System.Windows.Forms.Padding(8);
            this.txtTag.Name = "txtTag";
            this.txtTag.Size = new System.Drawing.Size(632, 44);
            this.tooltip.SetToolTip(this.txtTag, "为客户端设置一个标识标签，便于区分不同的客户端");
            this.txtTag.TabIndex = 3;
            this.txtTag.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(42, 50);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(539, 38);
            this.label7.TabIndex = 1;
            this.label7.Text = "您可以选择一个标签来标识您的客户端。";
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(42, 108);
            this.lblTag.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(168, 38);
            this.lblTag.TabIndex = 2;
            this.lblTag.Text = "客户端标签:";
            // 
            // txtMutex
            // 
            this.txtMutex.Location = new System.Drawing.Point(228, 329);
            this.txtMutex.Margin = new System.Windows.Forms.Padding(8);
            this.txtMutex.MaxLength = 64;
            this.txtMutex.Name = "txtMutex";
            this.txtMutex.Size = new System.Drawing.Size(626, 44);
            this.tooltip.SetToolTip(this.txtMutex, "设置客户端的互斥量，确保同一系统上只有一个实例运行");
            this.txtMutex.TabIndex = 7;
            this.txtMutex.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // btnMutex
            // 
            this.btnMutex.Location = new System.Drawing.Point(556, 401);
            this.btnMutex.Margin = new System.Windows.Forms.Padding(8);
            this.btnMutex.Name = "btnMutex";
            this.btnMutex.Size = new System.Drawing.Size(302, 58);
            this.btnMutex.TabIndex = 8;
            this.btnMutex.Text = "随机互斥量";
            this.tooltip.SetToolTip(this.btnMutex, "生成一个随机的互斥量字符串");
            this.btnMutex.UseVisualStyleBackColor = true;
            this.btnMutex.Click += new System.EventHandler(this.btnMutex_Click);
            // 
            // line5
            // 
            this.line5.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line5.Location = new System.Drawing.Point(280, 12);
            this.line5.Margin = new System.Windows.Forms.Padding(8);
            this.line5.Name = "line5";
            this.line5.Size = new System.Drawing.Size(678, 32);
            this.line5.TabIndex = 15;
            this.line5.TabStop = false;
            // 
            // lblMutex
            // 
            this.lblMutex.AutoSize = true;
            this.lblMutex.Location = new System.Drawing.Point(42, 332);
            this.lblMutex.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblMutex.Name = "lblMutex";
            this.lblMutex.Size = new System.Drawing.Size(110, 38);
            this.lblMutex.TabIndex = 6;
            this.lblMutex.Text = "互斥量:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 38);
            this.label6.TabIndex = 0;
            this.label6.Text = "客户端标识";
            // 
            // connectionPage
            // 
            this.connectionPage.BackColor = System.Drawing.SystemColors.Control;
            this.connectionPage.Controls.Add(this.numericUpDownPort);
            this.connectionPage.Controls.Add(this.numericUpDownDelay);
            this.connectionPage.Controls.Add(this.line3);
            this.connectionPage.Controls.Add(this.label4);
            this.connectionPage.Controls.Add(this.line1);
            this.connectionPage.Controls.Add(this.label1);
            this.connectionPage.Controls.Add(this.lstHosts);
            this.connectionPage.Controls.Add(this.btnAddHost);
            this.connectionPage.Controls.Add(this.lblMS);
            this.connectionPage.Controls.Add(this.lblHost);
            this.connectionPage.Controls.Add(this.txtHost);
            this.connectionPage.Controls.Add(this.lblDelay);
            this.connectionPage.Controls.Add(this.lblPort);
            this.connectionPage.Location = new System.Drawing.Point(140, 4);
            this.connectionPage.Margin = new System.Windows.Forms.Padding(8);
            this.connectionPage.Name = "connectionPage";
            this.connectionPage.Padding = new System.Windows.Forms.Padding(8);
            this.connectionPage.Size = new System.Drawing.Size(906, 952);
            this.connectionPage.TabIndex = 0;
            this.connectionPage.Text = "连接设置";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(589, 163);
            this.numericUpDownPort.Margin = new System.Windows.Forms.Padding(8);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(200, 44);
            this.tooltip.SetToolTip(this.numericUpDownPort, "设置客户端连接到控制端服务器的端口号");
            this.numericUpDownPort.TabIndex = 3;
            this.numericUpDownPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownDelay.Location = new System.Drawing.Point(582, 782);
            this.numericUpDownDelay.Margin = new System.Windows.Forms.Padding(8);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(200, 44);
            this.tooltip.SetToolTip(this.numericUpDownDelay, "设置客户端在连接失败后重新连接的延迟时间");
            this.numericUpDownDelay.TabIndex = 10;
            this.numericUpDownDelay.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownDelay.ValueChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line3
            // 
            this.line3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.line3.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line3.Location = new System.Drawing.Point(197, 731);
            this.line3.Margin = new System.Windows.Forms.Padding(8);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(693, 35);
            this.line3.TabIndex = 18;
            this.line3.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 731);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 38);
            this.label4.TabIndex = 17;
            this.label4.Text = "重连延迟";
            // 
            // line1
            // 
            this.line1.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line1.Location = new System.Drawing.Point(220, 12);
            this.line1.Margin = new System.Windows.Forms.Padding(8);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(702, 32);
            this.line1.TabIndex = 13;
            this.line1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 38);
            this.label1.TabIndex = 14;
            this.label1.Text = "连接主机";
            // 
            // lstHosts
            // 
            this.lstHosts.ContextMenuStrip = this.contextMenuStrip;
            this.lstHosts.FormattingEnabled = true;
            this.lstHosts.ItemHeight = 37;
            this.lstHosts.Location = new System.Drawing.Point(50, 52);
            this.lstHosts.Margin = new System.Windows.Forms.Padding(8);
            this.lstHosts.Name = "lstHosts";
            this.lstHosts.Size = new System.Drawing.Size(366, 633);
            this.tooltip.SetToolTip(this.lstHosts, "显示已添加的连接主机列表，右键可删除或清空");
            this.lstHosts.TabIndex = 5;
            this.lstHosts.TabStop = false;
            // 
            // btnAddHost
            // 
            this.btnAddHost.Location = new System.Drawing.Point(516, 223);
            this.btnAddHost.Margin = new System.Windows.Forms.Padding(8);
            this.btnAddHost.Name = "btnAddHost";
            this.btnAddHost.Size = new System.Drawing.Size(322, 55);
            this.btnAddHost.TabIndex = 4;
            this.btnAddHost.Text = "添加主机";
            this.tooltip.SetToolTip(this.btnAddHost, "主机是指控制端的电脑。可以设置多台");
            this.btnAddHost.UseVisualStyleBackColor = true;
            this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
            // 
            // lblMS
            // 
            this.lblMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMS.AutoSize = true;
            this.lblMS.Location = new System.Drawing.Point(782, 785);
            this.lblMS.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblMS.Name = "lblMS";
            this.lblMS.Size = new System.Drawing.Size(75, 38);
            this.lblMS.TabIndex = 11;
            this.lblMS.Text = "毫秒";
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(438, 58);
            this.lblHost.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(144, 38);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "IP/主机名:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(445, 106);
            this.txtHost.Margin = new System.Windows.Forms.Padding(8);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(400, 44);
            this.tooltip.SetToolTip(this.txtHost, "输入控制端服务器的IP地址或域名");
            this.txtHost.TabIndex = 1;
            // 
            // lblDelay
            // 
            this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(41, 788);
            this.lblDelay.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(400, 38);
            this.lblDelay.TabIndex = 9;
            this.lblDelay.Text = "重新连接尝试之间等待的时间:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(432, 167);
            this.lblPort.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(81, 38);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "端口:";
            // 
            // installationPage
            // 
            this.installationPage.BackColor = System.Drawing.SystemColors.Control;
            this.installationPage.Controls.Add(this.chkHideSubDirectory);
            this.installationPage.Controls.Add(this.line7);
            this.installationPage.Controls.Add(this.label10);
            this.installationPage.Controls.Add(this.line4);
            this.installationPage.Controls.Add(this.label5);
            this.installationPage.Controls.Add(this.picUAC2);
            this.installationPage.Controls.Add(this.picUAC1);
            this.installationPage.Controls.Add(this.chkInstall);
            this.installationPage.Controls.Add(this.rbSystem);
            this.installationPage.Controls.Add(this.lblInstallName);
            this.installationPage.Controls.Add(this.rbProgramFiles);
            this.installationPage.Controls.Add(this.txtInstallName);
            this.installationPage.Controls.Add(this.txtRegistryKeyName);
            this.installationPage.Controls.Add(this.lblExtension);
            this.installationPage.Controls.Add(this.lblRegistryKeyName);
            this.installationPage.Controls.Add(this.chkStartup);
            this.installationPage.Controls.Add(this.rbAppdata);
            this.installationPage.Controls.Add(this.chkHide);
            this.installationPage.Controls.Add(this.lblInstallDirectory);
            this.installationPage.Controls.Add(this.lblInstallSubDirectory);
            this.installationPage.Controls.Add(this.lblPreviewPath);
            this.installationPage.Controls.Add(this.txtInstallSubDirectory);
            this.installationPage.Controls.Add(this.txtPreviewPath);
            this.installationPage.Location = new System.Drawing.Point(140, 4);
            this.installationPage.Margin = new System.Windows.Forms.Padding(8);
            this.installationPage.Name = "installationPage";
            this.installationPage.Padding = new System.Windows.Forms.Padding(8);
            this.installationPage.Size = new System.Drawing.Size(906, 952);
            this.installationPage.TabIndex = 1;
            this.installationPage.Text = "安装设置";
            // 
            // chkHideSubDirectory
            // 
            this.chkHideSubDirectory.AutoSize = true;
            this.chkHideSubDirectory.Location = new System.Drawing.Point(435, 462);
            this.chkHideSubDirectory.Margin = new System.Windows.Forms.Padding(8);
            this.chkHideSubDirectory.Name = "chkHideSubDirectory";
            this.chkHideSubDirectory.Size = new System.Drawing.Size(374, 42);
            this.chkHideSubDirectory.TabIndex = 37;
            this.chkHideSubDirectory.Text = "将子目录属性设置为隐藏";
            this.tooltip.SetToolTip(this.chkHideSubDirectory, "启用后，将安装的客户端子目录属性设置为隐藏");
            this.chkHideSubDirectory.UseVisualStyleBackColor = true;
            // 
            // line7
            // 
            this.line7.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line7.Location = new System.Drawing.Point(120, 685);
            this.line7.Margin = new System.Windows.Forms.Padding(8);
            this.line7.Name = "line7";
            this.line7.Size = new System.Drawing.Size(808, 32);
            this.line7.TabIndex = 36;
            this.line7.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 685);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 38);
            this.label10.TabIndex = 14;
            this.label10.Text = "自动启动";
            // 
            // line4
            // 
            this.line4.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line4.Location = new System.Drawing.Point(292, 12);
            this.line4.Margin = new System.Windows.Forms.Padding(8);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(665, 32);
            this.line4.TabIndex = 34;
            this.line4.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(475, 168);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 38);
            this.label5.TabIndex = 0;
            this.label5.Text = "安装位置";
            // 
            // chkInstall
            // 
            this.chkInstall.AutoSize = true;
            this.chkInstall.Location = new System.Drawing.Point(50, 52);
            this.chkInstall.Margin = new System.Windows.Forms.Padding(8);
            this.chkInstall.Name = "chkInstall";
            this.chkInstall.Size = new System.Drawing.Size(200, 42);
            this.chkInstall.TabIndex = 1;
            this.chkInstall.Text = "安装客户端";
            this.tooltip.SetToolTip(this.chkInstall, "启用后，客户端将在目标计算机上安装并持久化运行");
            this.chkInstall.UseVisualStyleBackColor = true;
            this.chkInstall.CheckedChanged += new System.EventHandler(this.chkInstall_CheckedChanged);
            // 
            // lblInstallName
            // 
            this.lblInstallName.AutoSize = true;
            this.lblInstallName.Location = new System.Drawing.Point(42, 390);
            this.lblInstallName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblInstallName.Name = "lblInstallName";
            this.lblInstallName.Size = new System.Drawing.Size(139, 38);
            this.lblInstallName.TabIndex = 8;
            this.lblInstallName.Text = "安装名称:";
            // 
            // rbProgramFiles
            // 
            this.rbProgramFiles.Location = new System.Drawing.Point(432, 156);
            this.rbProgramFiles.Margin = new System.Windows.Forms.Padding(8);
            this.rbProgramFiles.Name = "rbProgramFiles";
            this.rbProgramFiles.Size = new System.Drawing.Size(260, 60);
            this.rbProgramFiles.TabIndex = 38;
            // 
            // txtInstallName
            // 
            this.txtInstallName.Location = new System.Drawing.Point(285, 380);
            this.txtInstallName.Margin = new System.Windows.Forms.Padding(8);
            this.txtInstallName.Name = "txtInstallName";
            this.txtInstallName.Size = new System.Drawing.Size(419, 44);
            this.tooltip.SetToolTip(this.txtInstallName, "设置客户端安装后的文件名");
            this.txtInstallName.TabIndex = 9;
            this.txtInstallName.TextChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            this.txtInstallName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstallname_KeyPress);
            // 
            // txtRegistryKeyName
            // 
            this.txtRegistryKeyName.Location = new System.Drawing.Point(425, 810);
            this.txtRegistryKeyName.Margin = new System.Windows.Forms.Padding(8);
            this.txtRegistryKeyName.Name = "txtRegistryKeyName";
            this.txtRegistryKeyName.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtRegistryKeyName, "设置客户端在注册表中的启动项名称");
            this.txtRegistryKeyName.TabIndex = 17;
            this.txtRegistryKeyName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblExtension
            // 
            this.lblExtension.AutoSize = true;
            this.lblExtension.Location = new System.Drawing.Point(710, 396);
            this.lblExtension.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblExtension.Name = "lblExtension";
            this.lblExtension.Size = new System.Drawing.Size(66, 38);
            this.lblExtension.TabIndex = 10;
            this.lblExtension.Text = ".exe";
            // 
            // lblRegistryKeyName
            // 
            this.lblRegistryKeyName.AutoSize = true;
            this.lblRegistryKeyName.Location = new System.Drawing.Point(42, 818);
            this.lblRegistryKeyName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblRegistryKeyName.Name = "lblRegistryKeyName";
            this.lblRegistryKeyName.Size = new System.Drawing.Size(168, 38);
            this.lblRegistryKeyName.TabIndex = 16;
            this.lblRegistryKeyName.Text = "启动项名称:";
            // 
            // chkStartup
            // 
            this.chkStartup.AutoSize = true;
            this.chkStartup.Location = new System.Drawing.Point(50, 745);
            this.chkStartup.Margin = new System.Windows.Forms.Padding(8);
            this.chkStartup.Name = "chkStartup";
            this.chkStartup.Size = new System.Drawing.Size(374, 42);
            this.chkStartup.TabIndex = 15;
            this.chkStartup.Text = "计算机启动时运行客户端";
            this.tooltip.SetToolTip(this.chkStartup, "启用后，客户端将在系统启动时自动运行");
            this.chkStartup.UseVisualStyleBackColor = true;
            this.chkStartup.CheckedChanged += new System.EventHandler(this.chkInstall_CheckedChanged);
            // 
            // rbAppdata
            // 
            this.rbAppdata.AutoSize = true;
            this.rbAppdata.Checked = true;
            this.rbAppdata.Location = new System.Drawing.Point(432, 110);
            this.rbAppdata.Margin = new System.Windows.Forms.Padding(8);
            this.rbAppdata.Name = "rbAppdata";
            this.rbAppdata.Size = new System.Drawing.Size(286, 42);
            this.rbAppdata.TabIndex = 3;
            this.rbAppdata.TabStop = true;
            this.rbAppdata.Text = "用户应用程序数据";
            this.rbAppdata.UseVisualStyleBackColor = true;
            this.rbAppdata.CheckedChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            // 
            // chkHide
            // 
            this.chkHide.AutoSize = true;
            this.chkHide.Location = new System.Drawing.Point(50, 462);
            this.chkHide.Margin = new System.Windows.Forms.Padding(8);
            this.chkHide.Name = "chkHide";
            this.chkHide.Size = new System.Drawing.Size(345, 42);
            this.chkHide.TabIndex = 11;
            this.chkHide.Text = "将文件属性设置为隐藏";
            this.tooltip.SetToolTip(this.chkHide, "启用后，将安装的客户端文件属性设置为隐藏");
            this.chkHide.UseVisualStyleBackColor = true;
            this.chkHide.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblInstallDirectory
            // 
            this.lblInstallDirectory.AutoSize = true;
            this.lblInstallDirectory.Location = new System.Drawing.Point(42, 118);
            this.lblInstallDirectory.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblInstallDirectory.Name = "lblInstallDirectory";
            this.lblInstallDirectory.Size = new System.Drawing.Size(139, 38);
            this.lblInstallDirectory.TabIndex = 2;
            this.lblInstallDirectory.Text = "安装目录:";
            // 
            // lblInstallSubDirectory
            // 
            this.lblInstallSubDirectory.AutoSize = true;
            this.lblInstallSubDirectory.Location = new System.Drawing.Point(42, 315);
            this.lblInstallSubDirectory.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblInstallSubDirectory.Name = "lblInstallSubDirectory";
            this.lblInstallSubDirectory.Size = new System.Drawing.Size(168, 38);
            this.lblInstallSubDirectory.TabIndex = 6;
            this.lblInstallSubDirectory.Text = "安装子目录:";
            // 
            // lblPreviewPath
            // 
            this.lblPreviewPath.AutoSize = true;
            this.lblPreviewPath.Location = new System.Drawing.Point(42, 545);
            this.lblPreviewPath.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblPreviewPath.Name = "lblPreviewPath";
            this.lblPreviewPath.Size = new System.Drawing.Size(197, 38);
            this.lblPreviewPath.TabIndex = 12;
            this.lblPreviewPath.Text = "安装位置预览:";
            // 
            // txtInstallSubDirectory
            // 
            this.txtInstallSubDirectory.Location = new System.Drawing.Point(285, 306);
            this.txtInstallSubDirectory.Margin = new System.Windows.Forms.Padding(8);
            this.txtInstallSubDirectory.Name = "txtInstallSubDirectory";
            this.txtInstallSubDirectory.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtInstallSubDirectory, "设置客户端安装的子目录名称");
            this.txtInstallSubDirectory.TabIndex = 7;
            this.txtInstallSubDirectory.TextChanged += new System.EventHandler(this.HasChangedSettingAndFilePath);
            this.txtInstallSubDirectory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstallsub_KeyPress);
            // 
            // txtPreviewPath
            // 
            this.txtPreviewPath.Location = new System.Drawing.Point(20, 585);
            this.txtPreviewPath.Margin = new System.Windows.Forms.Padding(8);
            this.txtPreviewPath.Name = "txtPreviewPath";
            this.txtPreviewPath.ReadOnly = true;
            this.txtPreviewPath.Size = new System.Drawing.Size(902, 44);
            this.txtPreviewPath.TabIndex = 13;
            this.txtPreviewPath.TabStop = false;
            // 
            // assemblyPage
            // 
            this.assemblyPage.BackColor = System.Drawing.SystemColors.Control;
            this.assemblyPage.Controls.Add(this.iconPreview);
            this.assemblyPage.Controls.Add(this.btnBrowseIcon);
            this.assemblyPage.Controls.Add(this.txtIconPath);
            this.assemblyPage.Controls.Add(this.line8);
            this.assemblyPage.Controls.Add(this.label11);
            this.assemblyPage.Controls.Add(this.chkChangeAsmInfo);
            this.assemblyPage.Controls.Add(this.txtFileVersion);
            this.assemblyPage.Controls.Add(this.line9);
            this.assemblyPage.Controls.Add(this.lblProductName);
            this.assemblyPage.Controls.Add(this.label12);
            this.assemblyPage.Controls.Add(this.chkChangeIcon);
            this.assemblyPage.Controls.Add(this.lblFileVersion);
            this.assemblyPage.Controls.Add(this.txtProductName);
            this.assemblyPage.Controls.Add(this.txtProductVersion);
            this.assemblyPage.Controls.Add(this.lblDescription);
            this.assemblyPage.Controls.Add(this.lblProductVersion);
            this.assemblyPage.Controls.Add(this.txtDescription);
            this.assemblyPage.Controls.Add(this.txtOriginalFilename);
            this.assemblyPage.Controls.Add(this.lblCompanyName);
            this.assemblyPage.Controls.Add(this.lblOriginalFilename);
            this.assemblyPage.Controls.Add(this.txtCompanyName);
            this.assemblyPage.Controls.Add(this.txtTrademarks);
            this.assemblyPage.Controls.Add(this.lblCopyright);
            this.assemblyPage.Controls.Add(this.lblTrademarks);
            this.assemblyPage.Controls.Add(this.txtCopyright);
            this.assemblyPage.Location = new System.Drawing.Point(140, 4);
            this.assemblyPage.Margin = new System.Windows.Forms.Padding(8);
            this.assemblyPage.Name = "assemblyPage";
            this.assemblyPage.Size = new System.Drawing.Size(906, 952);
            this.assemblyPage.TabIndex = 2;
            this.assemblyPage.Text = "程序集设置";
            // 
            // iconPreview
            // 
            this.iconPreview.Image = global::Quasar.Server.Properties.Resources.远程控制;
            this.iconPreview.Location = new System.Drawing.Point(731, 755);
            this.iconPreview.Margin = new System.Windows.Forms.Padding(8);
            this.iconPreview.Name = "iconPreview";
            this.iconPreview.Size = new System.Drawing.Size(160, 160);
            this.iconPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPreview.TabIndex = 42;
            this.iconPreview.TabStop = false;
            // 
            // btnBrowseIcon
            // 
            this.btnBrowseIcon.Location = new System.Drawing.Point(410, 858);
            this.btnBrowseIcon.Margin = new System.Windows.Forms.Padding(8);
            this.btnBrowseIcon.Name = "btnBrowseIcon";
            this.btnBrowseIcon.Size = new System.Drawing.Size(312, 58);
            this.btnBrowseIcon.TabIndex = 41;
            this.btnBrowseIcon.Text = "浏览...";
            this.tooltip.SetToolTip(this.btnBrowseIcon, "选择客户端的自定义图标文件");
            this.btnBrowseIcon.UseVisualStyleBackColor = true;
            this.btnBrowseIcon.Click += new System.EventHandler(this.btnBrowseIcon_Click);
            // 
            // txtIconPath
            // 
            this.txtIconPath.Location = new System.Drawing.Point(18, 788);
            this.txtIconPath.Margin = new System.Windows.Forms.Padding(8);
            this.txtIconPath.Name = "txtIconPath";
            this.txtIconPath.Size = new System.Drawing.Size(699, 44);
            this.tooltip.SetToolTip(this.txtIconPath, "显示选择的图标文件路径");
            this.txtIconPath.TabIndex = 39;
            // 
            // line8
            // 
            this.line8.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line8.Location = new System.Drawing.Point(305, 12);
            this.line8.Margin = new System.Windows.Forms.Padding(8);
            this.line8.Name = "line8";
            this.line8.Size = new System.Drawing.Size(652, 32);
            this.line8.TabIndex = 36;
            this.line8.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 12);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(162, 38);
            this.label11.TabIndex = 35;
            this.label11.Text = "程序集信息";
            // 
            // chkChangeAsmInfo
            // 
            this.chkChangeAsmInfo.AutoSize = true;
            this.chkChangeAsmInfo.Location = new System.Drawing.Point(50, 52);
            this.chkChangeAsmInfo.Margin = new System.Windows.Forms.Padding(8);
            this.chkChangeAsmInfo.Name = "chkChangeAsmInfo";
            this.chkChangeAsmInfo.Size = new System.Drawing.Size(258, 42);
            this.chkChangeAsmInfo.TabIndex = 0;
            this.chkChangeAsmInfo.Text = "更改程序集信息";
            this.tooltip.SetToolTip(this.chkChangeAsmInfo, "启用后，可以自定义生成客户端的程序集信息");
            this.chkChangeAsmInfo.UseVisualStyleBackColor = true;
            this.chkChangeAsmInfo.CheckedChanged += new System.EventHandler(this.chkChangeAsmInfo_CheckedChanged);
            // 
            // txtFileVersion
            // 
            this.txtFileVersion.Location = new System.Drawing.Point(326, 600);
            this.txtFileVersion.Margin = new System.Windows.Forms.Padding(8);
            this.txtFileVersion.Name = "txtFileVersion";
            this.txtFileVersion.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtFileVersion, "设置客户端的文件版本号");
            this.txtFileVersion.TabIndex = 16;
            this.txtFileVersion.Text = "2.0.0";
            this.txtFileVersion.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // line9
            // 
            this.line9.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line9.Location = new System.Drawing.Point(176, 690);
            this.line9.Margin = new System.Windows.Forms.Padding(8);
            this.line9.Name = "line9";
            this.line9.Size = new System.Drawing.Size(750, 32);
            this.line9.TabIndex = 38;
            this.line9.TabStop = false;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(42, 118);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(139, 38);
            this.lblProductName.TabIndex = 1;
            this.lblProductName.Text = "产品名称:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 690);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 38);
            this.label12.TabIndex = 0;
            this.label12.Text = "程序集图标";
            // 
            // chkChangeIcon
            // 
            this.chkChangeIcon.AutoSize = true;
            this.chkChangeIcon.Location = new System.Drawing.Point(50, 735);
            this.chkChangeIcon.Margin = new System.Windows.Forms.Padding(8);
            this.chkChangeIcon.Name = "chkChangeIcon";
            this.chkChangeIcon.Size = new System.Drawing.Size(258, 42);
            this.chkChangeIcon.TabIndex = 2;
            this.chkChangeIcon.Text = "更改程序集图标";
            this.tooltip.SetToolTip(this.chkChangeIcon, "启用后，可以为客户端设置自定义图标");
            this.chkChangeIcon.UseVisualStyleBackColor = true;
            this.chkChangeIcon.CheckedChanged += new System.EventHandler(this.chkChangeIcon_CheckedChanged);
            // 
            // lblFileVersion
            // 
            this.lblFileVersion.AutoSize = true;
            this.lblFileVersion.Location = new System.Drawing.Point(42, 608);
            this.lblFileVersion.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblFileVersion.Name = "lblFileVersion";
            this.lblFileVersion.Size = new System.Drawing.Size(139, 38);
            this.lblFileVersion.TabIndex = 15;
            this.lblFileVersion.Text = "文件版本:";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(326, 110);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(8);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtProductName, "设置客户端的产品名称");
            this.txtProductName.TabIndex = 2;
            this.txtProductName.Text = "PC远程管理助手";
            this.txtProductName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtProductVersion
            // 
            this.txtProductVersion.Location = new System.Drawing.Point(326, 530);
            this.txtProductVersion.Margin = new System.Windows.Forms.Padding(8);
            this.txtProductVersion.Name = "txtProductVersion";
            this.txtProductVersion.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtProductVersion, "设置客户端的产品版本号");
            this.txtProductVersion.TabIndex = 14;
            this.txtProductVersion.Text = "2.0.0";
            this.txtProductVersion.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(42, 188);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(81, 38);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "描述:";
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.AutoSize = true;
            this.lblProductVersion.Location = new System.Drawing.Point(42, 538);
            this.lblProductVersion.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(139, 38);
            this.lblProductVersion.TabIndex = 13;
            this.lblProductVersion.Text = "产品版本:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(326, 180);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(8);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtDescription, "设置客户端的描述信息");
            this.txtDescription.TabIndex = 4;
            this.txtDescription.Text = "用户支持；日常系统管理；员工监控等场景";
            this.txtDescription.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtOriginalFilename
            // 
            this.txtOriginalFilename.Location = new System.Drawing.Point(326, 460);
            this.txtOriginalFilename.Margin = new System.Windows.Forms.Padding(8);
            this.txtOriginalFilename.Name = "txtOriginalFilename";
            this.txtOriginalFilename.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtOriginalFilename, "设置客户端的原始文件名");
            this.txtOriginalFilename.TabIndex = 12;
            this.txtOriginalFilename.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(42, 258);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(139, 38);
            this.lblCompanyName.TabIndex = 5;
            this.lblCompanyName.Text = "公司名称:";
            // 
            // lblOriginalFilename
            // 
            this.lblOriginalFilename.AutoSize = true;
            this.lblOriginalFilename.Location = new System.Drawing.Point(42, 468);
            this.lblOriginalFilename.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblOriginalFilename.Name = "lblOriginalFilename";
            this.lblOriginalFilename.Size = new System.Drawing.Size(168, 38);
            this.lblOriginalFilename.TabIndex = 11;
            this.lblOriginalFilename.Text = "原始文件名:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(326, 250);
            this.txtCompanyName.Margin = new System.Windows.Forms.Padding(8);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtCompanyName, "设置客户端的公司名称");
            this.txtCompanyName.TabIndex = 6;
            this.txtCompanyName.Text = "科控物联";
            this.txtCompanyName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtTrademarks
            // 
            this.txtTrademarks.Location = new System.Drawing.Point(326, 390);
            this.txtTrademarks.Margin = new System.Windows.Forms.Padding(8);
            this.txtTrademarks.Name = "txtTrademarks";
            this.txtTrademarks.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtTrademarks, "设置客户端的商标信息");
            this.txtTrademarks.TabIndex = 10;
            this.txtTrademarks.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(42, 328);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(81, 38);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "版权:";
            // 
            // lblTrademarks
            // 
            this.lblTrademarks.AutoSize = true;
            this.lblTrademarks.Location = new System.Drawing.Point(42, 398);
            this.lblTrademarks.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblTrademarks.Name = "lblTrademarks";
            this.lblTrademarks.Size = new System.Drawing.Size(81, 38);
            this.lblTrademarks.TabIndex = 9;
            this.lblTrademarks.Text = "商标:";
            // 
            // txtCopyright
            // 
            this.txtCopyright.Location = new System.Drawing.Point(326, 320);
            this.txtCopyright.Margin = new System.Windows.Forms.Padding(8);
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(496, 44);
            this.tooltip.SetToolTip(this.txtCopyright, "设置客户端的版权信息");
            this.txtCopyright.TabIndex = 8;
            this.txtCopyright.Text = "版权所有 (c) 2025 科控物联";
            this.txtCopyright.TextChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // monitoringTab
            // 
            this.monitoringTab.BackColor = System.Drawing.SystemColors.Control;
            this.monitoringTab.Controls.Add(this.chkHideLogDirectory);
            this.monitoringTab.Controls.Add(this.txtLogDirectoryName);
            this.monitoringTab.Controls.Add(this.lblLogDirectory);
            this.monitoringTab.Controls.Add(this.line10);
            this.monitoringTab.Controls.Add(this.label14);
            this.monitoringTab.Controls.Add(this.chkKeylogger);
            this.monitoringTab.Location = new System.Drawing.Point(140, 4);
            this.monitoringTab.Margin = new System.Windows.Forms.Padding(8);
            this.monitoringTab.Name = "monitoringTab";
            this.monitoringTab.Size = new System.Drawing.Size(906, 952);
            this.monitoringTab.TabIndex = 3;
            this.monitoringTab.Text = "监控设置";
            // 
            // chkHideLogDirectory
            // 
            this.chkHideLogDirectory.AutoSize = true;
            this.chkHideLogDirectory.Location = new System.Drawing.Point(50, 180);
            this.chkHideLogDirectory.Margin = new System.Windows.Forms.Padding(8);
            this.chkHideLogDirectory.Name = "chkHideLogDirectory";
            this.chkHideLogDirectory.Size = new System.Drawing.Size(345, 42);
            this.chkHideLogDirectory.TabIndex = 7;
            this.chkHideLogDirectory.Text = "将目录属性设置为隐藏";
            this.tooltip.SetToolTip(this.chkHideLogDirectory, "启用后，将隐藏键盘记录日志目录");
            this.chkHideLogDirectory.UseVisualStyleBackColor = true;
            this.chkHideLogDirectory.CheckedChanged += new System.EventHandler(this.HasChangedSetting);
            // 
            // txtLogDirectoryName
            // 
            this.txtLogDirectoryName.Location = new System.Drawing.Point(274, 110);
            this.txtLogDirectoryName.Margin = new System.Windows.Forms.Padding(8);
            this.txtLogDirectoryName.Name = "txtLogDirectoryName";
            this.txtLogDirectoryName.Size = new System.Drawing.Size(500, 44);
            this.tooltip.SetToolTip(this.txtLogDirectoryName, "设置键盘记录日志的目录名称");
            this.txtLogDirectoryName.TabIndex = 6;
            this.txtLogDirectoryName.TextChanged += new System.EventHandler(this.HasChangedSetting);
            this.txtLogDirectoryName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLogDirectoryName_KeyPress);
            // 
            // lblLogDirectory
            // 
            this.lblLogDirectory.AutoSize = true;
            this.lblLogDirectory.Location = new System.Drawing.Point(42, 118);
            this.lblLogDirectory.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblLogDirectory.Name = "lblLogDirectory";
            this.lblLogDirectory.Size = new System.Drawing.Size(197, 38);
            this.lblLogDirectory.TabIndex = 5;
            this.lblLogDirectory.Text = "日志目录名称:";
            // 
            // line10
            // 
            this.line10.LineAlignment = Quasar.Server.Controls.Line.Alignment.Horizontal;
            this.line10.Location = new System.Drawing.Point(195, 12);
            this.line10.Margin = new System.Windows.Forms.Padding(8);
            this.line10.Name = "line10";
            this.line10.Size = new System.Drawing.Size(755, 32);
            this.line10.TabIndex = 41;
            this.line10.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 12);
            this.label14.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 38);
            this.label14.TabIndex = 3;
            this.label14.Text = "监控";
            // 
            // chkKeylogger
            // 
            this.chkKeylogger.AutoSize = true;
            this.chkKeylogger.Location = new System.Drawing.Point(50, 52);
            this.chkKeylogger.Margin = new System.Windows.Forms.Padding(8);
            this.chkKeylogger.Name = "chkKeylogger";
            this.chkKeylogger.Size = new System.Drawing.Size(229, 42);
            this.chkKeylogger.TabIndex = 4;
            this.chkKeylogger.Text = "启用键盘记录";
            this.tooltip.SetToolTip(this.chkKeylogger, "启用后，客户端将记录用户的键盘输入");
            this.chkKeylogger.UseVisualStyleBackColor = true;
            this.chkKeylogger.CheckedChanged += new System.EventHandler(this.chkKeylogger_CheckedChanged);
            // 
            // FrmBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1050, 1046);
            this.Controls.Add(this.builderTabs);
            this.Controls.Add(this.btnBuild);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客户端构建器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBuilder_FormClosing);
            this.Load += new System.EventHandler(this.FrmBuilder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picUAC2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUAC1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.builderTabs.ResumeLayout(false);
            this.generalPage.ResumeLayout(false);
            this.generalPage.PerformLayout();
            this.connectionPage.ResumeLayout(false);
            this.connectionPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.installationPage.ResumeLayout(false);
            this.installationPage.PerformLayout();
            this.assemblyPage.ResumeLayout(false);
            this.assemblyPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPreview)).EndInit();
            this.monitoringTab.ResumeLayout(false);
            this.monitoringTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.CheckBox chkInstall;
        private System.Windows.Forms.TextBox txtInstallName;
        private System.Windows.Forms.Label lblInstallName;
        private System.Windows.Forms.TextBox txtMutex;
        private System.Windows.Forms.Label lblMutex;
        private System.Windows.Forms.Label lblExtension;
        private System.Windows.Forms.Label lblInstallDirectory;
        private System.Windows.Forms.RadioButton rbAppdata;
        private System.Windows.Forms.RadioButton rbProgramFiles;
        private System.Windows.Forms.TextBox txtInstallSubDirectory;
        private System.Windows.Forms.Label lblInstallSubDirectory;
        private System.Windows.Forms.Label lblPreviewPath;
        private System.Windows.Forms.TextBox txtPreviewPath;
        private System.Windows.Forms.Button btnMutex;
        private System.Windows.Forms.CheckBox chkHide;
        private System.Windows.Forms.TextBox txtRegistryKeyName;
        private System.Windows.Forms.Label lblRegistryKeyName;
        private System.Windows.Forms.CheckBox chkStartup;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Label lblMS;
        private System.Windows.Forms.RadioButton rbSystem;
        private System.Windows.Forms.PictureBox picUAC1;
        private System.Windows.Forms.PictureBox picUAC2;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.CheckBox chkChangeIcon;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtOriginalFilename;
        private System.Windows.Forms.Label lblOriginalFilename;
        private System.Windows.Forms.TextBox txtTrademarks;
        private System.Windows.Forms.Label lblTrademarks;
        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtFileVersion;
        private System.Windows.Forms.Label lblFileVersion;
        private System.Windows.Forms.TextBox txtProductVersion;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.CheckBox chkChangeAsmInfo;
        private System.Windows.Forms.CheckBox chkKeylogger;
        private Controls.DotNetBarTabControl builderTabs;
        private System.Windows.Forms.TabPage connectionPage;
        private System.Windows.Forms.TabPage installationPage;
        private System.Windows.Forms.TabPage assemblyPage;
        private System.Windows.Forms.TabPage monitoringTab;
        private System.Windows.Forms.ListBox lstHosts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.Button btnAddHost;
        private System.Windows.Forms.ToolStripMenuItem removeHostToolStripMenuItem;
        private Controls.Line line1;
        private System.Windows.Forms.Label label1;
        private Controls.Line line3;
        private System.Windows.Forms.Label label4;
        private Controls.Line line4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage generalPage;
        private System.Windows.Forms.TextBox txtTag;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTag;
        private Controls.Line line5;
        private System.Windows.Forms.Label label6;
        private Controls.Line line6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Controls.Line line7;
        private System.Windows.Forms.Label label10;
        private Controls.Line line8;
        private System.Windows.Forms.Label label11;
        private Controls.Line line9;
        private System.Windows.Forms.Label label12;
        private Controls.Line line10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button btnBrowseIcon;
        private System.Windows.Forms.TextBox txtIconPath;
        private System.Windows.Forms.PictureBox iconPreview;
        private System.Windows.Forms.Label lblLogDirectory;
        private System.Windows.Forms.TextBox txtLogDirectoryName;
        private System.Windows.Forms.CheckBox chkHideLogDirectory;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.CheckBox chkHideSubDirectory;
        private System.Windows.Forms.CheckBox chkUnattendedMode;
        private Line line2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

    }
}
