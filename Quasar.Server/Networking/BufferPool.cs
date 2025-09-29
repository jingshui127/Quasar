﻿using System;
using System.Collections.Generic;

namespace Quasar.Server.Networking
{
    /// <summary>
    /// 实现字节数组池以提高解析数据时的分配性能。
    /// </summary>
    /// <threadsafety>此类型对于多线程操作是安全的。</threadsafety>
    public class BufferPool
    {
        private readonly int _bufferLength;
        private int _bufferCount;
        private readonly Stack<byte[]> _buffers;

        /// <summary>
        /// 当分配超出初始长度的新缓冲区时通知侦听器。
        /// </summary>
        public event EventHandler NewBufferAllocated;
        /// <summary>
        /// 引发 <see>NewBufferAllocated</see> 事件。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnNewBufferAllocated(EventArgs e)
        {
            var handler = NewBufferAllocated;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 通知侦听器已分配缓冲区。
        /// </summary>
        public event EventHandler BufferRequested;
        /// <summary>
        /// 引发 <see>BufferRequested</see> 事件。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnBufferRequested(EventArgs e)
        {
            var handler =BufferRequested;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 通知侦听器已返回缓冲区。
        /// </summary>
        public event EventHandler BufferReturned;
        /// <summary>
        /// 引发 <see>BufferReturned</see> 事件。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnBufferReturned(EventArgs e)
        {
            var handler = BufferReturned; 
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// 获取从此池分配的缓冲区大小。
        /// </summary>
        public int BufferLength
        {
            get { return _bufferLength; }
        }

        /// <summary>
        /// 获取从此池在任何给定时间可用的最大缓冲区数。
        /// </summary>
        public int MaxBufferCount
        {
            get { return _bufferCount; }
        }

        /// <summary>
        /// 获取当前可用于使用的缓冲区数。
        /// </summary>
        public int BuffersAvailable => _buffers.Count;

        /// <summary>
        /// 获取或设置返回缓冲区时是否将其内容清零。  
        /// </summary>
        public bool ClearOnReturn { get; set; }

        /// <summary>
        /// 创建具有指定名称、缓冲区大小和缓冲区计数的新缓冲池。
        /// </summary>
        /// <param name="baseBufferLength">预分配缓冲区的大小。</param>
        /// <param name="baseBufferCount">应该可用的预分配缓冲区数量。</param>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="baseBufferLength"/> 或
        /// <paramref name="baseBufferCount"/> 为零或负数时抛出。</exception>
        public BufferPool(int baseBufferLength, int baseBufferCount)
        {
            if (baseBufferLength <= 0)
                throw new ArgumentOutOfRangeException("baseBufferLength", baseBufferLength, "缓冲区长度必须为正整数值。");
            if (baseBufferCount <= 0)
                throw new ArgumentOutOfRangeException("baseBufferCount", baseBufferCount, "缓冲区计数必须为正整数值。");

            _bufferLength = baseBufferLength;
            _bufferCount = baseBufferCount;

            _buffers = new Stack<byte[]>(baseBufferCount);

            for (int i = 0; i < baseBufferCount; i++)
            {
                _buffers.Push(new byte[baseBufferLength]);
            }
        }

        /// <summary>
        /// 如果有可用的缓冲区则从可用池中获取一个，否则分配一个新的。
        /// </summary>
        /// <remarks>
        /// <para>使用此方法检索的缓冲区应通过使用
        /// <see>ReturnBuffer</see> 方法返回到池中。</para>
        /// </remarks>
        /// <returns>来自池的 <see>byte</see>[]。</returns>
        public byte[] GetBuffer()
        {
            lock (_buffers)
            {
                if (_buffers.Count > 0)
                {
                    byte[] buffer = _buffers.Pop();
                    return buffer;
                }
            }

            return AllocateNewBuffer();
        }

        private byte[] AllocateNewBuffer()
        {
            byte[] newBuffer = new byte[_bufferLength];
            _bufferCount++;
            OnNewBufferAllocated(EventArgs.Empty);

            return newBuffer;
        }

        /// <summary>
        /// 将指定的缓冲区返回到池中。
        /// </summary>
        /// <returns>如果缓冲区属于此池并已释放则为 <see langword="true" />；否则为 <see langword="false" />。</returns>
        /// <remarks>
        /// <para>如果 <see>ClearOnFree</see> 属性为 <see langword="true" />，则在将缓冲区恢复到池中之前会将其清零。</para>
        /// </remarks>
        /// <param name="buffer">要返回到池中的缓冲区。</param>
        /// <exception cref="ArgumentNullException">如果 <paramref name="buffer" /> 为 <see langword="null" /> 时抛出。</exception>
        public bool ReturnBuffer(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "缓冲区不能为空。");
            if (buffer.Length != _bufferLength)
                return false;

            if (ClearOnReturn)
                Array.Clear(buffer, 0, buffer.Length);

            lock (_buffers)
            {
                if (!_buffers.Contains(buffer))
                    _buffers.Push(buffer);
            }
            return true;
        }

        /// <summary>
        /// 通过给定大小增加池中可用的缓冲区数量。
        /// </summary>
        /// <param name="buffersToAdd">要预分配的缓冲区数量。</param>
        /// <exception cref="OutOfMemoryException">如果系统无法预分配请求的缓冲区数量时抛出。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="buffersToAdd"/> 小于或等于 0 时抛出。</exception>
        /// <remarks>
        /// <para>此方法不会导致引发 <see>NewBufferAllocated</see> 事件。</para>
        /// </remarks>
        public void IncreaseBufferCount(int buffersToAdd)
        {
            if (buffersToAdd <= 0)
                throw new ArgumentOutOfRangeException("buffersToAdd", buffersToAdd, "要添加的缓冲区数量必须是非负、非零整数。");

            List<byte[]> newBuffers = new List<byte[]>(buffersToAdd);
            for (int i = 0; i < buffersToAdd; i++)
            {
                newBuffers.Add(new byte[_bufferLength]);
            }

            lock (_buffers)
            {
                _bufferCount += buffersToAdd;
                for (int i = 0; i < buffersToAdd; i++)
                {
                    _buffers.Push(newBuffers[i]);
                }
            }
        }

        /// <summary>
        /// 从池中最多移除指定数量的缓冲区。
        /// </summary>
        /// <param name="buffersToRemove">要尝试移除的缓冲区数量。</param>
        /// <returns>实际移除的缓冲区数量。</returns>
        /// <remarks>
        /// <para>如果指定数量的缓冲区未空闲，则实际移除的缓冲区数量可能低于请求的数量。
        /// 例如，如果空闲缓冲区数量为 15，而调用者请求移除 20 个缓冲区，则只会释放 15 个，因此
        /// 返回值将为 15。</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">如果 <paramref name="buffersToRemove"/> 小于或等于 0 时抛出。</exception>
        public int DecreaseBufferCount(int buffersToRemove)
        {
            if (buffersToRemove <= 0)
                throw new ArgumentOutOfRangeException("buffersToRemove", buffersToRemove, "要移除的缓冲区数量必须是非负、非零整数。");

            int numRemoved = 0;

            lock (_buffers)
            {
                for (int i = 0; i < buffersToRemove && _buffers.Count > 0; i++)
                {
                    _buffers.Pop();
                    numRemoved++;
                    _bufferCount--;
                }
            }

            return numRemoved;
        }
    }
}