using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WJOP.Foundation.Common.Utility
{
    public static class JsonSerializerUtil
    {
        public static T Deserialize<T>(string json)
        {
            return (new JavaScriptSerializer()
            {
                MaxJsonLength = 2147483647
            }).Deserialize<T>(json);
        }

        public static T JsonDeserialize<T>(this string jsonString)
        {
            return JsonSerializerUtil.Deserialize<T>(jsonString);
        }

        public static string Serialize<T>(T obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer()
            {
                MaxJsonLength = 2147483647
            };
            return javaScriptSerializer.Serialize(obj);
        }

        public static string ToJsonString(this object obj)
        {
            return JsonSerializerUtil.Serialize<object>(obj);
        }
    }
}
