﻿using Quasar.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Quasar.Common.IO
{
    public class FileSplit : IEnumerable<FileChunk>, IDisposable
    {
        /// <summary>
        /// 每个文件块的最大大小。
        /// </summary>
        public readonly int MaxChunkSize = 65535;

        /// <summary>
        /// 打开文件的文件路径。
        /// </summary>
        public string FilePath => _fileStream.Name;

        /// <summary>
        /// 打开文件的文件大小。
        /// </summary>
        public long FileSize => _fileStream.Length;

        /// <summary>
        /// 打开文件的文件流。
        /// </summary>
        private readonly FileStream _fileStream;

        /// <summary>
        /// 使用给定的文件路径和访问模式初始化 <see cref="FileSplit"/> 类的新实例。
        /// </summary>
        /// <param name="filePath">要打开的文件路径。</param>
        /// <param name="fileAccess">打开文件的文件访问模式。允许使用 <see cref="FileAccess.Read"/> 和 <see cref="FileAccess.Write"/>。</param>
        public FileSplit(string filePath, FileAccess fileAccess)
        {
            switch (fileAccess)
            {
                case FileAccess.Read:
                    _fileStream = File.OpenRead(filePath);
                    break;
                case FileAccess.Write:
                    _fileStream = File.OpenWrite(filePath);
                    break;
                default:
                    throw new ArgumentException($"{nameof(fileAccess)} must be either Read or Write.");
            }
        }

        /// <summary>
        /// 将一个块写入文件。
        /// </summary>
        /// <param name="chunk"></param>
        public void WriteChunk(FileChunk chunk)
        {
            _fileStream.Seek(chunk.Offset, SeekOrigin.Begin);
            _fileStream.Write(chunk.Data, 0, chunk.Data.Length);
        }

        /// <summary>
        /// 读取文件的一个块。
        /// </summary>
        /// <param name="offset">文件的偏移量，必须是 <see cref="MaxChunkSize"/> 的倍数才能正确重建。</param>
        /// <returns>在给定偏移量处读取的文件块。</returns>
        /// <remarks>
        /// 如果从偏移量开始的剩余文件大小小于 <see cref="MaxChunkSize"/>，则返回的文件块可能小于 <see cref="MaxChunkSize"/>，
        /// 此时使用剩余文件大小。
        /// </remarks>
        public FileChunk ReadChunk(long offset)
        {
            _fileStream.Seek(offset, SeekOrigin.Begin);

            long chunkSize = _fileStream.Length - _fileStream.Position < MaxChunkSize
                ? _fileStream.Length - _fileStream.Position
                : MaxChunkSize;

            var chunkData = new byte[chunkSize];
            _fileStream.Read(chunkData, 0, chunkData.Length);

            return new FileChunk
            {
                Data = chunkData,
                Offset = _fileStream.Position - chunkData.Length
            };
        }

        /// <summary>
        /// 返回一个枚举器，用于遍历文件块。
        /// </summary>
        /// <returns>一个 <see cref="IEnumerator"/> 对象，可用于遍历文件块。</returns>
        public IEnumerator<FileChunk> GetEnumerator()
        {
            for (long currentChunk = 0; currentChunk <= _fileStream.Length / MaxChunkSize; currentChunk++)
            {
                yield return ReadChunk(currentChunk * MaxChunkSize);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileStream.Dispose();
            }
        }

        /// <summary>
        /// 释放与此类关联的所有托管和非托管资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
