using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WJOP.Foundation.Common;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;

namespace WJOP.Foundation.APIStore.SDK
{
    public class APIStoreHelper
    {
        public APIStoreHelper()
        {
        }

        private static T Convert<T>(string objStr)
        {
            T getValue;
            T t;
            T t1;
            if (!string.IsNullOrWhiteSpace(objStr))
            {
                if (typeof(T) == typeof(bool))
                {
                    t1 = default(T);
                    getValue = objStr.ToGetValue<T>(t1);
                }
                else if (!(typeof(T) == typeof(string)))
                {
                    getValue = (new JavaScriptSerializer()).Deserialize<T>(objStr);
                }
                else
                {
                    t1 = default(T);
                    getValue = objStr.ToGetValue<T>(t1);
                }
                t = getValue;
            }
            else
            {
                t1 = default(T);
                t = t1;
            }
            return t;
        }

        public static ServiceDiscoveryOperationResult DeregisterServiceNode()
        {
            return ConsulHost.Instance.TriggerAutoDeregistry();
        }

        public static string DiscoverService(string serviceName, string tags = "", bool supportFailoverSite = false, int cacheExpirePeriod = 1000)
        {
            return ConsulHost.Instance.DiscoverService(serviceName, tags, supportFailoverSite, cacheExpirePeriod);
        }

        public static T Get<T>(string serviceName, string relativeURL = "", APIGatewayMode mode = 0)
        {
            T t;
            try
            {
                string str = APIStoreHelper.Invoke(serviceName, relativeURL, string.Empty, "GET", mode, "application/json; charset=utf-8", "utf-8", null, null);
                t = APIStoreHelper.Convert<T>(str);
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
                t = default(T);
            }
            return t;
        }

        private static byte[] GetBytes(string encodingType, string data)
        {
            return Encoding.GetEncoding(encodingType).GetBytes(data);
        }

        public static ServiceDiscoveryOperationResult InitializeServiceDiscoveryConfiguration(string relativeConfigFilePath = ".\\MicroServiceConfig\\ServiceDiscovery.xml")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            char[] chrArray = new char[] { '\\' };
            string str = Path.Combine(baseDirectory.TrimEnd(chrArray), relativeConfigFilePath);
            return ConsulConfigurator.Instance.InitializeConfiguration(str);
        }

        public static string Invoke(string serviceName, string relativeURL, string httpBody, string httpMethod = "POST", APIGatewayMode mode = 0, string contentType = "application/json; charset=utf-8", string encodingType = "utf-8", NameValueCollection addHeaders = null, IWebProxy proxy = null)
        {
            string empty = string.Empty;
            string str = string.Empty;
            if (mode == APIGatewayMode.Redirect)
            {
                str = string.Concat(APIRepository.Instance.Lookup(serviceName), relativeURL);
            }
            else if (mode == APIGatewayMode.Relay)
            {
                str = string.Concat(LocationHelper.Instance.GetAppUri(AppNameEnum.ApiStore), serviceName, relativeURL);
            }
            if (!string.IsNullOrEmpty(str))
            {
                empty = APIStoreHelper.InvokeAPI(str, httpBody, httpMethod, contentType, encodingType, addHeaders, proxy);
            }
            return empty;
        }

        private static string InvokeAPI(string url, string requestBody, string httpMethod, string contentType = "application/json; charset=utf-8", string encodingType = "utf-8", NameValueCollection addHeaders = null, IWebProxy proxy = null)
        {
            HttpWebRequest length = (HttpWebRequest)WebRequest.Create(url);
            length.Method = httpMethod;
            if (!string.IsNullOrWhiteSpace(AppContext.AppKey))
            {
                if (addHeaders == null)
                {
                    addHeaders = new NameValueCollection();
                }
                addHeaders["AppKey"] = AppContext.AppKey;
                WJOPRequestContext.AttachToHttpHeaders(addHeaders);
            }
            if ((addHeaders == null ? false : addHeaders.Count > 0))
            {
                length.Headers.Add(addHeaders);
            }
            if (proxy != null)
            {
                length.Proxy = proxy;
            }
            length.ContentType = contentType;
            length.ContentLength = (long)requestBody.Length;
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                Stream requestStream = length.GetRequestStream();
                try
                {
                    byte[] bytes = APIStoreHelper.GetBytes(encodingType, requestBody);
                    requestStream.Write(bytes, 0, (int)bytes.Length);
                }
                finally
                {
                    if (requestStream != null)
                    {
                        ((IDisposable)requestStream).Dispose();
                    }
                }
            }
            WebResponse response = length.GetResponse();
            string empty = string.Empty;
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            try
            {
                empty = streamReader.ReadToEnd();
                if (empty.StartsWith("ApiStore调用服务时发生错误"))
                {
                    throw new WebException(empty);
                }
            }
            finally
            {
                if (streamReader != null)
                {
                    ((IDisposable)streamReader).Dispose();
                }
            }
            return empty;
        }

        private static string InvokeAPIStore(string serviceName, string relativeURL, string requestBody, string httpMethod, string contentType = "application/json; charset=utf-8", string encodingType = "utf-8", NameValueCollection addHeaders = null, IWebProxy proxy = null)
        {
            string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.ApiStore);
            string str = string.Concat(appUri, serviceName, relativeURL);
            HttpWebRequest length = (HttpWebRequest)WebRequest.Create(str);
            length.Method = httpMethod;
            if (!string.IsNullOrWhiteSpace(AppContext.AppKey))
            {
                if (addHeaders == null)
                {
                    addHeaders = new NameValueCollection();
                }
                addHeaders["AppKey"] = AppContext.AppKey;
                WJOPRequestContext.AttachToHttpHeaders(addHeaders);
            }
            if ((addHeaders == null ? false : addHeaders.Count > 0))
            {
                length.Headers.Add(addHeaders);
            }
            if (proxy != null)
            {
                length.Proxy = proxy;
            }
            length.ContentType = contentType;
            length.ContentLength = (long)requestBody.Length;
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                Stream requestStream = length.GetRequestStream();
                try
                {
                    byte[] bytes = APIStoreHelper.GetBytes(encodingType, requestBody);
                    requestStream.Write(bytes, 0, (int)bytes.Length);
                }
                finally
                {
                    if (requestStream != null)
                    {
                        ((IDisposable)requestStream).Dispose();
                    }
                }
            }
            WebResponse response = length.GetResponse();
            string empty = string.Empty;
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            try
            {
                empty = streamReader.ReadToEnd();
                if (empty.StartsWith("ApiStore调用服务时发生错误"))
                {
                    throw new WebException(empty);
                }
            }
            finally
            {
                if (streamReader != null)
                {
                    ((IDisposable)streamReader).Dispose();
                }
            }
            return empty;
        }

        public static T Post<T>(string serviceName, string requestBody, string relativeURL = "", APIGatewayMode mode = 0)
        {
            T t;
            try
            {
                string str = APIStoreHelper.Invoke(serviceName, relativeURL, requestBody, "POST", mode, "application/json; charset=utf-8", "utf-8", null, null);
                t = APIStoreHelper.Convert<T>(str);
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
                t = default(T);
            }
            return t;
        }

        public static ServiceDiscoveryOperationResult RegisterServiceNode(string relativeConfigFilePath = ".\\MicroServiceConfig\\ServiceDiscovery.xml")
        {
            ServiceDiscoveryOperationResult serviceDiscoveryOperationResult = APIStoreHelper.InitializeServiceDiscoveryConfiguration(relativeConfigFilePath);
            if (serviceDiscoveryOperationResult.IsSuccess)
            {
                serviceDiscoveryOperationResult = ConsulHost.Instance.TriggerAutoRegistry();
            }
            return serviceDiscoveryOperationResult;
        }

        public static string ResolveServiceURL(string serviceName)
        {
            return APIRepository.Instance.Lookup(serviceName);
        }
    }
}
