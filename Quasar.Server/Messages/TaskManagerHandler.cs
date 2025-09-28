﻿using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Networking;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程任务交互的消息。
    /// </summary>
    public class TaskManagerHandler : MessageProcessorBase<Process[]>
    {
        /// <summary>
        /// 表示将处理进程操作结果的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="action">执行的进程操作。</param>
        /// <param name="result">执行的进程操作的结果。</param>
        public delegate void ProcessActionPerformedEventHandler(object sender, ProcessAction action, bool result);

        /// <summary>
        /// 当接收到已启动进程的结果时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event ProcessActionPerformedEventHandler ProcessActionPerformed;

        /// <summary>
        /// 报告已启动进程的结果。
        /// </summary>
        /// <param name="action">执行的进程操作。</param>
        /// <param name="result">执行的进程操作的结果。</param>
        private void OnProcessActionPerformed(ProcessAction action, bool result)
        {
            SynchronizationContext.Post(r =>
            {
                var handler = ProcessActionPerformed;
                handler?.Invoke(this, action, (bool)r);
            }, result);
        }

        /// <summary>
        /// 与此远程执行处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="TaskManagerHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public TaskManagerHandler(Client client) : base(true)
        {
            _client = client;
        }

        public override bool CanExecute(IMessage message) => message is DoProcessResponse ||
                                                             message is GetProcessesResponse;

        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoProcessResponse execResp:
                    Execute(sender, execResp);
                    break;
                case GetProcessesResponse procResp:
                    Execute(sender, procResp);
                    break;
            }
        }

        /// <summary>
        /// 远程启动一个新进程。
        /// </summary>
        /// <param name="remotePath">用于启动新进程的远程路径。</param>
        /// <param name="isUpdate">决定进程是否为客户端更新。</param>
        public void StartProcess(string remotePath, bool isUpdate = false)
        {
            _client.Send(new DoProcessStart { FilePath = remotePath, IsUpdate = isUpdate });
        }

        /// <summary>
        /// 从网络下载文件并远程执行。
        /// </summary>
        /// <param name="url">要下载和执行的URL。</param>
        /// <param name="isUpdate">决定文件是否为客户端更新。</param>
        public void StartProcessFromWeb(string url, bool isUpdate = false)
        {
            _client.Send(new DoProcessStart { DownloadUrl = url, IsUpdate = isUpdate});
        }

        /// <summary>
        /// 刷新当前已启动的进程。
        /// </summary>
        public void RefreshProcesses()
        {
            _client.Send(new GetProcesses());
        }

        /// <summary>
        /// 根据进程ID结束已启动的进程。
        /// </summary>
        /// <param name="pid">要结束的进程ID。</param>
        public void EndProcess(int pid)
        {
            _client.Send(new DoProcessEnd { Pid = pid });
        }

        private void Execute(ISender client, DoProcessResponse message)
        {
            OnProcessActionPerformed(message.Action, message.Result);
        }

        private void Execute(ISender client, GetProcessesResponse message)
        {
            OnReport(message.Processes);
        }
    }
}
