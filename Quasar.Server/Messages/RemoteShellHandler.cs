﻿using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Server.Networking;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程shell交互的消息。
    /// </summary>
    public class RemoteShellHandler : MessageProcessorBase<string>
    {
        /// <summary>
        /// 与此远程shell处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 表示将处理命令错误的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="errorMessage">错误消息。</param>
        public delegate void CommandErrorEventHandler(object sender, string errorMessage);

        /// <summary>
        /// 当命令写入stderr时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event CommandErrorEventHandler CommandError;

        /// <summary>
        /// 报告命令错误。
        /// </summary>
        /// <param name="errorMessage">错误消息。</param>
        private void OnCommandError(string errorMessage)
        {
            SynchronizationContext.Post(val =>
            {
                var handler = CommandError;
                handler?.Invoke(this, (string)val);
            }, errorMessage);
        }

        /// <summary>
        /// 使用给定客户端初始化 <see cref="RemoteShellHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public RemoteShellHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is DoShellExecuteResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoShellExecuteResponse resp:
                    Execute(sender, resp);
                    break;
            }
        }

        /// <summary>
        /// 发送要在客户端的远程shell中执行的命令。
        /// </summary>
        /// <param name="command">要执行的命令。</param>
        public void SendCommand(string command)
        {
            _client.Send(new DoShellExecute {Command = command});
        }

        private void Execute(ISender client, DoShellExecuteResponse message)
        {
            if (message.IsError)
                OnCommandError(message.Output);
            else
                OnReport(message.Output);
        }
    }
}
