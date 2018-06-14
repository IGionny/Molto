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
            if (t.IsPrimitive || t == typeof(Decimal) || t == typeof(String))
            {
                return true;
            }

            return false;
        }

    }
}
