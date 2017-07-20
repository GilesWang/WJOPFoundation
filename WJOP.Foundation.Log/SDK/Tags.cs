using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.SDK
{
    public class Tags
    {
        public static IDictionary<string, Tag> Tag(string key, string stringValue)
        {
            return new Dictionary<string, Tag>()
            {
                { key, stringValue }
            };
        }

        public static IDictionary<string, Tag> Tag(string key, int intValue)
        {
            return new Dictionary<string, Tag>()
            {
                { key, intValue }
            };
        }

        public static IDictionary<string, Tag> Tag(string key, float floatValue)
        {
            return new Dictionary<string, Tag>()
            {
                { key, (float)floatValue }
            };
        }
    }
}
