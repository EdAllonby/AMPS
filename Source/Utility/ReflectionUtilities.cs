using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utility
{
    public static class ReflectionUtilities
    {
        public static Type FindFirstDerivedTypeWithGenericArgument(this Type baseType, Type baseGenericArgument)
        {
            return baseType.FindAllDerivedTypes(baseGenericArgument).FirstOrDefault();
        }

        private static IEnumerable<Type> FindAllDerivedTypes(this Type type, Type baseGenericArgument)
        {
            return Assembly.GetAssembly(type)
                .GetTypes()
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GenericTypeArguments[0] == baseGenericArgument
                            && t.BaseType.GetGenericTypeDefinition() == type);
        }
    }
}