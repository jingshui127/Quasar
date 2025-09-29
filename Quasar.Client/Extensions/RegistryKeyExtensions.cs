﻿﻿using Microsoft.Win32;
using Quasar.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quasar.Client.Extensions
{
    /// <summary>
    /// 为注册表键和值操作提供扩展。
    /// </summary>
    public static class RegistryKeyExtensions
    {
        /// <summary>
        /// 确定提供的名称的注册表键是否为null或具有null值。
        /// </summary>
        /// <param name="keyName">与注册表键关联的名称。</param>
        /// <param name="key">实际的注册表键。</param>
        /// <returns>如果提供的名称为null或空，或键为null则返回True；否则返回False。</returns>
        private static bool IsNameOrValueNull(this string keyName, RegistryKey key)
        {
            return (string.IsNullOrEmpty(keyName) || (key == null));
        }

        /// <summary>
        /// 尝试使用指定的键名获取键的字符串值。此方法假定输入正确。
        /// </summary>
        /// <param name="key">我们获取其值的键。</param>
        /// <param name="keyName">键的名称。</param>
        /// <param name="defaultValue">如果无法确定值，则为默认值。</param>
        /// <returns>返回使用指定键名获取的键值。如果无法获取，则返回defaultValue。</returns>
        public static string GetValueSafe(this RegistryKey key, string keyName, string defaultValue = "")
        {
            try
            {
                return key.GetValue(keyName, defaultValue).ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试从提供的键中使用指定名称获取只读（不可写）子键。抛出的异常将被捕获，只会返回null键。
        /// 此方法假定调用者在使用完键后会处理它。
        /// </summary>
        /// <param name="key">获取子键的键。</param>
        /// <param name="name">子键的名称。</param>
        /// <returns>返回从提供的键和名称获取的子键；如果无法获取子键则返回null。</returns>
        public static RegistryKey OpenReadonlySubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.OpenSubKey(name, false);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 尝试从提供的键中使用指定名称获取可写子键。此方法假定调用者在使用完键后会处理它。
        /// </summary>
        /// <param name="key">获取子键的键。</param>
        /// <param name="name">子键的名称。</param>
        /// <returns>返回从提供的键和名称获取的子键；如果无法获取子键则返回null。</returns>
        public static RegistryKey OpenWritableSubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.OpenSubKey(name, true);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 尝试从提供的键中使用指定名称创建子键。此方法假定调用者在使用完键后会处理它。
        /// </summary>
        /// <param name="key">创建子键的键。</param>
        /// <param name="name">子键的名称。</param>
        /// <returns>返回为提供的键和名称创建的子键；如果无法创建子键则返回null。</returns>
        public static RegistryKey CreateSubKeySafe(this RegistryKey key, string name)
        {
            try
            {
                return key.CreateSubKey(name);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 尝试从提供的键中使用指定名称删除子键及其子项。
        /// </summary>
        /// <param name="key">删除子键的键。</param>
        /// <param name="name">子键的名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool DeleteSubKeyTreeSafe(this RegistryKey key, string name)
        {
            try
            {
                key.DeleteSubKeyTree(name, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*
        * Derived and Adapted from drdandle's article, 
        * Copy and Rename Registry Keys at Code project.
        * Copy and Rename Registry Keys (Post Date: November 11, 2006)
        * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * This is a work that is not of the original. It
        * has been modified to suit the needs of another
        * application.
        * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * First Modified by StingRaptor on January 21, 2016
        * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        * Original Source:
        * http://www.codeproject.com/Articles/16343/Copy-and-Rename-Registry-Keys
        */

        /// <summary>
        /// 尝试使用指定的旧名称和新名称将子键重命名为提供的键。
        /// </summary>
        /// <param name="key">重命孙子键的键。</param>
        /// <param name="oldName">子键的旧名称。</param>
        /// <param name="newName">子键的新名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool RenameSubKeySafe(this RegistryKey key, string oldName, string newName)
        {
            try
            {
                //Copy from old to new
                key.CopyKey(oldName, newName);
                //Dispose of the old key
                key.DeleteSubKeyTree(oldName);
                return true;
            }
            catch
            {
                //Try to dispose of the newKey (The rename failed)
                key.DeleteSubKeyTreeSafe(newName);
                return false;
            }
        }

        /// <summary>
        /// 尝试将旧子键复制到新子键，使用指定的旧名称和新名称为提供的键复制。(抛出异常)
        /// </summary>
        /// <param name="key">删除子键的键。</param>
        /// <param name="oldName">子键的旧名称。</param>
        /// <param name="newName">子键的新名称。</param>
        public static void CopyKey(this RegistryKey key, string oldName, string newName)
        {
            //Create a new key
            using (RegistryKey newKey = key.CreateSubKey(newName))
            {
                //Open old key
                using (RegistryKey oldKey = key.OpenSubKey(oldName, true))
                {
                    //Copy from old to new
                    RecursiveCopyKey(oldKey, newKey);
                }
            }
        }

        /// <summary>
        /// 尝试使用指定的旧名称和新名称将子键重命名为提供的键。
        /// </summary>
        /// <param name="sourceKey">要复制的源键。</param>
        /// <param name="destKey">要复制到的目标键。</param>
        private static void RecursiveCopyKey(RegistryKey sourceKey, RegistryKey destKey)
        {

            //Copy all of the registry values
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object valueObj = sourceKey.GetValue(valueName);
                RegistryValueKind valueKind = sourceKey.GetValueKind(valueName);
                destKey.SetValue(valueName, valueObj, valueKind);
            }

            //Copy all of the subkeys
            foreach (string subKeyName in sourceKey.GetSubKeyNames())
            {
                using (RegistryKey sourceSubkey = sourceKey.OpenSubKey(subKeyName))
                {
                    using (RegistryKey destSubKey = destKey.CreateSubKey(subKeyName))
                    {
                        //Recursive call to copy the sub key data
                        RecursiveCopyKey(sourceSubkey, destSubKey);
                    }
                }
            }
        }

        /// <summary>
        /// 尝试使用指定的名称、数据和类型为提供的键设置注册表值。如果注册表值不存在，则会创建它
        /// </summary>
        /// <param name="key">要为其设置值的键。</param>
        /// <param name="name">值的名称。</param>
        /// <param name="data">值的数据</param>
        /// <param name="kind">值的类型</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool SetValueSafe(this RegistryKey key, string name, object data, RegistryValueKind kind)
        {
            try
            {
                // handle type conversion
                if (kind != RegistryValueKind.Binary && data.GetType() == typeof(byte[]))
                {
                    switch (kind)
                    {
                        case RegistryValueKind.String:
                        case RegistryValueKind.ExpandString:
                            data = ByteConverter.ToString((byte[]) data);
                            break;
                        case RegistryValueKind.DWord:
                            data = ByteConverter.ToUInt32((byte[]) data);
                            break;
                        case RegistryValueKind.QWord:
                            data = ByteConverter.ToUInt64((byte[]) data);
                            break;
                        case RegistryValueKind.MultiString:
                            data = ByteConverter.ToStringArray((byte[]) data);
                            break;
                    }
                }
                key.SetValue(name, data, kind);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试使用指定的名称删除提供的键的注册表值。
        /// </summary>
        /// <param name="key">要从中删除值的键。</param>
        /// <param name="name">值的名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool DeleteValueSafe(this RegistryKey key, string name)
        {
            try
            {
                key.DeleteValue(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试使用指定的旧名称和新名称将注册表值重命名为提供的键。
        /// </summary>
        /// <param name="key">要从中重命名注册表值的键。</param>
        /// <param name="oldName">注册表值的旧名称。</param>
        /// <param name="newName">注册表值的新名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool RenameValueSafe(this RegistryKey key, string oldName, string newName)
        {
            try
            {
                //Copy from old to new
                key.CopyValue(oldName, newName);
                //Dispose of the old value
                key.DeleteValue(oldName);
                return true;
            }
            catch
            {
                //Try to dispose of the newKey (The rename failed)
                key.DeleteValueSafe(newName);
                return false;
            }
        }

        /// <summary>
        /// 尝试使用指定的旧名称和新名称将旧注册表值复制到提供的键的新注册表值。(抛出异常)
        /// </summary>
        /// <param name="key">要复制注册表值的键。</param>
        /// <param name="oldName">注册表值的旧名称。</param>
        /// <param name="newName">注册表值的新名称。</param>
        public static void CopyValue(this RegistryKey key, string oldName, string newName)
        {
            RegistryValueKind valueKind = key.GetValueKind(oldName);
            object valueData = key.GetValue(oldName);

            key.SetValue(newName, valueData, valueKind);
        }

        /// <summary>
        /// 检查指定的子键是否存在于键中
        /// </summary>
        /// <param name="key">要搜索的键。</param>
        /// <param name="name">要查找的子键名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool ContainsSubKey(this RegistryKey key, string name)
        {
            foreach (string subkey in key.GetSubKeyNames())
            {
                if (subkey == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查指定的注册表值是否存在于键中
        /// </summary>
        /// <param name="key">要搜索的键。</param>
        /// <param name="name">要查找的注册表值名称。</param>
        /// <returns>如果操作成功则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool ContainsValue(this RegistryKey key, string name)
        {
            foreach (string value in key.GetValueNames())
            {
                if (value == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取与注册表键关联的所有值名称，并返回过滤值的格式化字符串。
        /// </summary>
        /// <param name="key">获取值的注册表键。</param>
        /// <returns>Yield返回键和键值的格式化字符串。</returns>
        public static IEnumerable<Tuple<string, string>> GetKeyValues(this RegistryKey key)
        {
            if (key == null) yield break;

            foreach (var k in key.GetValueNames().Where(keyVal => !keyVal.IsNameOrValueNull(key)).Where(k => !string.IsNullOrEmpty(k)))
            {
                yield return new Tuple<string, string>(k, key.GetValueSafe(k));
            }
        }

        /// <summary>
        /// 获取给定注册表值数据类型的默认值。
        /// </summary>
        /// <param name="valueKind">注册表值的数据类型。</param>
        /// <returns>给定<see cref="valueKind"/>的默认值。</returns>
        public static object GetDefault(this RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.Binary:
                    return new byte[] {};
                case RegistryValueKind.MultiString:
                    return new string[] {};
                case RegistryValueKind.DWord:
                    return 0;
                case RegistryValueKind.QWord:
                    return (long)0;
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return "";
                default:
                    return null;
            }
        }
    }
}
