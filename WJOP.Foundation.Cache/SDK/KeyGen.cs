using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Cache.SDK
{
    public class KeyGen
    {
        public static string AttachPrefixToKey(string key)
        {
            return string.Concat(AppContext.AppKey, ":", key);
        }

        public static IList<string> AttachPrefixToKey(IList<string> keys)
        {
            List<string> strs = new List<string>();
            foreach (var key in keys)
            {
                strs.Add(AttachPrefixToKey(key));
            }
            return strs;
        }

        public static IDictionary<string, T> AttachPrefixToKey<T>(IDictionary<string, T> items)
        {
            Dictionary<string, T> strs = new Dictionary<string, T>();
            foreach (var item in items)
            {
                strs.Add(AttachPrefixToKey(item.Key), item.Value);
            }
            return strs;
        }

        public static string DetachPrefixFromKey(string key)
        {
            return key.Remove(0, AppContext.AppKey.Length + 1);
        }
        public static IList<string> DetachPrefixFromKey(IList<string> keys)
        {
            List<string> strs = new List<string>();
            foreach (string key in keys)
            {
                strs.Add(DetachPrefixFromKey(key));
            }
            return strs;
        }
    }
}
