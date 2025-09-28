﻿using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Quasar.Common.Extensions
{
    /// <summary>
    /// Socket KeepAlive 扩展
    /// </summary>
    /// <Author>Abdullah Saleem</Author>
    /// <Email>a.saleem2993@gmail.com</Email>
    public static class SocketExtensions
    {
        /// <summary>
        ///     SetKeepAliveEx 方法使用的结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct TcpKeepAlive
        {
            internal uint onoff;
            internal uint keepalivetime;
            internal uint keepaliveinterval;
        };

        /// <summary>
        ///     为当前TCP连接设置Keep-Alive值
        /// </summary>
        /// <param name="socket">当前socket实例</param>
        /// <param name="keepAliveInterval">指定当没有收到响应时TCP重复发送keep-alive传输的频率。TCP发送keep-alive传输以验证空闲连接是否仍然活跃。这可以防止TCP意外断开活动线路。</param>
        /// <param name="keepAliveTime">指定TCP发送keep-alive传输的频率。TCP发送keep-alive传输以验证空闲连接是否仍然活跃。当远程系统响应TCP时使用此条目。否则，传输间隔由keepAliveInterval条目的值确定。</param>
        public static void SetKeepAliveEx(this Socket socket, uint keepAliveInterval, uint keepAliveTime)
        {
            var keepAlive = new TcpKeepAlive
            {
                onoff = 1,
                keepaliveinterval = keepAliveInterval,
                keepalivetime = keepAliveTime
            };
            int size = Marshal.SizeOf(keepAlive);
            IntPtr keepAlivePtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(keepAlive, keepAlivePtr, true);
            var buffer = new byte[size];
            Marshal.Copy(keepAlivePtr, buffer, 0, size);
            Marshal.FreeHGlobal(keepAlivePtr);
            socket.IOControl(IOControlCode.KeepAliveValues, buffer, null);
        }
    }
}
