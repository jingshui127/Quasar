﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;

namespace Quasar.Server.Forms
{
    public partial class FrmConnections : Form
    {
        /// <summary>
        /// 可用于连接管理器的客户端。
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// 用于处理与客户端通信的消息处理器。
        /// </summary>
        private readonly TcpConnectionsHandler _connectionsHandler;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, ListViewGroup> _groups = new Dictionary<string, ListViewGroup>();

        /// <summary>
        /// 为每个客户端保存已打开的连接管理器表单。
        /// </summary>
        private static readonly Dictionary<Client, FrmConnections> OpenedForms = new Dictionary<Client, FrmConnections>();

        /// <summary>
        /// 为客户创建新的连接管理器表单，或者如果已存在则获取当前打开的表单。
        /// </summary>
        /// <param name="client">用于连接管理器表单的客户端。</param>
        /// <returns>
        /// 如果当前没有打开的连接管理器表单，则为客户端返回一个新的连接管理器表单，否则创建一个新的。
        /// </returns>
        public static FrmConnections CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmConnections f = new FrmConnections(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }

        /// <summary>
        /// 使用给定的客户端初始化 <see cref="FrmConnections"/> 类的新实例。
        /// </summary>
        /// <param name="client">用于连接管理器表单的客户端。</param>
        public FrmConnections(Client client)
        {
            _connectClient = client;
            _connectionsHandler = new TcpConnectionsHandler(client);

            RegisterMessageHandler();
            InitializeComponent();
        }

        /// <summary>
        /// 注册连接管理器消息处理器以进行客户端通信。
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _connectionsHandler.ProgressChanged += TcpConnectionsChanged;
            MessageHandler.Register(_connectionsHandler);
        }

        /// <summary>
        /// 注销连接管理器消息处理器。
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(_connectionsHandler);
            _connectionsHandler.ProgressChanged -= TcpConnectionsChanged;
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

        /// <summary>
        /// 当TCP连接发生变化时调用。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="connections">客户端的当前TCP连接。</param>
        private void TcpConnectionsChanged(object sender, TcpConnection[] connections)
        {
            lstConnections.Items.Clear();

            foreach (var con in connections)
            {
                string state = con.State.ToString();

                ListViewItem lvi = new ListViewItem(new[]
                {
                    con.ProcessName, con.LocalAddress, con.LocalPort.ToString(),
                    con.RemoteAddress, con.RemotePort.ToString(), state
                });

                if (!_groups.ContainsKey(state))
                {
                    // create new group if not exists already
                    ListViewGroup g = new ListViewGroup(state, state);
                    lstConnections.Groups.Add(g);
                    _groups.Add(state, g);
                }

                lvi.Group = lstConnections.Groups[state];
                lstConnections.Items.Add(lvi);
            }
        }

        private void FrmConnections_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.favicon;
            this.Text = WindowHelper.GetWindowTitle("Connections", _connectClient);
            _connectionsHandler.RefreshTcpConnections();
        }

        private void FrmConnections_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterMessageHandler();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _connectionsHandler.RefreshTcpConnections();
        }

        private void closeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool modified = false;

            foreach (ListViewItem lvi in lstConnections.SelectedItems)
            {
                _connectionsHandler.CloseTcpConnection(lvi.SubItems[1].Text, ushort.Parse(lvi.SubItems[2].Text),
                    lvi.SubItems[3].Text, ushort.Parse(lvi.SubItems[4].Text));
                modified = true;
            }

            if (modified)
            {
                _connectionsHandler.RefreshTcpConnections();
            }
        }

        private void lstConnections_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lstConnections.LvwColumnSorter.NeedNumberCompare = (e.Column == 2 || e.Column == 4);
        }
    }
}
