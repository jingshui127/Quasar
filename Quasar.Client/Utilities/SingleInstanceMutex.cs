using System;
using System.Threading;

namespace Quasar.Client.Utilities
{
    /// <summary>
    /// 用户范围的互斥锁，确保一次只运行一个实例。
    /// </summary>
    public class SingleInstanceMutex : IDisposable
    {
        /// <summary>
        /// 用于进程同步的互斥锁。
        /// </summary>
        private readonly Mutex _appMutex;

        /// <summary>
        /// 表示互斥锁是在系统上创建的还是已经存在的。
        /// </summary>
        public bool CreatedNew { get; }

        /// <summary>
        /// 确定实例是否已释放且不应再使用。
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 使用给定的互斥锁名称初始化 <see cref="SingleInstanceMutex"/> 的新实例。
        /// </summary>
        /// <param name="name">互斥锁的名称。</param>
        public SingleInstanceMutex(string name)
        {
            _appMutex = new Mutex(false, $"Local\\{name}", out var createdNew);
            CreatedNew = createdNew;
        }

        /// <summary>
        /// 释放此 <see cref="SingleInstanceMutex"/> 使用的所有资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放互斥锁对象。
        /// </summary>
        /// <param name="disposing">如果从 <see cref="Dispose"/> 调用则为 <c>True</c>，如果从终结器调用则为 <c>false</c>。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                _appMutex?.Dispose();
            }

            IsDisposed = true;
        }
    }
}
