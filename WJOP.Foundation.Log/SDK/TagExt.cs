using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.SDK
{
    public static class TagExt
    {
        public static IDictionary<string, Tag> Tag(this IDictionary<string, Tag> tags, string key, string stringValue)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, Tag>();
            }
            tags.Add(key, stringValue);
            return tags;
        }

        public static IDictionary<string, Tag> Tag(this IDictionary<string, Tag> tags, string key, int intValue)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, Tag>();
            }
            tags.Add(key, intValue);
            return tags;
        }

        public static IDictionary<string, Tag> Tag(this IDictionary<string, Tag> tags, string key, float floatValue)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, Tag>();
            }
            tags.Add(key, (float)floatValue);
            return tags;
        }
    }
}
