﻿﻿﻿using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Threading;

namespace Quasar.Server.Networking
{
    public class Client : IEquatable<Client>, ISender
    {
        /// <summary>
        /// 当客户端状态更改时发生。
        /// </summary>
        public event ClientStateEventHandler ClientState;

        /// <summary>
        /// 表示将处理客户端状态更改的方法。
        /// </summary>
        /// <param name="s">更改状态的客户端。</param>
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
        /// 当从客户端接收到消息时发生。
        /// </summary>
        public event ClientReadEventHandler ClientRead;

        /// <summary>
        /// 表示将处理从客户端接收的消息的方法。
        /// </summary>
        /// <param name="s">接收消息的客户端。</param>
        /// <param name="message">客户端接收的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        public delegate void ClientReadEventHandler(Client s, IMessage message, int messageLength);

        /// <summary>
        /// 触发一个事件，通知订阅者已从客户端接收到消息。
        /// </summary>
        /// <param name="message">客户端接收的消息。</param>
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
        /// 表示将处理发送消息的方法。
        /// </summary>
        /// <param name="s">发送消息的客户端。</param>
        /// <param name="message">客户端发送的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        public delegate void ClientWriteEventHandler(Client s, IMessage message, int messageLength);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已发送消息。
        /// </summary>
        /// <param name="message">客户端发送的消息。</param>
        /// <param name="messageLength">消息的长度。</param>
        private void OnClientWrite(IMessage message, int messageLength)
        {
            var handler = ClientWrite;
            handler?.Invoke(this, message, messageLength);
        }

        public static bool operator ==(Client c1, Client c2)
        {
            if (ReferenceEquals(c1, null))
                return ReferenceEquals(c2, null);

            return c1.Equals(c2);
        }

        public static bool operator !=(Client c1, Client c2)
        {
            return !(c1 == c2);
        }

        /// <summary>
        /// 检查客户端是否相等。
        /// </summary>
        /// <param name="other">要比较的客户端。</param>
        /// <returns>如果相等则为True，否则为False。</returns>
        public bool Equals(Client other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            try
            {
                // the port is always unique for each client
                return this.EndPoint.Port.Equals(other.EndPoint.Port);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Client);
        }

        /// <summary>
        /// 返回此实例的哈希码。
        /// </summary>
        /// <returns>当前实例的哈希码。</returns>
        public override int GetHashCode()
        {
            return this.EndPoint.GetHashCode();
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
        /// 用于通信的流。
        /// </summary>
        private readonly SslStream _stream;

        /// <summary>
        /// 用于保存客户端接收缓冲区的缓冲池。
        /// </summary>
        private readonly BufferPool _bufferPool;

        /// <summary>
        /// 保存要发送消息的队列。
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
        /// 保存要读取缓冲区的队列。
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

        // receive info
        private int _readOffset;
        private int _writeOffset;
        private int _readableDataLen;
        private int _payloadLen;
        private ReceiveType _receiveState = ReceiveType.Header;

        /// <summary>
        /// 客户端连接的时间。
        /// </summary>
        public DateTime ConnectedTime { get; }

        /// <summary>
        /// 客户端的连接状态。
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// 确定客户端是否已识别。
        /// </summary>
        public bool Identified { get; set; }

        /// <summary>
        /// 存储用户的值。
        /// </summary>
        public UserState Value { get; set; }

        /// <summary>
        /// 客户端连接的端点。
        /// </summary>
        public IPEndPoint EndPoint { get; }

        /// <summary>
        /// 客户端传入消息的缓冲区。
        /// </summary>
        private readonly byte[] _readBuffer;

        /// <summary>
        /// 客户端传入有效载荷的缓冲区。
        /// </summary>
        private byte[] _payloadBuffer;

        /// <summary>
        /// 头部大小（以字节为单位）。
        /// </summary>
        private const int HeaderSize = 4;  // 4 B

        /// <summary>
        /// 消息的最大大小（以字节为单位）。
        /// </summary>
        private const int MaxMessageSize = (1024 * 1024) * 5; // 5 MB

        /// <summary>
        /// 互斥锁防止在<see cref="_stream"/>上进行多个同时写入操作。
        /// </summary>
        private readonly Mutex _singleWriteMutex = new Mutex();

        public Client(BufferPool bufferPool, SslStream stream, IPEndPoint endPoint)
        {
            try
            {
                Identified = false;
                Value = new UserState();
                EndPoint = endPoint;
                ConnectedTime = DateTime.UtcNow;
                _stream = stream;
                _bufferPool = bufferPool;
                _readBuffer = _bufferPool.GetBuffer();
                _stream.BeginRead(_readBuffer, 0, _readBuffer.Length, AsyncReceive, null);
                OnClientState(true);
            }
            catch (Exception)
            {
                Disconnect();
            }
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
            catch (Exception)
            {
                Disconnect();
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
            catch (Exception)
            {
                Disconnect();
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
                                    _payloadBuffer = new byte[HeaderSize];

                                if (_readableDataLen + _writeOffset >= HeaderSize)
                                {
                                    // completely received header
                                    int headerLength = HeaderSize - _writeOffset;

                                    try
                                    {
                                        Array.Copy(readBuffer, _readOffset, _payloadBuffer, _writeOffset, headerLength);

                                        _payloadLen = BitConverter.ToInt32(_payloadBuffer, _readOffset);

                                        if (_payloadLen <= 0 || _payloadLen > MaxMessageSize)
                                            throw new Exception("invalid header");

                                        // try to re-use old payload buffers which fit
                                        if (_payloadBuffer.Length <= _payloadLen + HeaderSize)
                                            Array.Resize(ref _payloadBuffer, _payloadLen + HeaderSize);
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
                                    // received only a part of the header
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
                                int length = (_writeOffset - HeaderSize + _readableDataLen) >= _payloadLen
                                    ?  _payloadLen - (_writeOffset - HeaderSize)
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
                                
                                if (_writeOffset - HeaderSize == _payloadLen)
                                {
                                    // completely received payload
                                    try
                                    {
                                        using (PayloadReader pr = new PayloadReader(_payloadBuffer, _payloadLen + HeaderSize, false))
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
        /// 向连接的客户端发送消息。
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
        /// 向连接的客户端发送消息。
        /// 阻塞线程直到消息发送完毕。
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
        /// 写入操作。
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
        /// 断开客户端与服务器的连接并释放
        /// 与客户端关联的资源。
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
                _singleWriteMutex.Dispose();

                _bufferPool.ReturnBuffer(_readBuffer);
            }

            OnClientState(false);
        }
    }
}
