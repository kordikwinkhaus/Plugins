using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctor.Models.Scripting
{
    internal static class TypeCache
    {
        private static readonly Dictionary<Type, TypeCacheInfo> s_types = new Dictionary<Type, TypeCacheInfo>();

        internal static TypeCacheInfo GetTypeInfo(Type type)
        {
            TypeCacheInfo result;
            if (!s_types.TryGetValue(type, out result))
            {
                result = BuildTypeInfo(type);
                s_types.Add(type, result);
            }
            return result;
        }

        private static TypeCacheInfo BuildTypeInfo(Type type)
        {
            return new TypeCacheInfo(type);
        }
    }
}
