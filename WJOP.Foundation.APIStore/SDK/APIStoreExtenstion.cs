using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WJOP.Foundation.APIStore.SDK
{
    public static class APIStoreExtenstion
    {
        public static T PostJsonToAPIStore<T>(this string json, string serviceName, string relativeURL = "")
        {
            return APIStoreHelper.Post<T>(serviceName, json, "", APIGatewayMode.Redirect);
        }

        public static T PostToAPIStore<T>(this object request, string serviceName, string relativeURL = "")
        {
            T t = APIStoreHelper.Post<T>(serviceName, (new JavaScriptSerializer()).Serialize(request), "", APIGatewayMode.Redirect);
            return t;
        }
    }
}
