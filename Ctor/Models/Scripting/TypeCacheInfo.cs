using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using IronPython.Runtime;

namespace Ctor.Models.Scripting
{
    internal class TypeCacheInfo
    {
        private static Dictionary<string, string> s_builtinTypes = new Dictionary<string, string>
        {
            { "System.Boolean", "bool" },
            { "System.Byte", "byte" },
            { "System.SByte", "sbyte" },
            { "System.Char", "char" },
            { "System.Decimal", "decimal" },
            { "System.Double", "double" },
            { "System.Single", "float" },
            { "System.Int32", "int" },
            { "System.UInt32", "uint" },
            { "System.Int64", "long" },
            { "System.UInt64", "ulong" },
            { "System.Object", "object" },
            { "System.Int16", "short" },
            { "System.UInt16", "ushort" },
            { "System.String", "string" }
        };

        private readonly Func<object, string> _getDebugValue;
        private readonly string _typename;
        private readonly List<PropertyInfo> _properties;

        internal TypeCacheInfo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var attrs = type.GetCustomAttributes(false);

            _typename = GetTypeName(type, attrs);
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

        private string GetTypeName(Type type, object[] attrs)
        {
            foreach (var attr in attrs)
            {
                var pythonTypeAttr = attr as PythonTypeAttribute;
                if (pythonTypeAttr != null)
                {
                    return pythonTypeAttr.Name;
                }
            }

            if (type.IsGenericType)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(type.Namespace).Append(".");
                int id = type.Name.IndexOf('`');
                if (id != -1)
                {
                    sb.Append(type.Name.Substring(0, id));
                }
                else
                {
                    sb.Append(type.Name);
                }
                sb.Append("<");

                var typeArgs = type.GetGenericArguments();
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }
                    var paramType = TypeCache.GetTypeInfo(typeArgs[i]);
                    sb.Append(paramType.Name);
                }
                sb.Append(">");

                return sb.ToString();
            }

            string typeName = type.ToString();
            string shortName;
            if (s_builtinTypes.TryGetValue(typeName, out shortName))
            {
                return shortName;
            }
            else
            {
                return typeName;
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
