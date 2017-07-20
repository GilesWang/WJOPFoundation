using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Configuration;
using WJOP.Foundation.Common.Extensions;

namespace WJOP.Foundation.Common.Utility
{
    public class AppContext
    {
        private static IDictionary<string, string> _infoDic;
        private static IDictionary<string, bool> _onOffDic;
        private static List<MessageQueueConnection> _messageQueueConnections;
        private static string _macAddress;
        private static string _localIPv4;
        private static string _computerName;
        private const string WJOPEnv_RegKeyPath = "SOFTWARE\\TGOP";
        private const string WJOPEnv_RegKeyName = "Env";

        static AppContext()
        {
            _onOffDic = new Dictionary<string, bool>();
            _infoDic = new Dictionary<string, string>();
            _messageQueueConnections = new List<MessageQueueConnection>();
            InitConfiguration();
        }

        public AppContext()
        {

        }

        private static void InitConfiguration()
        {
            bool writeRemoteDbFlag;
            bool WriteLocalFileFlag;
            bool dalLogEnabledFlag;
            bool dalMeasurementEnabledFlag;
            bool debugFlag;
            string str;
            WJOPConfiguration config = WJOPConfiguration.GetConfig();
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Log\\");
            string value = config.LogCollection.GetValue("LocalFilePath") ?? str1;
            bool logMode = config.LogCollection.Mode.ToLower() != "off";
            bool measurementMode = config.LogCollection.Mode.ToLower() != "off";
            if (!bool.TryParse(config.LogCollection.GetValue("WriteRemoteDB"), out writeRemoteDbFlag))
            {
                writeRemoteDbFlag = true;
            }
            if (!bool.TryParse(config.LogCollection.GetValue("WriteLocalFile"), out WriteLocalFileFlag))
            {
                WriteLocalFileFlag = false;
            }
            if (!bool.TryParse(config.DalCollection.GetValue("DalLogEnabled"), out dalLogEnabledFlag))
            {
                dalLogEnabledFlag = true;
            }
            if (!bool.TryParse(config.DalCollection.GetValue("DalMeasurementEnabled"), out dalMeasurementEnabledFlag))
            {
                dalMeasurementEnabledFlag = true;
            }
            if (!bool.TryParse(config.IsDebug, out debugFlag))
            {
                debugFlag = false;
            }

            AppContext._onOffDic.Add("LogEnabled", logMode);
            AppContext._onOffDic.Add("MeasurementEnabled", measurementMode);
            AppContext._onOffDic.Add("WriteRemoteDB", writeRemoteDbFlag);
            AppContext._onOffDic.Add("WriteLocalFile", WriteLocalFileFlag);
            AppContext._onOffDic.Add("DalLogEnabled", dalLogEnabledFlag);
            AppContext._onOffDic.Add("DalMeasurementEnabled", dalMeasurementEnabledFlag);
            AppContext._onOffDic.Add("IsDebug", debugFlag);
            AppContext._infoDic.Add("AppKey", config.AppKey);
            AppContext._infoDic.Add("CacheType", (string.IsNullOrWhiteSpace(config.CacheCollection.CacheType) ? "couchbase" : config.CacheCollection.CacheType));
            AppContext._infoDic.Add("CacheConfigString", config.CacheCollection.GetValue("CacheConfigString"));
            if (string.IsNullOrWhiteSpace(config.Location))
            {
                str = "http://wjop-location.vipabc.com/";
            }
            else
            {
                str = string.Format("{0}/", config.Location.TrimEnd('/'));
            }
            AppContext._infoDic.Add("Location", str);
            AppContext._infoDic.Add("LocalFilePath", value);
            AppContext._infoDic.Add("MeasurementDB", config.MeasurementCollection.GetValue("MeasurementDB") ?? "");
            AppContext._infoDic.Add("LogSendFrequency", (string.IsNullOrWhiteSpace(config.LogCollection.SendFrequency) ? "1000" : config.LogCollection.SendFrequency));
            AppContext._infoDic.Add("LogMaxMessageCount", (string.IsNullOrWhiteSpace(config.LogCollection.MaxMessageCount) ? "1000" : config.LogCollection.MaxMessageCount));
            AppContext._infoDic.Add("LogMaxQueueSize", (string.IsNullOrWhiteSpace(config.LogCollection.MaxQueueSize) ? "10000" : config.LogCollection.MaxQueueSize));
            AppContext._infoDic.Add("MeasurementSendFrequency", (string.IsNullOrWhiteSpace(config.MeasurementCollection.SendFrequency) ? "1000" : config.MeasurementCollection.SendFrequency));
            AppContext._infoDic.Add("MeasurementMaxMessageCount", (string.IsNullOrWhiteSpace(config.MeasurementCollection.MaxMessageCount) ? "1000" : config.MeasurementCollection.MaxMessageCount));
            AppContext._infoDic.Add("BucketName", (string.IsNullOrWhiteSpace(config.CacheCollection.BucketName) ? "default" : config.CacheCollection.BucketName));
            AppContext._infoDic.Add("BucketPassword", (string.IsNullOrWhiteSpace(config.CacheCollection.BucketPassword) ? string.Empty : config.CacheCollection.BucketPassword));
            AppContext.LoadMessageQueueConnections(config);
            DebugUtil.CollectDebugInfo(AppContext._onOffDic, "TGOP.Foundation.Common.Utility.AppContext.InitConfiguration line:97");
            DebugUtil.CollectDebugInfo(AppContext._infoDic, "TGOP.Foundation.Common.Utility.AppContext.InitConfiguration line:98");

        }
        private static void LoadMessageQueueConnections(WJOPConfiguration config)
        {
            AppContext._messageQueueConnections = new List<MessageQueueConnection>();
            WJOPMessageQueueElement item = null;
            for (int i = 0; i < config.MessageQueueConnections.Count; i++)
            {
                item = config.MessageQueueConnections[i];
                MessageQueueConnection messageQueueConnection = new MessageQueueConnection()
                {
                    ConnectionName = item.ConnectionName,
                    HostUrl = item.HostUrl,
                    Space = item.Space,
                    Password = item.Password,
                    UserName = item.UserName,
                    Prefetchcount = item.Prefetchcount,
                    RequestedHeartbeat = item.RequestedHeartbeat,
                    Timeout = item.Timeout
                };
                AppContext._messageQueueConnections.Add(messageQueueConnection);
            }
        }

        #region Props
        public static string AppKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_infoDic["AppKey"]))
                {
                    throw new Exception("AppKey not found in config file.");
                }
                return _infoDic["AppKey"];
            }
        }
        public static string Environment
        {
            get
            {
                string value = "Dev";
                RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(WJOPEnv_RegKeyPath);
                try
                {
                    if (registryKey != null)
                    {
                        value = registryKey.GetValue(WJOPEnv_RegKeyName) as string;
                    }
                }
                finally
                {
                    if (registryKey != null)
                    {
                        ((IDisposable)registryKey).Dispose();
                    }
                }
                return (value.Length > 0 ? value : "Dev");
            }
            set
            {
                RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(WJOPEnv_RegKeyPath, true);
                try
                {
                    if (registryKey != null)
                    {
                        registryKey.SetValue(WJOPEnv_RegKeyName, value, RegistryValueKind.String);
                    }
                    else
                    {
                        RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).CreateSubKey(WJOPEnv_RegKeyPath);
                        try
                        {
                            registryKey1.SetValue(WJOPEnv_RegKeyName, value, RegistryValueKind.String);
                        }
                        finally
                        {
                            if (registryKey1 != null)
                            {
                                ((IDisposable)registryKey1).Dispose();
                            }
                        }
                    }
                }
                finally
                {
                    if (registryKey != null)
                    {
                        ((IDisposable)registryKey).Dispose();
                    }
                }
            }
        }
        public static bool WriteRemoteDB
        {
            get
            {
                return _onOffDic["WriteRemoteDB"];
            }
        }
        public static bool WriteLocalFile
        {
            get
            {
                return _onOffDic["WriteLocalFile"];
            }
        }
        public static bool WorkerQueuePerfCounterEnabled
        {
            get
            {
                return false;
            }
        }
        public static List<MessageQueueConnection> MessageQueueConnections
        {
            get
            {
                return _messageQueueConnections;
            }
        }
        public static bool MeasurementTraceEnabled
        {
            get
            {
                return false;
            }
        }
        public static int MeasurementSendFrequency
        {
            get
            {
                return _infoDic["MeasurementSendFrequency"].ToInt(1000);
            }
        }
        public static int MeasurementMaxMessageCount
        {
            get
            {
                return _infoDic["MeasurementMaxMessageCount"].ToInt(1000);
            }
        }
        public static bool MeasurementEnabled
        {
            get
            {
                return _onOffDic["MeasurementEnabled"];
            }
        }
        public static string MeasurementDBRetentionPolicy
        {
            get
            {
                return "default";
            }
        }
        public static string MeasurementDB
        {
            get
            {
                return AppContext._infoDic["MeasurementDB"];
            }
        }
        public static string MacAddress
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(AppContext._macAddress))
                    {
                        foreach (ManagementObject instance in (new ManagementClass("Win32_NetworkAdapterConfiguration")).GetInstances())
                        {
                            if ((bool)instance["IPEnabled"])
                            {
                                if (instance.Properties["IpAddress"].Value is Array)
                                {
                                    AppContext._macAddress = instance["MacAddress"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    AppContext._macAddress = "UnknownMac";
                }
                return AppContext._macAddress;
            }
        }
        public static int LogMaxQueueSize
        {
            get
            {
                return AppContext._infoDic["LogMaxQueueSize"].ToInt(10000);
            }
        }
        public static int LogSendFrequency
        {
            get
            {
                return AppContext._infoDic["LogSendFrequency"].ToInt(1000);
            }
        }
        public static int LogMaxMessageCount
        {
            get
            {
                return AppContext._infoDic["LogMaxMessageCount"].ToInt(1000);
            }
        }
        public static bool LogEnabled
        {
            get
            {
                bool flag;
                if (!AppContext._onOffDic["LogEnabled"])
                {
                    flag = false;
                }
                else
                {
                    flag = (AppContext._onOffDic["WriteRemoteDB"] ? true : AppContext._onOffDic["WriteLocalFile"]);
                }
                return flag;
            }
        }
        public static string LocationServerURI
        {
            get
            {
                string item;
                if (!string.IsNullOrWhiteSpace(AppContext._infoDic["Location"]))
                {
                    item = AppContext._infoDic["Location"];
                }
                else
                {
                    item = null;
                }
                return item;
            }
        }
        public static string LocalFilePath
        {
            get
            {
                return AppContext._infoDic["LocalFilePath"];
            }
        }
        public static bool IsDebug
        {
            get
            {
                return AppContext._onOffDic["IsDebug"];
            }
        }
        public static string IPv4
        {
            get
            {
                if (string.IsNullOrEmpty(AppContext._localIPv4))
                {
                    Dns.GetHostEntry(string.Empty).AddressList.ToList<IPAddress>();
                    IEnumerable<IPAddress> addressList =
                        from ip in (IEnumerable<IPAddress>)Dns.GetHostEntry(string.Empty).AddressList
                        where ip.AddressFamily == AddressFamily.InterNetwork
                        select ip;
                    if (addressList == null)
                    {
                        AppContext._localIPv4 = "UnknownIP";
                    }
                    else
                    {
                        AppContext._localIPv4 = string.Join<IPAddress>(",", addressList);
                    }
                }
                return AppContext._localIPv4;
            }
        }
        public static bool DalMeasurementEnabled
        {
            get
            {
                return AppContext._onOffDic["DalMeasurementEnabled"];
            }
        }
        public static bool DalLogEnabled
        {
            get
            {
                return AppContext._onOffDic["DalLogEnabled"];
            }
        }
        public static string ComputerName
        {
            get
            {
                if (string.IsNullOrEmpty(AppContext._computerName))
                {
                    AppContext._computerName = System.Environment.GetEnvironmentVariable("ComputerName");
                }
                return AppContext._computerName;
            }
        }
        public static string BucketName
        {
            get
            {
                return AppContext._infoDic["BucketName"];
            }
        }
        public static string BucketPassword
        {
            get
            {
                return AppContext._infoDic["BucketPassword"];
            }
        }
        public static string CacheConfigString
        {
            get
            {
                return AppContext._infoDic["CacheConfigString"];
            }
        }

        public static string CacheType
        {
            get
            {
                return AppContext._infoDic["CacheType"];
            }
        }

        #endregion
    }
}
