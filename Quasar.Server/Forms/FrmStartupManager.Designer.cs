﻿﻿﻿﻿﻿﻿using Quasar.Server.Controls;

namespace Quasar.Server.Forms
{
    partial class FrmStartupManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStartupManager));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstStartupItems = new AeroListView();
            this.hName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEntryToolStripMenuItem,
            this.removeEntryToolStripMenuItem});
            this.contextMenuStrip.Name = "ctxtMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // addEntryToolStripMenuItem
            // 
            this.addEntryToolStripMenuItem.Image = global::Quasar.Server.Properties.Resources.application_add;
            this.addEntryToolStripMenuItem.Name = "addEntryToolStripMenuItem";
            this.addEntryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addEntryToolStripMenuItem.Text = "添加条目";
            this.addEntryToolStripMenuItem.Click += new System.EventHandler(this.addEntryToolStripMenuItem_Click);
            // 
            // removeEntryToolStripMenuItem
            // 
            this.removeEntryToolStripMenuItem.Image = global::Quasar.Server.Properties.Resources.application_delete;
            this.removeEntryToolStripMenuItem.Name = "removeEntryToolStripMenuItem";
            this.removeEntryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeEntryToolStripMenuItem.Text = "删除条目";
            this.removeEntryToolStripMenuItem.Click += new System.EventHandler(this.removeEntryToolStripMenuItem_Click);
            // 
            // lstStartupItems
            // 
            this.lstStartupItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstStartupItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hName,
            this.hPath});
            this.lstStartupItems.ContextMenuStrip = this.contextMenuStrip;
            this.lstStartupItems.FullRowSelect = true;
            this.lstStartupItems.GridLines = true;
            this.lstStartupItems.Location = new System.Drawing.Point(12, 12);
            this.lstStartupItems.Name = "lstStartupItems";
            this.lstStartupItems.Size = new System.Drawing.Size(653, 349);
            this.lstStartupItems.TabIndex = 0;
            this.lstStartupItems.UseCompatibleStateImageBehavior = false;
            this.lstStartupItems.View = System.Windows.Forms.View.Details;
            // 
            // hName
            // 
            this.hName.Text = "名称";
            this.hName.Width = 187;
            // 
            // hPath
            // 
            this.hPath.Text = "路径";
            this.hPath.Width = 460;
            // 
            // FrmStartupManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(677, 373);
            this.Controls.Add(this.lstStartupItems);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "FrmStartupManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "启动项管理器 []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStartupManager_FormClosing);
            this.Load += new System.EventHandler(this.FrmStartupManager_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader hName;
        private System.Windows.Forms.ColumnHeader hPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeEntryToolStripMenuItem;
        private Controls.AeroListView lstStartupItems;

    }
}