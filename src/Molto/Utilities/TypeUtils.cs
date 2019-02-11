using System;

namespace Molto.Utilities
{
    /**
     * Resources:
     * https://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
     */

    public static class TypeUtils
    {
        public static bool IsPrimitive<T>()
        {
            return IsPrimitive(typeof(T));
        }

        public static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive || type == typeof(decimal) || type == typeof(string);
        }


        public static Type RevealType<T>()
        {
            return RevealType(typeof(T));
        }

        public static Type RevealType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(type);
            }

            return type;
        }
    }
}