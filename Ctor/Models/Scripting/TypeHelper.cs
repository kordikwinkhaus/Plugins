using System;

namespace Ctor.Models.Scripting
{
    internal static class TypeHelper
    {
        internal static bool ImplementsInterface(Type type, Type interfaceType)
        {
            foreach (Type implementedInterface in type.GetInterfaces())
            {
                if (implementedInterface == interfaceType)
                {
                    return true;
                }
                if (implementedInterface.IsGenericType && 
                    implementedInterface.GetGenericTypeDefinition() == interfaceType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
