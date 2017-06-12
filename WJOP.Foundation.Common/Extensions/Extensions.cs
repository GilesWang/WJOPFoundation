using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Extensions
{
    public static class Extensions
    {
        public static int ToInt(this string value, int defaultValue = 0)
        {
            int num, num1 = 0;
            num = int.TryParse(value, out num1) ? num1 : defaultValue;
            return num;
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

        public static ApiResult<TResult> PostWebApi<TResult, TData>(this string url, TData data, NameValueCollection headers = null, int timeout = 180000)
        {
            return ProxyBase.Call<TResult, TData>(url, data, ApiHttpMethod.POST, timeout, headers);
        }
    }
}
