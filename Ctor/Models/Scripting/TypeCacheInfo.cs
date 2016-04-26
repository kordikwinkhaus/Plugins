using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using IronPython.Runtime;

namespace Ctor.Models.Scripting
{
    internal class TypeCacheInfo
    {
        internal const string NULL = "null";

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
            _getDebugValue = GetDebugValueFunc(type, attrs);
        }

        private static string GetTypeName(Type type, object[] attrs)
        {
            // for "python types" test PythonTypeAttribute
            foreach (var attr in attrs)
            {
                var pythonTypeAttr = attr as PythonTypeAttribute;
                if (pythonTypeAttr != null)
                {
                    return pythonTypeAttr.Name;
                }
            }

            // test debugger display attribute
            foreach (var attr in attrs)
            {
                var debugDisplayAttr = attr as DebuggerDisplayAttribute;
                if (debugDisplayAttr != null && !string.IsNullOrEmpty(debugDisplayAttr.Type))
                {
                    return debugDisplayAttr.Type;
                }
            }

            // for generic types generate C# syntax
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

            // arrays
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                var elementType = type.GetElementType();
                var typeInfo = TypeCache.GetTypeInfo(elementType);

                StringBuilder sb = new StringBuilder();
                sb.Append(typeInfo.Name);
                sb.Append("[");
                for (int i = 1; i < rank; i++)
                {
                    sb.Append(",");
                }
                sb.Append("]");
                return sb.ToString();
            }

            // test C# builtin types (use shorter names)
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

        private static List<PropertyInfo> GetPublicProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();

            if (type == typeof(string)) return result;

            foreach (var propInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                         .Where(p => p.GetIndexParameters().Length == 0)
                                         .OrderBy(p => p.Name))
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

        private Func<object, string> GetDebugValueFunc(Type type, object[] attrs)
        {
            // test debugger display attribute
            foreach (var attr in attrs)
            {
                var debugDisplayAttr = attr as DebuggerDisplayAttribute;
                if (debugDisplayAttr != null)
                {
                    string valueFmt = debugDisplayAttr.Value;
                    var tokens = GetTokensForDebuggerDisplay(type, valueFmt);
                    return value => FormatDebuggerDisplay(type, value, valueFmt, tokens);
                }
            }

            if (type == typeof(string))
            {
                return value => "\"" + value.ToString() + "\"";
            }

            if (TypeHelper.ImplementsInterface(type, typeof(IConvertible)))
            {
                return value => ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                // for generic types without ToString override use type name instead of "object.ToString()"
                if (type.IsGenericType)
                {
                    var toStringMethod = type.GetMethod("ToString", new Type[] { });
                    if (toStringMethod.GetBaseDefinition().DeclaringType == typeof(object))
                    {
                        return value => this.Name;
                    }
                }

                return value => value.ToString();
            }
        }

        private static List<Token> GetTokensForDebuggerDisplay(Type type, string valueFmt)
        {
            List<Token> tokens = new List<Token>();

            var re = new Regex("(\\{[^}]+\\})");
            MatchCollection matches = re.Matches(valueFmt);

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (match.Success)
                {
                    var group = match.Groups[1];
                    var token = new Token 
                    {
                        Value = group.Value
                    };

                    string rawExp = token.Value.Substring(1, token.Value.Length - 2);
                    token.NonQuote = rawExp.EndsWith(",nq");
                    if (token.NonQuote)
                    {
                        rawExp = rawExp.Substring(0, rawExp.Length - 3);
                    }
                    bool isFunction = (rawExp.EndsWith(")"));
                    string memberName = rawExp;
                    if (isFunction)
                    {
                        int id = rawExp.IndexOf('(');
                        if (id != -1)
                        {
                            memberName = memberName.Substring(0, id);
                        }
                    }
                    MethodInfo method = null;
                    if (isFunction)
                    {
                        method = type.GetMethod(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    }
                    else
                    {
                        var propInfo = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (propInfo != null)
                        {
                            method = propInfo.GetGetMethod(true);
                        }
                    }

                    if (method != null)
                    {
                        token.Method = method;
                        tokens.Add(token);
                    }
                }
            }

            return tokens;
        }

        private static string FormatDebuggerDisplay(Type type, object value, string valueFmt, List<Token> tokens)
        {
            string result = valueFmt;

            foreach (var token in tokens)
            {
                object returnValue = token.Method.Invoke(value, null);
                string strReturnValue = NULL;
                if (returnValue != null)
                {
                    if (returnValue is string && token.NonQuote)
                    {
                        strReturnValue = returnValue.ToString();
                    }
                    else
                    {
                        var typeInfo = TypeCache.GetTypeInfo(returnValue.GetType());
                        strReturnValue = typeInfo.GetDebugValue(returnValue);
                    }
                }
                result = result.Replace(token.Value, strReturnValue);
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

        #region nested classes

        private class Token
        {
            internal string Value;
            internal bool NonQuote;
            internal MethodInfo Method;
        }

        #endregion
    }
}
