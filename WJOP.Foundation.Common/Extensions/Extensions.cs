using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

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
        public static bool PostProtobufObject(this string url, object data)
        {
            return url.PostProtobufObject(data, null);
        }
        public static bool PostProtobufObject(this string url, object data, NameValueCollection headers)
        {
            bool flag = true;
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 60000;
                if (headers != null)
                {
                    httpWebRequest.Headers.Add(headers);
                }
                flag = httpWebRequest.PostProtobufObject(data);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                flag = false;
                Extensions.WirteException(exception);
            }
            return flag;
        }

        public static bool PostProtobufObject(this WebRequest webRequest, object data)
        {
            bool flag = true;
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
                    try
                    {
                        BufferedStream bufferedStream = new BufferedStream(gZipStream, 65536);
                        try
                        {
                            Serializer.Serialize<object>(bufferedStream, data);
                        }
                        finally
                        {
                            if (bufferedStream != null)
                            {
                                ((IDisposable)bufferedStream).Dispose();
                            }
                        }
                    }
                    finally
                    {
                        if (gZipStream != null)
                        {
                            ((IDisposable)gZipStream).Dispose();
                        }
                    }
                    webRequest.ContentLength = memoryStream.Length;
                    Stream requestStream = webRequest.GetRequestStream();
                    try
                    {
                        memoryStream.Position = (long)0;
                        memoryStream.CopyTo(requestStream);
                        requestStream.Flush();
                    }
                    finally
                    {
                        if (requestStream != null)
                        {
                            ((IDisposable)requestStream).Dispose();
                        }
                    }
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        ((IDisposable)memoryStream).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                flag = false;
                Extensions.WirteException(exception);
            }
            return flag;
        }

        public static ApiResult<TResult> PostWebApi<TResult, TData>(this string url, TData data, NameValueCollection headers = null, int timeout = 180000)
        {
            return ProxyBase.Call<TResult, TData>(url, data, ApiHttpMethod.POST, timeout, headers);
        }

        private static void WirteException(Exception ex)
        {
            DebugUtil.LogException(ex);
        }
    }
}
