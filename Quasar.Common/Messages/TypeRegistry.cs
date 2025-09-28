﻿using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quasar.Common.Messages
{
    public static class TypeRegistry
    {
        /// <summary>
        /// 消息类型的内部索引。
        /// </summary>
        private static int _typeIndex;

        /// <summary>
        /// 向序列化器添加一个类型，以便消息能够正确序列化。
        /// </summary>
        /// <param name="parent">父类型，例如：IMessage</param>
        /// <param name="type">要添加的类型</param>
        public static void AddTypeToSerializer(Type parent, Type type)
        {
            if (type == null || parent == null)
                throw new ArgumentNullException();

            bool isAlreadyAdded = RuntimeTypeModel.Default[parent].GetSubtypes().Any(subType => subType.DerivedType.Type == type);

            if (!isAlreadyAdded)
                RuntimeTypeModel.Default[parent].AddSubType(++_typeIndex, type);
        }

        /// <summary>
        /// 向序列化器添加多个类型。
        /// </summary>
        /// <param name="parent">父类型，例如：IMessage</param>
        /// <param name="types">要添加的类型。</param>
        public static void AddTypesToSerializer(Type parent, params Type[] types)
        {
            foreach (Type type in types)
                AddTypeToSerializer(parent, type);
        }

        public static IEnumerable<Type> GetPacketTypes(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
        }
    }
}
