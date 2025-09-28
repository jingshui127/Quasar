﻿﻿﻿using Quasar.Common.Extensions;
using Quasar.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Quasar.Server.Networking
{
    public class Server
    {
        /// <summary>
        /// 当服务器状态更改时发生。
        /// </summary>
        public event ServerStateEventHandler ServerState;

        /// <summary>
        /// 表示将处理服务器状态更改的方法。
        /// </summary>
        /// <param name="s">更改状态的服务器。</param>
        /// <param name="listening">服务器的新监听状态。</param>
        /// <param name="port">服务器监听的端口，如果listening为True。</param>
        public delegate void ServerStateEventHandler(Server s, bool listening, ushort port);

        /// <summary>
        /// 触发一个事件，通知订阅者服务器已更改其状态。
        /// </summary>
        /// <param name="listening">服务器的新监听状态。</param>
        private void OnServerState(bool listening)
        {
            if (Listening == listening) return;

            Listening = listening;

            var handler = ServerState;
            handler?.Invoke(this, listening, Port);
        }

        /// <summary>
        /// 当客户端状态更改时发生。
        /// </summary>
        public event ClientStateEventHandler ClientState;

        /// <summary>
        /// 表示将处理客户端状态更改的方法。
        /// </summary>
        /// <param name="s">客户端连接的服务器。</param>
        /// <param name="c">更改状态的客户端。</param>
        /// <param name="connected">客户端的新连接状态。</param>
        public delegate void ClientStateEventHandler(Server s, Client c, bool connected);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已更改其状态。
        /// </summary>
        /// <param name="c">更改状态的客户端。</param>
        /// <param name="connected">客户端的新连接状态。</param>
        private void OnClientState(Client c, bool connected)
        {
            if (!connected)
                RemoveClient(c);

            var handler = ClientState;
            handler?.Invoke(this, c, connected);
        }

        /// <summary>
        /// 当客户端接收到消息时发生。
        /// </summary>
        public event ClientReadEventHandler ClientRead;

        /// <summary>
        /// 表示将处理从客户端接收消息的方法。
        /// </summary>
        /// <param name="s">客户端连接的服务器。</param>
        /// <param name="c">接收消息的客户端。</param>
        /// <param name="message">客户端接收的消息。</param>
        public delegate void ClientReadEventHandler(Server s, Client c, IMessage message);

        /// <summary>
        /// 触发一个事件，通知订阅者已从客户端接收到消息。
        /// </summary>
        /// <param name="c">接收消息的客户端。</param>
        /// <param name="message">客户端接收的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        private void OnClientRead(Client c, IMessage message, int messageLength)
        {
            BytesReceived += messageLength;
            var handler = ClientRead;
            handler?.Invoke(this, c, message);
        }

        /// <summary>
        /// 当客户端发送消息时发生。
        /// </summary>
        public event ClientWriteEventHandler ClientWrite;

        /// <summary>
        /// 表示将处理客户端发送消息的方法。
        /// </summary>
        /// <param name="s">客户端连接的服务器。</param>
        /// <param name="c">发送消息的客户端。</param>
        /// <param name="message">客户端发送的消息。</param>
        public delegate void ClientWriteEventHandler(Server s, Client c, IMessage message);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已发送消息。
        /// </summary>
        /// <param name="c">发送消息的客户端。</param>
        /// <param name="message">客户端发送的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        private void OnClientWrite(Client c, IMessage message, int messageLength)
        {
            BytesSent += messageLength;
            var handler = ClientWrite;
            handler?.Invoke(this, c, message);
        }

        /// <summary>
        /// 服务器监听的端口。
        /// </summary>
        public ushort Port { get; private set; }

        /// <summary>
        /// 接收的总字节数。
        /// </summary>
        public long BytesReceived { get; set; }

        /// <summary>
        /// 发送的总字节数。
        /// </summary>
        public long BytesSent { get; set; }

        /// <summary>
        /// 接收数据的缓冲区大小（以字节为单位）。
        /// </summary>
        private const int BufferSize = 1024 * 16; // 16 KB

        /// <summary>
        /// 保持活动状态的时间（以毫秒为单位）。
        /// </summary>
        private const uint KeepAliveTime = 25000; // 25 s

        /// <summary>
        /// 保持活动状态的间隔（以毫秒为单位）。
        /// </summary>
        private const uint KeepAliveInterval = 25000; // 25 s

        /// <summary>
        /// 用于保存客户端接收缓冲区的缓冲池。
        /// </summary>
        private readonly BufferPool _bufferPool = new BufferPool(BufferSize, 1) { ClearOnReturn = false };

        /// <summary>
        /// 服务器的监听状态。如果正在监听则为True，否则为False。
        /// </summary>
        public bool Listening { get; private set; }

        /// <summary>
        /// 获取当前连接到服务器的客户端。
        /// </summary>
        protected Client[] Clients
        {
            get
            {
                lock (_clientsLock)
                {
                    return _clients.ToArray();
                }
            }
        }

        /// <summary>
        /// 服务器套接字的句柄。
        /// </summary>
        private Socket _handle;

        /// <summary>
        /// 服务器证书。
        /// </summary>
        protected readonly X509Certificate2 ServerCertificate;

        /// <summary>
        /// 异步接受新连接的事件。
        /// </summary>
        private SocketAsyncEventArgs _item;

        /// <summary>
        /// 连接到服务器的客户端列表。
        /// </summary>
        private readonly List<Client> _clients = new List<Client>();

        /// <summary>
        /// 用于发现、创建和删除端口映射的UPnP服务。
        /// </summary>
        private UPnPService _UPnPService;

        /// <summary>
        /// 客户端列表的锁定对象。
        /// </summary>
        private readonly object _clientsLock = new object();

        /// <summary>
        /// 确定服务器当前是否正在处理Disconnect方法。
        /// </summary>
        protected bool ProcessingDisconnect { get; set; }

        /// <summary>
        /// 服务器的构造函数，初始化序列化器类型。
        /// </summary>
        /// <param name="serverCertificate">服务器证书。</param>
        protected Server(X509Certificate2 serverCertificate)
        {
            ServerCertificate = serverCertificate;
            TypeRegistry.AddTypesToSerializer(typeof(IMessage), TypeRegistry.GetPacketTypes(typeof(IMessage)).ToArray());
        }

        /// <summary>
        /// 开始监听客户端。
        /// </summary>
        /// <param name="port">监听客户端的端口。</param>
        /// <param name="ipv6">如果设置为true，使用双栈套接字允许IPv4/6连接。否则使用仅IPv4套接字。</param>
        /// <param name="enableUPnP">启用自动UPnP端口转发。</param>
        public void Listen(ushort port, bool ipv6, bool enableUPnP)
        {
            if (Listening) return;
            this.Port = port;

            if (enableUPnP)
            {
                _UPnPService = new UPnPService();
                _UPnPService.CreatePortMapAsync(port);
            }

            if (Socket.OSSupportsIPv6 && ipv6)
            {
                _handle = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                _handle.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
                _handle.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
            }
            else
            {
                _handle = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _handle.Bind(new IPEndPoint(IPAddress.Any, port));
            }
            _handle.Listen(1000);

            OnServerState(true);

            _item = new SocketAsyncEventArgs();
            _item.Completed += AcceptClient;

            if (!_handle.AcceptAsync(_item))
                AcceptClient(this, _item);
        }

        /// <summary>
        /// 接受并开始验证传入的客户端。
        /// </summary>
        /// <param name="s">发送者。</param>
        /// <param name="e">异步套接字事件。</param>
        private void AcceptClient(object s, SocketAsyncEventArgs e)
        {
            try
            {
                do
                {
                    switch (e.SocketError)
                    {
                        case SocketError.Success:
                            SslStream sslStream = null;
                            try
                            {
                                Socket clientSocket = e.AcceptSocket;
                                clientSocket.SetKeepAliveEx(KeepAliveInterval, KeepAliveTime);
                                sslStream = new SslStream(new NetworkStream(clientSocket, true), false);
                                // the SslStream owns the socket and on disposing also disposes the NetworkStream and Socket
                                sslStream.BeginAuthenticateAsServer(ServerCertificate, false, SslProtocols.Tls12, false, EndAuthenticateClient,
                                    new PendingClient {Stream = sslStream, EndPoint = (IPEndPoint) clientSocket.RemoteEndPoint});
                            }
                            catch (Exception)
                            {
                                sslStream?.Close();
                            }
                            break;
                        case SocketError.ConnectionReset:
                            break;
                        default:
                            throw new SocketException((int) e.SocketError);
                    }

                    e.AcceptSocket = null; // enable reuse
                } while (!_handle.AcceptAsync(e));
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private class PendingClient
        {
            public SslStream Stream { get; set; }
            public IPEndPoint EndPoint { get; set; }
        }

        /// <summary>
        /// 结束新连接客户端的验证过程。
        /// </summary>
        /// <param name="ar">异步操作的状态。</param>
        private void EndAuthenticateClient(IAsyncResult ar)
        {
            var con = (PendingClient) ar.AsyncState;
            try
            {
                con.Stream.EndAuthenticateAsServer(ar);

                Client client = new Client(_bufferPool, con.Stream, con.EndPoint);
                AddClient(client);
                OnClientState(client, true);
            }
            catch (Exception)
            {
                con.Stream.Close();
            }
        }

        /// <summary>
        /// 将连接的客户端添加到客户端列表中，
        /// 订阅客户端的事件。
        /// </summary>
        /// <param name="client">要添加的客户端。</param>
        private void AddClient(Client client)
        {
            lock (_clientsLock)
            {
                client.ClientState += OnClientState;
                client.ClientRead += OnClientRead;
                client.ClientWrite += OnClientWrite;
                _clients.Add(client);
            }
        }

        /// <summary>
        /// 从客户端列表中移除断开连接的客户端，
        /// 取消订阅客户端的事件。
        /// </summary>
        /// <param name="client">要移除的客户端。</param>
        private void RemoveClient(Client client)
        {
            if (ProcessingDisconnect) return;

            lock (_clientsLock)
            {
                client.ClientState -= OnClientState;
                client.ClientRead -= OnClientRead;
                client.ClientWrite -= OnClientWrite;
                _clients.Remove(client);
            }
        }

        /// <summary>
        /// 断开服务器与所有客户端的连接并停止
        /// 监听（将服务器置于"关闭"状态）。
        /// </summary>
        public void Disconnect()
        {
            if (ProcessingDisconnect) return;
            ProcessingDisconnect = true;

            if (_handle != null)
            {
                _handle.Close();
                _handle = null;
            }

            if (_item != null)
            {
                _item.Dispose();
                _item = null;
            }

            if (_UPnPService != null)
            {
                _UPnPService.DeletePortMapAsync(Port);
                _UPnPService = null;
            }

            lock (_clientsLock)
            {
                while (_clients.Count != 0)
                {
                    try
                    {
                        _clients[0].Disconnect();
                        _clients[0].ClientState -= OnClientState;
                        _clients[0].ClientRead -= OnClientRead;
                        _clients[0].ClientWrite -= OnClientWrite;
                        _clients.RemoveAt(0);
                    }
                    catch
                    {
                    }
                }
            }

            ProcessingDisconnect = false;
            OnServerState(false);
        }
    }
}
