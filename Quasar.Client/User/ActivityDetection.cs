using Quasar.Client.Helper;
using Quasar.Client.Networking;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using System;
using System.Threading;

namespace Quasar.Client.User
{
    /// <summary>
    /// 提供用户活动检测，并在状态改变时发送 <see cref="SetUserStatus"/> 消息。
    /// </summary>
    public class ActivityDetection : IDisposable
    {
        /// <summary>
        /// 存储最后的用户状态以检测变化。
        /// </summary>
        private UserStatus _lastUserStatus;

        /// <summary>
        /// 用于与服务器通信的客户端。
        /// </summary>
        private readonly QuasarClient _client;

        /// <summary>
        /// 创建 <see cref="_token"/> 并发出取消信号。
        /// </summary>
        private readonly CancellationTokenSource _tokenSource;

        /// <summary>
        /// 用于检查取消的令牌。
        /// </summary>
        private readonly CancellationToken _token;

        /// <summary>
        /// 使用给定的客户端初始化 <see cref="ActivityDetection"/> 的新实例。
        /// </summary>
        /// <param name="client">互斥锁的名称。</param>
        public ActivityDetection(QuasarClient client)
        {
            _client = client;
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            client.ClientState += OnClientStateChange;
        }

        private void OnClientStateChange(Networking.Client s, bool connected)
        {
            // reset user status
            if (connected)
                _lastUserStatus = UserStatus.Active;
        }

        /// <summary>
        /// 启动用户活动检测。
        /// </summary>
        public void Start()
        {
            new Thread(UserActivityThread).Start();
        }

        /// <summary>
        /// 检查用户活动变化，在状态改变时向 <see cref="_client"/> 发送 <see cref="SetUserStatus"/>。
        /// </summary>
        private void UserActivityThread()
        {
            try
            {
                if (IsUserIdle())
                {
                    if (_lastUserStatus != UserStatus.Idle)
                    {
                        _lastUserStatus = UserStatus.Idle;
                        _client.Send(new SetUserStatus { Message = _lastUserStatus });
                    }
                }
                else
                {
                    if (_lastUserStatus != UserStatus.Active)
                    {
                        _lastUserStatus = UserStatus.Active;
                        _client.Send(new SetUserStatus { Message = _lastUserStatus });
                    }
                }
            }
            catch (Exception e) when (e is NullReferenceException || e is ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// 如果最后一次用户输入是在10分钟前，则确定用户是否空闲。
        /// </summary>
        /// <returns>如果用户空闲则返回 <c>True</c>，否则返回 <c>false</c>。</returns>
        private bool IsUserIdle()
        {
            var ticks = Environment.TickCount;

            var idleTime = ticks - NativeMethodsHelper.GetLastInputInfoTickCount();

            idleTime = ((idleTime > 0) ? (idleTime / 1000) : 0);

            return (idleTime > 600); // idle for 10 minutes
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
                _client.ClientState -= OnClientStateChange;
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }
    }
}
