using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;

namespace WJOP.Foundation.Cache.SDK
{
    internal sealed class CouchbaseConfigEntity
    {
        private IDictionary<string, string> defaultValue = new Dictionary<string, string>()
        {
            { "bucketname", "default" },
            { "password", "" },
            { "usessl", "false" },
            { "defaultconnectionlimit", "300000" },
            { "enabletcpkeepalives", "true" },
            { "tcpkeepalivetime", "360000" },
            { "tcpkeepaliveinterval", "5000" },
            { "defaultoperationlifespan", "300000" },
            { "poolmaxsize", "20" },
            { "poolminsize", "5" },
            { "poolsendtimeout", "300000" },
            { "enabledlocation", "true" },
            { "servers", "http://tgop-cache.vipabc.com:8091/pools/" }
        };

        public string BucketName
        {
            get;
            set;
        }

        public int DefaultConnectionLimit
        {
            get;
            set;
        }

        public uint DefaultOperationLifespan
        {
            get;
            set;
        }

        public bool EnabledLocation
        {
            get;
            set;
        }

        public bool EnableTcpKeepAlives
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int PoolMaxSize
        {
            get;
            set;
        }

        public int PoolMinSize
        {
            get;
            set;
        }

        public int PoolSendTimeout
        {
            get;
            set;
        }

        public List<Uri> Servers
        {
            get;
            set;
        }

        public uint TcpKeepAliveInterval
        {
            get;
            set;
        }

        public uint TcpKeepAliveTime
        {
            get;
            set;
        }

        public bool UseSsl
        {
            get;
            set;
        }

        public CouchbaseConfigEntity()
        {
            this.SetDefaultValue(AppContext.CacheConfigString);
            this.BucketName = this.defaultValue["bucketname"];
            this.Password = this.defaultValue["password"];
            this.UseSsl = bool.Parse(this.defaultValue["usessl"]);
            this.DefaultConnectionLimit = int.Parse(this.defaultValue["defaultconnectionlimit"]);
            this.EnableTcpKeepAlives = bool.Parse(this.defaultValue["enabletcpkeepalives"]);
            this.TcpKeepAliveTime = uint.Parse(this.defaultValue["tcpkeepalivetime"]);
            this.TcpKeepAliveInterval = uint.Parse(this.defaultValue["tcpkeepaliveinterval"]);
            this.DefaultOperationLifespan = uint.Parse(this.defaultValue["defaultoperationlifespan"]);
            this.PoolMaxSize = int.Parse(this.defaultValue["poolmaxsize"]);
            this.PoolMinSize = int.Parse(this.defaultValue["poolminsize"]);
            this.PoolSendTimeout = int.Parse(this.defaultValue["poolsendtimeout"]);
            this.EnabledLocation = bool.Parse(this.defaultValue["enabledlocation"]);
            this.Servers = new List<Uri>();
            if (!this.EnabledLocation)
            {
                string item = this.defaultValue["servers"];
                char[] chrArray = new char[] { '|' };
                foreach (string list in item.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).ToList<string>())
                {
                    this.Servers.Add(new Uri(list));
                }
            }
            else
            {
                string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.CouchBaseSvc);
                this.Servers.Add(new Uri(appUri));
            }
        }

        private void SetDefaultValue(string str)
        {
            if ((string.IsNullOrWhiteSpace(str) ? false : !string.IsNullOrEmpty(str)))
            {
                char[] chrArray = new char[] { ',' };
                foreach (string list in str.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).ToList<string>())
                {
                    chrArray = new char[] { ':' };
                    string[] strArrays = list.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                    if (((int)strArrays.Length != 2 ? false : this.defaultValue.Keys.Contains(strArrays[0].ToLower())))
                    {
                        this.defaultValue[strArrays[0].ToLower()] = strArrays[1];
                    }
                }
            }
        }
    }
}
