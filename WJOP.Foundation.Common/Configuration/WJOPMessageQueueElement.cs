using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Configuration
{
    public class WJOPMessageQueueElement:ConfigurationElement
    {
        [ConfigurationProperty("connectionName", IsRequired = true)]
        public string ConnectionName
        {
            get
            {
                return base["connectionName"] as string;
            }
        }

        [ConfigurationProperty("hostUrl", IsRequired = false)]
        public string HostUrl
        {
            get
            {
                return base["hostUrl"] as string;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return base["password"] as string;
            }
        }

        [ConfigurationProperty("prefetchcount", IsRequired = false, DefaultValue = 50)]
        public int Prefetchcount
        {
            get
            {
                object item = base["prefetchcount"];
                return (item == null ? 50 : (int)item);
            }
        }
        [ConfigurationProperty("requestedHeartbeat", IsRequired = false, DefaultValue = 10)]
        public int RequestedHeartbeat
        {
            get
            {
                object item = base["requestedHeartbeat"];
                return (item == null ? 10 : (int)item);
            }
        }

        [ConfigurationProperty("space", IsRequired = false, DefaultValue = "/")]
        public string Space
        {
            get
            {
                return base["space"] as string;
            }
        }

        [ConfigurationProperty("timeout", IsRequired = false, DefaultValue = 10)]
        public int Timeout
        {
            get
            {
                object item = base["timeout"];
                return (item == null ? 10 : (int)item);
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                return base["userName"] as string;
            }
        }
    }
}
