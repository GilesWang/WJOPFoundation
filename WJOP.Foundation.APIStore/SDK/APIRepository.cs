using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WJOP.Foundation.Common;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;

namespace WJOP.Foundation.APIStore.SDK
{
    internal sealed class APIRepository
    {
        private const string APIStoreRelativeUriFormat = "LocateService?ServiceName={0}";

        private const int SyncInterval = 30000;

        public readonly static APIRepository Instance;

        private ConcurrentDictionary<string, string> apiDics = new ConcurrentDictionary<string, string>();

        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        private Thread _bgWorker;

        static APIRepository()
        {
            APIRepository.Instance = new APIRepository();
        }

        private APIRepository()
        {
            try
            {
                this.CreateBackgroudThreadToSyncLocation();
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        private void CreateBackgroudThreadToSyncLocation()
        {
            this._bgWorker = new Thread(new ThreadStart(this.SyncCache))
            {
                IsBackground = true
            };
            this._bgWorker.Start();
        }

        public string Lookup(string serviceName)
        {
            string empty = string.Empty;
            if (!this.apiDics.ContainsKey(serviceName.ToLower()))
            {
                empty = this.RetrieveServiceURLByServiceName(serviceName, 0);
                if (!string.IsNullOrEmpty(empty))
                {
                    this.apiDics.TryAdd(serviceName.ToLower(), empty);
                }
            }
            else
            {
                empty = this.apiDics[serviceName.ToLower()];
            }
            return empty;
        }

        private void Refresh()
        {
            if (!this.apiDics.IsEmpty)
            {
                List<string> list = this.apiDics.Keys.ToList<string>();
                try
                {
                    foreach (string str in list)
                    {
                        string str1 = this.RetrieveServiceURLByServiceName(str, 0);
                        if (!string.IsNullOrEmpty(str1))
                        {
                            if (!string.Equals(this.apiDics[str.ToLower()], str1, StringComparison.OrdinalIgnoreCase))
                            {
                                this.apiDics[str.ToLower()] = str1;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    DebugUtil.LogException(exception);
                }
            }
        }

        private string RetrieveServiceURLByServiceName(string serviceName, int retryCounter = 0)
        {
            string empty = string.Empty;
            string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.ApiStore);
            Uri uri = new Uri(new Uri(appUri), string.Format("LocateService?ServiceName={0}", serviceName));
            try
            {
                byte[] numArray = (new WebClient()).DownloadData(uri);
                string str = Encoding.UTF8.GetString(numArray);
                FoundationResponse<string> foundationResponse = this.serializer.Deserialize(str, typeof(FoundationResponse<string>)) as FoundationResponse<string>;
                if (foundationResponse.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(foundationResponse.Data))
                    {
                        empty = foundationResponse.Data;
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
            if (string.IsNullOrEmpty(empty))
            {
                if (retryCounter < 2)
                {
                    int num = retryCounter + 1;
                    retryCounter = num;
                    this.RetrieveServiceURLByServiceName(serviceName, num);
                }
            }
            return empty;
        }

        private void SyncCache()
        {
            while (true)
            {
                this.Refresh();
                Thread.Sleep(30000);
            }
        }
    }
}
