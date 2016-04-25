using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ctor.Models.Scripting
{
    internal class TypeCacheInfo
    {
        private readonly Func<object, string> _getDebugValue;
        private readonly string _typename;
        private readonly List<PropertyInfo> _properties;

        internal TypeCacheInfo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            _typename = type.ToString();
            _properties = GetPublicProperties(type);

            if (type == typeof(string))
            {
                _getDebugValue = value => "\"" + value.ToString() + "\"";
                return;
            }

            if (TypeHelper.ImplementsInterface(type, typeof(IConvertible)))
            {
                _getDebugValue = value => ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                _getDebugValue = value => value.ToString();
            }
        }

        private List<PropertyInfo> GetPublicProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();

            if (type == typeof(string)) return result;
            foreach (var propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                         .Where(p => p.GetIndexParameters().Length == 0))
            {
                var attrs = propInfo.GetCustomAttributes(typeof(DebuggerBrowsableAttribute), true);
                if (attrs != null && attrs.Length != 0)
                {
                    DebuggerBrowsableAttribute attr = (DebuggerBrowsableAttribute)attrs[0];
                    if (attr.State != DebuggerBrowsableState.Collapsed)
                    {
                        continue;
                    }
                }

                result.Add(propInfo);
            }

            return result;
        }

        internal string Name
        {
            get { return _typename; }
        }

        internal string GetDebugValue(object value)
        {
            return _getDebugValue(value);
        }

        internal bool HasPublicProperties
        {
            get { return _properties.Count != 0; }
        }

        internal IList<PropertyInfo> GetPublicProperties()
        {
            return _properties;
        }
    }
}
