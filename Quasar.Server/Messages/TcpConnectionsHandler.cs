﻿using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Networking;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程TCP连接交互的消息。
    /// </summary>
    public class TcpConnectionsHandler : MessageProcessorBase<TcpConnection[]>
    {
        /// <summary>
        /// 与此TCP连接处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="TcpConnectionsHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public TcpConnectionsHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetConnectionsResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetConnectionsResponse con:
                    Execute(sender, con);
                    break;
            }
        }

        /// <summary>
        /// 刷新当前的TCP连接。
        /// </summary>
        public void RefreshTcpConnections()
        {
            _client.Send(new GetConnections());
        }

        /// <summary>
        /// 关闭客户端的TCP连接。
        /// </summary>
        /// <param name="localAddress">本地地址。</param>
        /// <param name="localPort">本地端口。</param>
        /// <param name="remoteAddress">远程地址。</param>
        /// <param name="remotePort">远程端口。</param>
        public void CloseTcpConnection(string localAddress, ushort localPort, string remoteAddress, ushort remotePort)
        {
            // a unique tcp connection is determined by local address + port and remote address + port
            _client.Send(new DoCloseConnection
            {
                LocalAddress = localAddress,
                LocalPort = localPort,
                RemoteAddress = remoteAddress,
                RemotePort = remotePort
            });
        }

        private void Execute(ISender client, GetConnectionsResponse message)
        {
            OnReport(message.Connections);
        }
    }
}
