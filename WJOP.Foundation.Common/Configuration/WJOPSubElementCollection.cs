using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Configuration
{
    public class WJOPSubElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("bucketname", IsRequired = false)]
        public string BucketName
        {
            get
            {
                return base["bucketname"] as string;
            }
        }
        [ConfigurationProperty("bucketpassword", IsRequired = false)]
        public string BucketPassword
        {
            get
            {
                return base["bucketpassword"] as string;
            }
        }
        [ConfigurationProperty("maxmessagecount", IsRequired = false)]
        public string MaxMessageCount
        {
            get
            {
                return base["maxmessagecount"] as string;
            }
        }
        [ConfigurationProperty("maxqueuesize", IsRequired = false)]
        public string MaxQueueSize
        {
            get
            {
                return base["maxqueuesize"] as string;
            }
        }
        [ConfigurationProperty("mode", IsRequired = false)]
        public string Mode
        {
            get
            {
                return base["mode"] as string;
            }
        }
        [ConfigurationProperty("sendfrequency", IsRequired = false)]
        public string SendFrequency
        {
            get
            {
                return base["sendfrequency"] as string;
            }
        }

        public WJOPSubElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as WJOPSubElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new WJOPSubElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WJOPSubElement)element).Key;
        }

        public string GetValue(string key)
        {
            string str;
            int num = 0;
            while (true)
            {
                if (num >= base.Count)
                {
                    str = null;
                    break;
                }
                else if (!(this[num].Key.ToLower() == key.ToLower()))
                {
                    num++;
                }
                else
                {
                    string value = this[num].Value;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        str = value;
                        break;
                    }
                    else
                    {
                        str = null;
                        break;
                    }
                }
            }
            return str;
        }
    }
}
