using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.SDK
{
    internal static class ObjectExtension
    {
        private static T ToConvert<T>(this object obj, T defaultValue)
        {
            T t;
            try
            {
                Type genericArguments = typeof(T);
                if ((!genericArguments.IsValueType || genericArguments.IsPrimitive ? false : genericArguments.IsGenericType))
                {
                    genericArguments = genericArguments.GetGenericArguments()[0];
                }
                t = (T)Convert.ChangeType(obj, genericArguments);
            }
            catch
            {
                t = defaultValue;
            }
            return t;
        }

        public static T ToGetValue<T>(this object obj)
        {
            return obj.ToGetValue<T>(default(T));
        }

        public static T ToGetValue<T>(this object obj, T defaultValue)
        {
            T convert;
            if (!(typeof(T) == typeof(string)))
            {
                convert = obj.ToConvert<T>(defaultValue);
            }
            else
            {
                T t = obj.ToConvert<T>(defaultValue);
                convert = (t != null ? t : defaultValue);
            }
            return convert;
        }
    }
}
