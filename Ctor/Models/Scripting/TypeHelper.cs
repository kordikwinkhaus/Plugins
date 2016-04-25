using System;
using System.Linq;

namespace Ctor.Models.Scripting
{
    internal static class TypeHelper
    {
        internal static bool ImplementsInterface(Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }
    }
}
