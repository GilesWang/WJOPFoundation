using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.Swagger
{
    public class Configuration
    {
        private string _tempFolderPath = Path.GetTempPath();
        private string _dateTimeFormat = "o";
        public const string Version = "1.0.0";
        private const string ISO8601_DATETIME_FORMAT = "o";
        public readonly static ExceptionFactory DefaultExceptionFactory;
        public Dictionary<string, string> ApiKey = new Dictionary<string, string>();
        public Dictionary<string, string> ApiKeyPrefix = new Dictionary<string, string>();
        public static Configuration Default;
        public ApiClient ApiClient;
        public string TempFolderPath
        {
            get
            {
                return this._tempFolderPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!Directory.Exists(value))
                    {
                        Directory.CreateDirectory(value);
                    }
                    if (value[value.Length - 1] != Path.DirectorySeparatorChar)
                    {
                        this._tempFolderPath = string.Concat(value, Path.DirectorySeparatorChar);
                    }
                    else
                    {
                        this._tempFolderPath = value;
                    }
                }
                else
                {
                    this._tempFolderPath = value;
                }
            }
        }
        public int Timeout
        {
            get
            {
                return this.ApiClient.RestClient.Timeout;
            }
            set
            {
                if (this.ApiClient != null)
                {
                    this.ApiClient.RestClient.Timeout = value;
                }
            }
        }
        public string UserAgent
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string AccessToken
        {
            get;
            set;
        }
        public bool EnableConsul
        {
            get;
            set;
        }
        public string ConsulServiceName
        {
            get;
            set;
        }
        public string ConsulServiceTag
        {
            get;
            set;
        }
        public Dictionary<string, string> DefaultHeader
        {
            get; set;
        }
        public string DateTimeFormat
        {
            get
            {
                return this._dateTimeFormat;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._dateTimeFormat = value;
                }
                else
                {
                    this._dateTimeFormat = "o";
                }
            }
        }
        public string Password
        {
            get;
            set;
        }
        #region ctor
        static Configuration()
        {
            Default = new Configuration(null, null, null, null, null, null, null, null, null, 100000, "WJOP-APIStore-SDK", false, null, null);
            DefaultExceptionFactory = (string methodName, IRestResponse response) =>
            {
                int statusCode = (int)response.StatusCode;
                return (statusCode < 400 ? (statusCode != 0 ? null : new ApiException(statusCode, string.Format("Error calling {0}: {1}", methodName, response.ErrorMessage), response.ErrorMessage)) : new ApiException(statusCode, string.Format("Error calling {0}: {1}", methodName, response.Content), response.Content));
            };
        }
        public Configuration(ApiClient apiClient = null, Dictionary<string, string> defaultHeader = null, string username = null, string password = null, string accessToken = null, Dictionary<string, string> apiKey = null, Dictionary<string, string> apiKeyPrefix = null, string tempFolderPath = null, string dateTimeFormat = null, int timeout = 100000, string userAgent = "TGOP-APIStore-SDK", bool enableConsul = false, string consulServiceName = null, string consulServiceTag = null)
        {
            this.setApiClientUsingDefault(apiClient);
            this.Username = username;
            this.Password = password;
            this.AccessToken = accessToken;
            this.UserAgent = userAgent;
            if (defaultHeader != null)
            {
                this.DefaultHeader = defaultHeader;
            }
            if (apiKey != null)
            {
                this.ApiKey = apiKey;
            }
            if (apiKeyPrefix != null)
            {
                this.ApiKeyPrefix = apiKeyPrefix;
            }
            this.TempFolderPath = tempFolderPath;
            this.DateTimeFormat = dateTimeFormat;
            this.Timeout = timeout;
            this.EnableConsul = enableConsul;
            this.ConsulServiceName = consulServiceName;
            this.ConsulServiceTag = consulServiceTag;
        }
        public Configuration(ApiClient apiClient)
        {
            this.setApiClientUsingDefault(apiClient);
        }
        #endregion
        public void setApiClientUsingDefault(ApiClient apiClient = null)
        {
            if (apiClient != null)
            {
                if ((Configuration.Default == null ? false : Configuration.Default.ApiClient == null))
                {
                    Configuration.Default.ApiClient = apiClient;
                }
                this.ApiClient = apiClient;
            }
            else
            {
                if ((Configuration.Default == null ? false : Configuration.Default.ApiClient == null))
                {
                    Configuration.Default.ApiClient = new ApiClient();
                }
                this.ApiClient = (Configuration.Default != null ? Configuration.Default.ApiClient : new ApiClient());
            }
        }
        public void AddDefaultHeader(string key, string value)
        {
            DefaultHeader.Add(key, value);
        }
        public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        {
            string str;
            string str1 = "";
            this.ApiKey.TryGetValue(apiKeyIdentifier, out str1);
            string str2 = "";
            str = (!this.ApiKeyPrefix.TryGetValue(apiKeyIdentifier, out str2) ? str1 : string.Concat(str2, " ", str1));
            return str;
        }
        public static string ToDebugReport()
        {
            object obj = "C# SDK (IO.Swagger) Debug Report:\n";
            object[] oSVersion = new object[] { obj, "    OS: ", Environment.OSVersion, "\n" };
            string str = string.Concat(string.Concat(oSVersion), "    .NET Framework Version: ", (
                from x in (IEnumerable<AssemblyName>)Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                where x.Name == "System.Core"
                select x).First<AssemblyName>().Version.ToString(), "\n");
            str = string.Concat(str, "    Version of the API: v1\n");
            return string.Concat(str, "    SDK Package Version: 1.0.0\n");
        }
    }
    public delegate Exception ExceptionFactory(string methodName, IRestResponse response);
}
