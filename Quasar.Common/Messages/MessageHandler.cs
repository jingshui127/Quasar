﻿using Quasar.Common.Networking;
using System.Collections.Generic;
using System.Linq;

namespace Quasar.Common.Messages
{
    /// <summary>
    /// 处理 <see cref="IMessageProcessor"/> 的注册和 <see cref="IMessage"/> 的处理。
    /// </summary>
    public static class MessageHandler
    {
        /// <summary>
        /// 已注册的 <see cref="IMessageProcessor"/> 列表。
        /// </summary>
        private static readonly List<IMessageProcessor> Processors = new List<IMessageProcessor>();

        /// <summary>
        /// 在 lock 语句中使用，用于同步线程间对 <see cref="Processors"/> 的访问。
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        /// 将 <see cref="IMessageProcessor"/> 注册到可用的 <see cref="Processors"/> 中。
        /// </summary>
        /// <param name="proc">要注册的 <see cref="IMessageProcessor"/>。</param>
        public static void Register(IMessageProcessor proc)
        {
            lock (SyncLock)
            {
                if (Processors.Contains(proc)) return;
                Processors.Add(proc);
            }
        }

        /// <summary>
        /// 从可用的 <see cref="Processors"/> 中注销 <see cref="IMessageProcessor"/>。
        /// </summary>
        /// <param name="proc"></param>
        public static void Unregister(IMessageProcessor proc)
        {
            lock (SyncLock)
            {
                Processors.Remove(proc);
            }
        }

        /// <summary>
        /// 将接收到的 <see cref="IMessage"/> 转发给适当的 <see cref="IMessageProcessor"/> 来执行。
        /// </summary>
        /// <param name="sender">消息的发送者。</param>
        /// <param name="msg">接收到的消息。</param>
        public static void Process(ISender sender, IMessage msg)
        {
            IEnumerable<IMessageProcessor> availableProcessors;
            lock (SyncLock)
            {
                // select appropriate message processors
                availableProcessors = Processors.Where(x => x.CanExecute(msg) && x.CanExecuteFrom(sender)).ToList();
                // ToList() is required to retrieve a thread-safe enumerator representing a moment-in-time snapshot of the message processors
            }

            foreach (var executor in availableProcessors)
                executor.Execute(sender, msg);
        }
    }
}
