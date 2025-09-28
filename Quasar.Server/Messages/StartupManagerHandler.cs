﻿using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Networking;
using System.Collections.Generic;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程启动任务交互的消息。
    /// </summary>
    public class StartupManagerHandler : MessageProcessorBase<List<StartupItem>>
    {
        /// <summary>
        /// 与此启动管理器处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="StartupManagerHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public StartupManagerHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetStartupItemsResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetStartupItemsResponse items:
                    Execute(sender, items);
                    break;
            }
        }

        /// <summary>
        /// 刷新当前的启动项。
        /// </summary>
        public void RefreshStartupItems()
        {
            _client.Send(new GetStartupItems());
        }

        /// <summary>
        /// 从启动项中移除一个项目。
        /// </summary>
        /// <param name="item">要移除的启动项。</param>
        public void RemoveStartupItem(StartupItem item)
        {
            _client.Send(new DoStartupItemRemove {StartupItem = item});
        }

        /// <summary>
        /// 添加一个项目到启动项。
        /// </summary>
        /// <param name="item">要添加的启动项。</param>
        public void AddStartupItem(StartupItem item)
        {
            _client.Send(new DoStartupItemAdd {StartupItem = item});
        }

        private void Execute(ISender client, GetStartupItemsResponse message)
        {
            OnReport(message.StartupItems);
        }
    }
}
