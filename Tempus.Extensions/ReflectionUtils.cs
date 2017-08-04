namespace Tempus.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ReflectionUtils
    {
        public static bool IsNullable(Type t)
        {
            ChecarSe.ArgumentoNaoNulo(t, nameof(t));

            if (t.IsValueType)
            {
                return IsNullableType(t);
            }

            return true;
        }

        public static bool IsNullableType(Type t)
        {
            ChecarSe.ArgumentoNaoNulo(t, nameof(t));

            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetObjectType(object v)
        {
            return v?.GetType();
        }
    }
}
