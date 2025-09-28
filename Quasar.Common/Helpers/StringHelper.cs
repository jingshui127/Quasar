﻿using Quasar.Common.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace Quasar.Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// 用于生成随机字符串的可用字母表。
        /// </summary>
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// 文件大小的缩写。
        /// </summary>
        private static readonly string[] Sizes = { "B", "KB", "MB", "GB", "TB", "PB" };

        /// <summary>
        /// 随机数生成器。
        /// </summary>
        private static readonly SafeRandom Random = new SafeRandom();

        /// <summary>
        /// 获取指定长度的随机字符串。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <returns>随机字符串。</returns>
        public static string GetRandomString(int length)
        {
            StringBuilder randomName = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                randomName.Append(Alphabet[Random.Next(Alphabet.Length)]);

            return randomName.ToString();
        }

        /// <summary>
        /// 获取给定大小的人类可读文件大小。
        /// </summary>
        /// <param name="size">以字节为单位的文件大小。</param>
        /// <returns>人类可读的文件大小。</returns>
        public static string GetHumanReadableFileSize(long size)
        {
            double len = size;
            int order = 0;
            while (len >= 1024 && order + 1 < Sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {Sizes[order]}";
        }

        /// <summary>
        /// 获取格式化的MAC地址。
        /// </summary>
        /// <param name="macAddress">未格式化的MAC地址。</param>
        /// <returns>格式化的MAC地址。</returns>
        public static string GetFormattedMacAddress(string macAddress)
        {
            return (macAddress.Length != 12)
                ? "00:00:00:00:00:00"
                : Regex.Replace(macAddress, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");
        }

        /// <summary>
        /// 安全地从字符串中移除最后N个字符。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <param name="amount">要移除的最后字符数量（=N）。</param>
        /// <returns>移除了N个字符的输入字符串。</returns>
        public static string RemoveLastChars(string input, int amount = 2)
        {
            if (input.Length > amount)
                input = input.Remove(input.Length - amount);
            return input;
        }
    }
}
