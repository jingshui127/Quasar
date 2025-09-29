using Quasar.Common.Enums;
using Quasar.Common.IO;
using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Enums;
using Quasar.Server.Models;
using Quasar.Server.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程文件和目录交互的消息。
    /// </summary>
    public class FileManagerHandler : MessageProcessorBase<string>, IDisposable
    {
        /// <summary>
        /// 表示将处理驱动器更改的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="drives">所有当前可用的驱动器。</param>
        public delegate void DrivesChangedEventHandler(object sender, Drive[] drives);

        /// <summary>
        /// 表示将处理目录更改的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="remotePath">目录的远程路径。</param>
        /// <param name="items">目录内容。</param>
        public delegate void DirectoryChangedEventHandler(object sender, string remotePath, FileSystemEntry[] items);

        /// <summary>
        /// 表示将处理文件传输更新的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="transfer">更新的文件传输。</param>
        public delegate void FileTransferUpdatedEventHandler(object sender, FileTransfer transfer);

        /// <summary>
        /// 当驱动器更改时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event DrivesChangedEventHandler DrivesChanged;

        /// <summary>
        /// 当目录更改时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event DirectoryChangedEventHandler DirectoryChanged;

        /// <summary>
        /// 当文件传输更新时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event FileTransferUpdatedEventHandler FileTransferUpdated;

        /// <summary>
        /// 报告更改的远程驱动器。
        /// </summary>
        /// <param name="drives">当前远程驱动器。</param>
        private void OnDrivesChanged(Drive[] drives)
        {
            SynchronizationContext.Post(d =>
            {
                var handler = DrivesChanged;
                handler?.Invoke(this, (Drive[])d);
            }, drives);
        }

        /// <summary>
        /// 报告目录更改。
        /// </summary>
        /// <param name="remotePath">目录的远程路径。</param>
        /// <param name="items">目录内容。</param>
        private void OnDirectoryChanged(string remotePath, FileSystemEntry[] items)
        {
            SynchronizationContext.Post(i =>
            {
                var handler = DirectoryChanged;
                handler?.Invoke(this, remotePath, (FileSystemEntry[])i);
            }, items);
        }

        /// <summary>
        /// 报告更新的文件传输。
        /// </summary>
        /// <param name="transfer">更新的文件传输。</param>
        private void OnFileTransferUpdated(FileTransfer transfer)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = FileTransferUpdated;
                handler?.Invoke(this, (FileTransfer)t);
            }, transfer.Clone());
        }

        /// <summary>
        /// 跟踪所有活动的文件传输。已完成或已取消的传输将被移除。
        /// </summary>
        private readonly List<FileTransfer> _activeFileTransfers = new List<FileTransfer>();

        /// <summary>
        /// 在lock语句中使用以同步UI线程和线程池之间的访问。
        /// </summary>
        private readonly object _syncLock = new object();

        /// <summary>
        /// 与此文件管理器处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 仅用于允许两个同时的文件上传。
        /// </summary>
        private readonly Semaphore _limitThreads = new Semaphore(2, 2);

        /// <summary>
        /// 客户端的基本下载目录路径。
        /// </summary>
        private readonly string _baseDownloadPath;

        private readonly TaskManagerHandler _taskManagerHandler;

        /// <summary>
        /// 使用给定的客户端初始化 <see cref="FileManagerHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        /// <param name="subDirectory">可选的子目录名称。</param>
        public FileManagerHandler(Client client, string subDirectory = "") : base(true)
        {
            _client = client;
            _baseDownloadPath = Path.Combine(client.Value.DownloadDirectory, subDirectory);
            _taskManagerHandler = new TaskManagerHandler(client);
            _taskManagerHandler.ProcessActionPerformed += ProcessActionPerformed;
            MessageHandler.Register(_taskManagerHandler);
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is FileTransferChunk ||
                                                             message is FileTransferCancel ||
                                                             message is FileTransferComplete ||
                                                             message is GetDrivesResponse ||
                                                             message is GetDirectoryResponse ||
                                                             message is SetStatusFileManager;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case FileTransferChunk file:
                    Execute(sender, file);
                    break;
                case FileTransferCancel cancel:
                    Execute(sender, cancel);
                    break;
                case FileTransferComplete complete:
                    Execute(sender, complete);
                    break;
                case GetDrivesResponse drive:
                    Execute(sender, drive);
                    break;
                case GetDirectoryResponse directory:
                    Execute(sender, directory);
                    break;
                case SetStatusFileManager status:
                    Execute(sender, status);
                    break;
            }
        }

        /// <summary>
        /// 开始从客户端下载文件。
        /// </summary>
        /// <param name="remotePath">要下载的文件的远程路径。</param>
        /// <param name="localFileName">本地文件名。</param>
        /// <param name="overwrite">用新下载的文件覆盖本地文件。</param>
        public void BeginDownloadFile(string remotePath, string localFileName = "", bool overwrite = false)
        {
            if (string.IsNullOrEmpty(remotePath))
                return;

            int id = GetUniqueFileTransferId();

            if (!Directory.Exists(_baseDownloadPath))
                Directory.CreateDirectory(_baseDownloadPath);

            string fileName = string.IsNullOrEmpty(localFileName) ? Path.GetFileName(remotePath) : localFileName;
            string localPath = Path.Combine(_baseDownloadPath, fileName);

            int i = 1;
            while (!overwrite && File.Exists(localPath))
            {
                // rename file if it exists already
                var newFileName = string.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(localPath), i, Path.GetExtension(localPath));
                localPath = Path.Combine(_baseDownloadPath, newFileName);
                i++;
            }

            var transfer = new FileTransfer
            {
                Id = id,
                Type = TransferType.Download,
                LocalPath = localPath,
                RemotePath = remotePath,
                Status = "Pending...",
                //Size = fileSize, TODO: Add file size here
                TransferredSize = 0
            };

            try
            {
                transfer.FileSplit = new FileSplit(transfer.LocalPath, FileAccess.Write);
            }
            catch (Exception)
            {
                transfer.Status = "Error writing file";
                OnFileTransferUpdated(transfer);
                return;
            }

            lock (_syncLock)
            {
                _activeFileTransfers.Add(transfer);
            }

            OnFileTransferUpdated(transfer);

            _client.Send(new FileTransferRequest {RemotePath = remotePath, Id = id});
        }

        /// <summary>
        /// 开始向客户端上传文件。
        /// </summary>
        /// <param name="localPath">要上传的文件的本地路径。</param>
        /// <param name="remotePath">将上传的文件保存到此远程路径。如果为空，则生成临时文件名。</param>
        public void BeginUploadFile(string localPath, string remotePath = "")
        {
            new Thread(() =>
            {
                int id = GetUniqueFileTransferId();

                FileTransfer transfer = new FileTransfer
                {
                    Id = id,
                    Type = TransferType.Upload,
                    LocalPath = localPath,
                    RemotePath = remotePath,
                    Status = "Pending...",
                    TransferredSize = 0
                };

                try
                {
                    transfer.FileSplit = new FileSplit(localPath, FileAccess.Read);
                }
                catch (Exception)
                {
                    transfer.Status = "Error reading file";
                    OnFileTransferUpdated(transfer);
                    return;
                }

                transfer.Size = transfer.FileSplit.FileSize;

                lock (_syncLock)
                {
                    _activeFileTransfers.Add(transfer);
                }

                transfer.Size = transfer.FileSplit.FileSize;
                OnFileTransferUpdated(transfer);

                _limitThreads.WaitOne();
                try
                {
                    foreach (var chunk in transfer.FileSplit)
                    {
                        transfer.TransferredSize += chunk.Data.Length;
                        decimal progress = transfer.Size == 0 ? 100 : Math.Round((decimal)((double)transfer.TransferredSize / (double)transfer.Size * 100.0), 2);
                        transfer.Status = $"Uploading...({progress}%)";
                        OnFileTransferUpdated(transfer);

                        bool transferCanceled;
                        lock (_syncLock)
                        {
                            transferCanceled = _activeFileTransfers.Count(f => f.Id == transfer.Id) == 0;
                        }

                        if (transferCanceled)
                        {
                            transfer.Status = "Canceled";
                            OnFileTransferUpdated(transfer);
                            _limitThreads.Release();
                            return;
                        }

                        // TODO: blocking sending might not be required, needs further testing
                        _client.SendBlocking(new FileTransferChunk
                        {
                            Id = id,
                            Chunk = chunk,
                            FilePath = remotePath,
                            FileSize = transfer.Size
                        });
                    }
                }
                catch (Exception)
                {
                    lock (_syncLock)
                    {
                        // if transfer is already cancelled, just return
                        if (_activeFileTransfers.Count(f => f.Id == transfer.Id) == 0)
                        {
                            _limitThreads.Release();
                            return;
                        }
                    }
                    transfer.Status = "Error reading file";
                    OnFileTransferUpdated(transfer);
                    CancelFileTransfer(transfer.Id);
                    _limitThreads.Release();
                    return;
                }

                _limitThreads.Release();
            }).Start();
        }

        /// <summary>
        /// 取消文件传输。
        /// </summary>
        /// <param name="transferId">要取消的文件传输的ID。</param>
        public void CancelFileTransfer(int transferId)
        {
            _client.Send(new FileTransferCancel {Id = transferId});
        }

        /// <summary>
        /// 重命名远程文件或目录。
        /// </summary>
        /// <param name="remotePath">要重命名的远程文件或目录路径。</param>
        /// <param name="newPath">远程文件或目录路径的新名称。</param>
        /// <param name="type">文件类型（文件或目录）。</param>
        public void RenameFile(string remotePath, string newPath, FileType type)
        {
            _client.Send(new DoPathRename
            {
                Path = remotePath,
                NewPath = newPath,
                PathType = type
            });
        }

        /// <summary>
        /// 删除远程文件或目录。
        /// </summary>
        /// <param name="remotePath">远程文件或目录路径。</param>
        /// <param name="type">文件类型（文件或目录）。</param>
        public void DeleteFile(string remotePath, FileType type)
        {
            _client.Send(new DoPathDelete {Path = remotePath, PathType = type});
        }

        /// <summary>
        /// 启动远程进程。
        /// </summary>
        /// <param name="remotePath">用于启动新进程的远程路径。</param>
        public void StartProcess(string remotePath)
        {
            _taskManagerHandler.StartProcess(remotePath);
        }

        /// <summary>
        /// 将项目添加到客户端的启动项。
        /// </summary>
        /// <param name="item">要添加的启动项。</param>
        public void AddToStartup(StartupItem item)
        {
            _client.Send(new DoStartupItemAdd {StartupItem = item});
        }

        /// <summary>
        /// 获取远程路径的目录内容。
        /// </summary>
        /// <param name="remotePath">目录的远程路径。</param>
        public void GetDirectoryContents(string remotePath)
        {
            _client.Send(new GetDirectory {RemotePath = remotePath});
        }

        /// <summary>
        /// 刷新远程驱动器。
        /// </summary>
        public void RefreshDrives()
        {
            _client.Send(new GetDrives());
        }

        private void Execute(ISender client, FileTransferChunk message)
        {
            FileTransfer transfer;
            lock (_syncLock)
            {
                transfer = _activeFileTransfers.FirstOrDefault(t => t.Id == message.Id);
            }

            if (transfer == null)
                return;

            transfer.Size = message.FileSize;
            transfer.TransferredSize += message.Chunk.Data.Length;

            try
            {
                transfer.FileSplit.WriteChunk(message.Chunk);
            }
            catch (Exception)
            {
                transfer.Status = "Error writing file";
                OnFileTransferUpdated(transfer);
                CancelFileTransfer(transfer.Id);
                return;
            }

            decimal progress = transfer.Size == 0 ? 100 : Math.Round((decimal) ((double) transfer.TransferredSize / (double) transfer.Size * 100.0), 2);
            transfer.Status = $"Downloading...({progress}%)";

            OnFileTransferUpdated(transfer);
        }

        private void Execute(ISender client, FileTransferCancel message)
        {
            FileTransfer transfer;
            lock (_syncLock)
            {
                transfer = _activeFileTransfers.FirstOrDefault(t => t.Id == message.Id);
            }

            if (transfer != null)
            {
                transfer.Status = message.Reason;
                OnFileTransferUpdated(transfer);
                RemoveFileTransfer(transfer.Id);
                // don't keep un-finished files
                if (transfer.Type == TransferType.Download)
                    File.Delete(transfer.LocalPath);
            }
        }

        private void Execute(ISender client, FileTransferComplete message)
        {
            FileTransfer transfer;
            lock (_syncLock)
            {
                transfer = _activeFileTransfers.FirstOrDefault(t => t.Id == message.Id);
            }

            if (transfer != null)
            {
                transfer.RemotePath = message.FilePath; // required for temporary file names generated on the client
                transfer.Status = "Completed";
                RemoveFileTransfer(transfer.Id);
                OnFileTransferUpdated(transfer);
            }
        }

        private void Execute(ISender client, GetDrivesResponse message)
        {
            if (message.Drives?.Length == 0)
                return;

            OnDrivesChanged(message.Drives);
        }
        
        private void Execute(ISender client, GetDirectoryResponse message)
        {
            if (message.Items == null)
            {
                message.Items = new FileSystemEntry[0];
            }
            OnDirectoryChanged(message.RemotePath, message.Items);
        }

        private void Execute(ISender client, SetStatusFileManager message)
        {
            OnReport(message.Message);
        }

        private void ProcessActionPerformed(object sender, ProcessAction action, bool result)
        {
            if (action != ProcessAction.Start) return;
            OnReport(result ? "进程启动成功" : "进程启动失败");
        }

        /// <summary>
        /// 根据传输ID移除文件传输。
        /// </summary>
        /// <param name="transferId">文件传输ID。</param>
        private void RemoveFileTransfer(int transferId)
        {
            lock (_syncLock)
            {
                var transfer = _activeFileTransfers.FirstOrDefault(t => t.Id == transferId);
                transfer?.FileSplit?.Dispose();
                _activeFileTransfers.RemoveAll(s => s.Id == transferId);
            }
        }

        /// <summary>
        /// 生成唯一的文件传输ID。
        /// </summary>
        /// <returns>唯一的文件传输ID。</returns>
        private int GetUniqueFileTransferId()
        {
            int id;
            lock (_syncLock)
            {
                do
                {
                    id = FileTransfer.GetRandomTransferId();
                    // generate new id until we have a unique one
                } while (_activeFileTransfers.Any(f => f.Id == id));
            }

            return id;
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
                lock (_syncLock)
                {
                    foreach (var transfer in _activeFileTransfers)
                    {
                        _client.Send(new FileTransferCancel {Id = transfer.Id});
                        transfer.FileSplit?.Dispose();
                        if (transfer.Type == TransferType.Download)
                            File.Delete(transfer.LocalPath);
                    }

                    _activeFileTransfers.Clear();
                }

                MessageHandler.Unregister(_taskManagerHandler);
                _taskManagerHandler.ProcessActionPerformed -= ProcessActionPerformed;
            }
        }
    }
}
