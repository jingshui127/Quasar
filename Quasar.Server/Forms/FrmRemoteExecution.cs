﻿using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Models;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmRemoteExecution : Form
    {
        private class RemoteExecutionMessageHandler
        {
            public FileManagerHandler FileHandler;
            public TaskManagerHandler TaskHandler;
        }

        /// <summary>
        /// 可用于远程执行的客户端。
        /// </summary>
        private readonly Client[] _clients;

        private readonly List<RemoteExecutionMessageHandler> _remoteExecutionMessageHandlers;

        private enum TransferColumn
        {
            Client,
            Status
        }

        private bool _isUpdate;

        public FrmRemoteExecution(Client[] clients)
        {
            _clients = clients;
            _remoteExecutionMessageHandlers = new List<RemoteExecutionMessageHandler>(clients.Length);

            InitializeComponent();

            foreach (var client in clients)
            {
                var remoteExecutionMessageHandler = new RemoteExecutionMessageHandler
                {
                    FileHandler = new FileManagerHandler(client), TaskHandler = new TaskManagerHandler(client)
                };

                var lvi = new ListViewItem(new[]
                {
                    $"{client.Value.Username}@{client.Value.PcName} [{client.EndPoint.Address}:{client.EndPoint.Port}]",
                    "Waiting..."
                }) {Tag = remoteExecutionMessageHandler};

                lstTransfers.Items.Add(lvi);
                _remoteExecutionMessageHandlers.Add(remoteExecutionMessageHandler);
                RegisterMessageHandler(remoteExecutionMessageHandler);
            }
        }

        /// <summary>
        /// 注册用于客户端通信的消息处理器。
        /// </summary>
        private void RegisterMessageHandler(RemoteExecutionMessageHandler remoteExecutionMessageHandler)
        {
            // TODO 处理断开连接
            remoteExecutionMessageHandler.TaskHandler.ProcessActionPerformed += ProcessActionPerformed;
            remoteExecutionMessageHandler.FileHandler.ProgressChanged += SetStatusMessage;
            remoteExecutionMessageHandler.FileHandler.FileTransferUpdated += FileTransferUpdated;
            MessageHandler.Register(remoteExecutionMessageHandler.FileHandler);
            MessageHandler.Register(remoteExecutionMessageHandler.TaskHandler);
        }

        /// <summary>
        /// 注销消息处理器。
        /// </summary>
        private void UnregisterMessageHandler(RemoteExecutionMessageHandler remoteExecutionMessageHandler)
        {
            MessageHandler.Unregister(remoteExecutionMessageHandler.TaskHandler);
            MessageHandler.Unregister(remoteExecutionMessageHandler.FileHandler);
            remoteExecutionMessageHandler.FileHandler.ProgressChanged -= SetStatusMessage;
            remoteExecutionMessageHandler.FileHandler.FileTransferUpdated -= FileTransferUpdated;
            remoteExecutionMessageHandler.TaskHandler.ProcessActionPerformed -= ProcessActionPerformed;
        }

        private void FrmRemoteExecution_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Execution", _clients.Length);
        }

        private void FrmRemoteExecution_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var handler in _remoteExecutionMessageHandlers)
            {
                UnregisterMessageHandler(handler);
                handler.FileHandler.Dispose();
            }

            _remoteExecutionMessageHandlers.Clear();
            lstTransfers.Items.Clear();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            _isUpdate = chkUpdate.Checked;

            if (radioURL.Checked)
            {
                foreach (var handler in _remoteExecutionMessageHandlers)
                {
                    if (!txtURL.Text.StartsWith("http"))
                        txtURL.Text = "http://" + txtURL.Text;

                    handler.TaskHandler.StartProcessFromWeb(txtURL.Text, _isUpdate);
                }
            }
            else
            {
                foreach (var handler in _remoteExecutionMessageHandlers)
                {
                    handler.FileHandler.BeginUploadFile(txtPath.Text);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Executable (*.exe)|*.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = Path.Combine(ofd.InitialDirectory, ofd.FileName);
                }
            }
        }

        private void radioLocalFile_CheckedChanged(object sender, EventArgs e)
        {
            groupLocalFile.Enabled = radioLocalFile.Checked;
            groupURL.Enabled = !radioLocalFile.Checked;
        }

        private void radioURL_CheckedChanged(object sender, EventArgs e)
        {
            groupLocalFile.Enabled = !radioURL.Checked;
            groupURL.Enabled = radioURL.Checked;
        }

        /// <summary>
        /// 当文件传输更新时调用。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="transfer">更新的文件传输。</param>
        private void FileTransferUpdated(object sender, FileTransfer transfer)
        {
            for (var i = 0; i < lstTransfers.Items.Count; i++)
            {
                var handler = (RemoteExecutionMessageHandler) lstTransfers.Items[i].Tag;

                if (handler.FileHandler.Equals(sender as FileManagerHandler) || handler.TaskHandler.Equals(sender as TaskManagerHandler))
                {
                    lstTransfers.Items[i].SubItems[(int) TransferColumn.Status].Text = transfer.Status;

                    if (transfer.Status == "Completed")
                    {
                        handler.TaskHandler.StartProcess(transfer.RemotePath, _isUpdate);
                    }
                    return;
                }
            }
        }

        // TODO: 更新文档
        /// <summary>
        /// 设置文件管理器的状态。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="message">新状态。</param>
        private void SetStatusMessage(object sender, string message)
        {
            for (var i = 0; i < lstTransfers.Items.Count; i++)
            {
                var handler = (RemoteExecutionMessageHandler)lstTransfers.Items[i].Tag;

                if (handler.FileHandler.Equals(sender as FileManagerHandler) || handler.TaskHandler.Equals(sender as TaskManagerHandler))
                {
                    lstTransfers.Items[i].SubItems[(int) TransferColumn.Status].Text = message;
                    return;
                }
            }
        }

        private void ProcessActionPerformed(object sender, ProcessAction action, bool result)
        {
            if (action != ProcessAction.Start) return;

            for (var i = 0; i < lstTransfers.Items.Count; i++)
            {
                var handler = (RemoteExecutionMessageHandler)lstTransfers.Items[i].Tag;

                if (handler.FileHandler.Equals(sender as FileManagerHandler) || handler.TaskHandler.Equals(sender as TaskManagerHandler))
                {
                    lstTransfers.Items[i].SubItems[(int)TransferColumn.Status].Text = result ? "Successfully started process" : "Failed to start process";
                    return;
                }
            }
        }
    }
}
