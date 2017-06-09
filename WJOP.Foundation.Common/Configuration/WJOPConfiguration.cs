using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Configuration
{
    public class WJOPConfiguration : ConfigurationSection
    {
        private static string ConfigurationSectionConst = "wjop";
        [ConfigurationProperty("appkey", IsRequired = true)]
        public string AppKey
        {
            get
            {
                return base["appkey"] as string;
            }
        }
        [ConfigurationProperty("cache")]
        public WJOPSubElementCollection CacheCollection
        {
            get
            {
                return (WJOPSubElementCollection)base["cache"] ?? new WJOPSubElementCollection();
            }
        }
        [ConfigurationProperty("dal")]
        public WJOPSubElementCollection DalCollection
        {
            get
            {
                return (WJOPSubElementCollection)base["dal"] ?? new WJOPSubElementCollection();
            }
        }
        [ConfigurationProperty("isdebug", IsRequired = false)]
        public string IsDebug
        {
            get
            {
                return base["isdebug"] as string;
            }
        }
        [ConfigurationProperty("location", IsRequired = false)]
        public string Location
        {
            get
            {
                return base["location"] as string;
            }
        }
        [ConfigurationProperty("log")]
        public WJOPSubElementCollection LogCollection
        {
            get
            {
                return (WJOPSubElementCollection)base["log"] ?? new WJOPSubElementCollection();
            }
        }
        [ConfigurationProperty("measurement")]
        public WJOPSubElementCollection MeasurementCollection
        {
            get
            {
                return (WJOPSubElementCollection)base["measurement"] ?? new WJOPSubElementCollection();
            }
        }
        [ConfigurationProperty("mq")]
        public WJOPMessageQueueElementCollection MessageQueueConnections
        {
            get
            {
                return (WJOPMessageQueueElementCollection)base["mq"] ?? new WJOPMessageQueueElementCollection();
            }
        }
        public static WJOPConfiguration GetConfig()
        {
            return (WJOPConfiguration)ConfigurationManager.GetSection(WJOPConfiguration.ConfigurationSectionConst) ?? new WJOPConfiguration();
        }

    }
}
