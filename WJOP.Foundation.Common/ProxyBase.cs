using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WJOP.Foundation.Common.Extensions;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Common
{
    public static class ProxyBase
    {
        public static ApiResult<TResult> Call<TResult, TData>(string domain, string controller, string action, TData data, ApiHttpMethod method = 0, int timeout = 180000)
        {
            domain = domain.TrimEnd('/') + "/";
            ApiResult<TResult> apiResult = ProxyBase.Call<TResult, TData>(string.Format("{0}{1}/{2}", domain, controller, action), data, method, timeout, null);
            return apiResult;
        }
        public static ApiResult<TResult> Call<TResult, TData>(string url, TData data, ApiHttpMethod method = 0, int timeout = 180000, NameValueCollection headers = null)
        {
            string empty = string.Empty;
            ApiResult<TResult> apiResult = new ApiResult<TResult>();
            TResult tResult;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            try
            {
                using (var client = new HttpClient())
                {
                    Task<HttpResponseMessage> response = null;
                    if (method == ApiHttpMethod.POST)
                    {
                        HttpContent content = new StringContent(javaScriptSerializer.Serialize(data), Encoding.UTF8, "application/json");
                        response = client.PostAsync(url, content);

                    }
                    else { response = client.GetAsync(url); }
                    response.Result.EnsureSuccessStatusCode();
                    apiResult.StatusCode = response.Result.StatusCode;
                    empty = response.Result.Content.ReadAsStringAsync().Result;
                    if (typeof(TResult) == typeof(bool))
                    {
                        tResult = default(TResult);
                        apiResult.Result = empty.ToGetValue<TResult>(tResult);
                    }
                    else if (!(typeof(TResult) == typeof(string)))
                    {
                        apiResult.Result = javaScriptSerializer.Deserialize<TResult>(empty);
                    }
                    else
                    {
                        tResult = default(TResult);
                        apiResult.Result = empty.ToGetValue<TResult>(tResult);
                    }
                }
            }
            catch (Exception ex)
            {
                apiResult.ExceptionMessage = ex.Message;
                DebugUtil.LogException(ex);
                DebugUtil.CollectDebugInfo(ex, "TGOP.Foundation.Common.ProxyBase.Call");
            }
            return apiResult;

        }
        public static ApiResult<TResult> Get<TResult>(string url)
        {
            return ProxyBase.Call<TResult, object>(url, null, ApiHttpMethod.GET, 180000, null);
        }
        public static ApiResult<TResult> Get<TResult>(string urlFormat, params string[] args)
        {
            return Get<TResult>(string.Format(urlFormat, args));
        }
    }
}
