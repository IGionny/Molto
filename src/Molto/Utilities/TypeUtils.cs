using System;
using System.Collections.Generic;
using System.Text;

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
            var t = typeof(T);
            return IsPrimitive(t);
        }

        public static bool IsPrimitive(Type type)
        {
            if (type.IsPrimitive || type == typeof(Decimal) || type == typeof(String))
            {
                return true;
            }

            return false;
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
