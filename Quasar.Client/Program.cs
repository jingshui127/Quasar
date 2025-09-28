﻿using Quasar.Client.IO;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // 启用 TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 设置未处理异常模式，强制所有 Windows Forms 错误通过我们的处理程序
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // 添加处理 UI 线程异常的事件处理程序
            Application.ThreadException += HandleThreadException;

            // 添加处理非 UI 线程异常的事件处理程序
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QuasarApplication());
        }

        private static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Debug.WriteLine(e);
            try
            {
                string batchFile = BatchFile.CreateRestartBatch(Application.ExecutablePath);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = batchFile
                };
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 通过重启应用程序来处理未处理的异常，并希望它们不再发生。
        /// </summary>
        /// <param name="sender">未处理异常事件的源。</param>
        /// <param name="e">异常事件参数。</param>
        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                Debug.WriteLine(e);
                try
                {
                    string batchFile = BatchFile.CreateRestartBatch(Application.ExecutablePath);

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        FileName = batchFile
                    };
                    Process.Start(startInfo);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
