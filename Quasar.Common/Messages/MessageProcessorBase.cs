using Quasar.Common.Networking;
using System;
using System.Threading;

namespace Quasar.Common.Messages
{
    /// <summary>
    /// 提供一个消息处理器实现，该实现提供进度报告回调。
    /// </summary>
    /// <typeparam name="T">指定进度报告值的类型。</typeparam>
    /// <remarks>
    /// 任何注册到<see cref="ProgressChanged"/>事件的事件处理程序都将通过在构造实例时选择的
    /// <see cref="System.Threading.SynchronizationContext"/>实例来调用。
    /// </remarks>
    public abstract class MessageProcessorBase<T> : IMessageProcessor, IProgress<T>
    {
        /// <summary>
        /// 在构造时选择的同步上下文。
        /// </summary>
        protected readonly SynchronizationContext SynchronizationContext;

        /// <summary>
        /// 用于向同步上下文发布调用的缓存委托。
        /// </summary>
        private readonly SendOrPostCallback _invokeReportProgressHandlers;

        /// <summary>
        /// 表示将处理进度更新的方法。
        /// </summary>
        /// <param name="sender">更新进度的消息处理器。</param>
        /// <param name="value">新的进度。</param>
        public delegate void ReportProgressEventHandler(object sender, T value);

        /// <summary>
        /// 为每个报告的进度值引发。
        /// </summary>
        /// <remarks>
        /// 注册到此事件的处理程序将在构造实例时选择的
        /// <see cref="System.Threading.SynchronizationContext"/>上调用。
        /// </remarks>
        public event ReportProgressEventHandler ProgressChanged;

        /// <summary>
        /// 报告进度更改。
        /// </summary>
        /// <param name="value">更新进度的值。</param>
        protected virtual void OnReport(T value)
        {
            // 如果没有处理程序，就不要费心通过同步上下文。
            // 在回调内部，我们需要再次检查，以防
            // 事件处理程序在此期间被移除。
            var handler = ProgressChanged;
            if (handler != null)
            {
                SynchronizationContext.Post(_invokeReportProgressHandlers, value);
            }
        }

        /// <summary>
        /// 初始化<see cref="MessageProcessorBase{T}"/>
        /// </summary>
        /// <param name="useCurrentContext">
        /// 如果此值为<c>false</c>，进度回调将在ThreadPool上调用。
        /// 否则将使用当前的SynchronizationContext。
        /// </param>
        protected MessageProcessorBase(bool useCurrentContext)
        {
            _invokeReportProgressHandlers = InvokeReportProgressHandlers;
            SynchronizationContext = useCurrentContext ? SynchronizationContext.Current : ProgressStatics.DefaultContext;
        }

        /// <summary>
        /// 调用进度事件回调。
        /// </summary>
        /// <param name="state">进度值。</param>
        private void InvokeReportProgressHandlers(object state)
        {
            var handler = ProgressChanged;
            handler?.Invoke(this, (T)state);
        }

        /// <inheritdoc />
        public abstract bool CanExecute(IMessage message);

        /// <inheritdoc />
        public abstract bool CanExecuteFrom(ISender sender);

        /// <inheritdoc />
        public abstract void Execute(ISender sender, IMessage message);

        void IProgress<T>.Report(T value) => OnReport(value);
    }

    /// <summary>
    /// 保存<see cref="MessageProcessorBase{T}"/>的静态值。
    /// </summary>
    /// <remarks>
    /// 这避免了每个类型T有一个静态实例。
    /// </remarks>
    internal static class ProgressStatics
    {
        /// <summary>
        /// 一个以<see cref="ThreadPool"/>为目标的默认同步上下文。
        /// </summary>
        internal static readonly SynchronizationContext DefaultContext = new SynchronizationContext();
    }
}
