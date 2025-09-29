﻿using Quasar.Common.Networking;

namespace Quasar.Common.Messages
{
    /// <summary>
    /// 提供处理消息的基本功能。
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// 决定此消息处理器是否可以执行指定的消息。
        /// </summary>
        /// <param name="message">要执行的消息。</param>
        /// <returns><c>True</c>如果此消息处理器可以执行该消息，则为<c>true</c>，否则为<c>false</c>。</returns>
        bool CanExecute(IMessage message);

        /// <summary>
        /// 决定此消息处理器是否可以执行从发送方接收的消息。
        /// </summary>
        /// <param name="sender">消息的发送方。</param>
        /// <returns><c>True</c>如果此消息处理器可以执行来自发送方的消息，则为<c>true</c>，否则为<c>false</c>。</returns>
        bool CanExecuteFrom(ISender sender);

        /// <summary>
        /// 执行接收到的消息。
        /// </summary>
        /// <param name="sender">此消息的发送方。</param>
        /// <param name="message">接收到的消息。</param>
        void Execute(ISender sender, IMessage message);
    }
}
