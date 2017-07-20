using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Log.SDK;

namespace WJOP.Foundation.Log.Common
{
    [ProtoContract]
    internal class LogContent
    {
        [ProtoMember(4)]
        public string Content { get; set; }

        [ProtoMember(2)]
        public string Level
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public string LogTimeStamp
        {
            get;
            set;
        }

        [ProtoMember(1)]
        public string Provider
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public IDictionary<string, Tag> Tags
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string Title
        {
            get;
            set;
        }
        private LogContent()
        {

        }

        internal IDictionary<string, object> TransformedTags
        {
            get
            {
                IDictionary<string, object> strs = new Dictionary<string, object>();
                if ((this.Tags == null ? false : this.Tags.Count > 0))
                {
                    foreach (var tag in this.Tags)
                    {
                        strs.Add(tag.Key, tag.Value.Dump());
                    }
                }
                return strs;
            }
        }
        public static LogContent CreateLog(LogLevelEnum level, string logProvider, string title, string content, IDictionary<string, Tag> tags)
        {
            var logContent = new LogContent()
            {
                Content = content,
                Level = level.ToString(),
                LogTimeStamp = DateTime.UtcNow.ToString("o"),
                Provider = logProvider,
                Tags = tags,
                Title = title
            };
            return logContent;
        }
    }
}
