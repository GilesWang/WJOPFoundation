using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WJOP.Foundation.Location.Models;
using WJOP.Foundation.Common;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Common.Extensions;

namespace WJOP.Foundation.Location
{
    public class LocationHelper : IRefreshable
    {
        private const int SyncInterval = 6000;
        private static ConcurrentDictionary<string, string> _appUriDic;
        private Thread _bgWorker;
        private static readonly Lazy<LocationHelper> lazy;

        public static LocationHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        static LocationHelper()
        {
            lazy = new Lazy<LocationHelper>(() => new LocationHelper());
        }
        private LocationHelper()
        {
            try
            {
                _appUriDic = new ConcurrentDictionary<string, string>();
                CreateBackgroudThreadToSyncLocation();
            }
            catch (Exception ex)
            {
                DebugUtil.LogException(ex);
            }
        }
        private void CreateBackgroudThreadToSyncLocation()
        {
            this._bgWorker = new Thread(new ThreadStart(this.SynCache))
            {
                IsBackground = true
            };
            this._bgWorker.Start();
        }
        private void SynCache()
        {
            while (true)
            {
                this.Refresh();
                Thread.Sleep(SyncInterval);
            }
        }
        private string GetAppLocation(string appName)
        {
            string data = null;
            try
            {
                string str = string.Concat(AppContext.LocationServerURI, "api/Location/GetAppLocation");
                DebugUtil.CollectDebugInfo(str, "WJOP.Foundation.Location.LocationHelper.GetAppUris line:70");
                GetLocationRequest getLocationRequest = new GetLocationRequest()
                {
                    AppKey = AppContext.AppKey,
                    AppName = appName
                };
                EnvInfo envInfo = new EnvInfo()
                {
                    IP = AppContext.IPv4,
                    Env = AppContext.Environment
                };
                getLocationRequest.EnvInfo = envInfo;
                GetLocationRequest getLocationRequest1 = getLocationRequest;
                FoundationResponse<string> result = str.PostWebApi<FoundationResponse<string>, GetLocationRequest>(getLocationRequest1, null, 180000).Result;
                if (!result.IsSuccess)
                {
                    DebugUtil.Log(string.Format("LocationHelper: Failed to match service uri for {0}, error Message: {1}", appName, result.ErrMsg));
                }
                else
                {
                    DebugUtil.Log(string.Format("LocationHelper: Found service uri for {0} : {1}", appName, result.Data));
                    data = result.Data;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                DebugUtil.LogException(exception);
                DebugUtil.CollectDebugInfo(exception, "WJOP.Foundation.Location.LocationHelper.GetAppUris line:98");
            }
            return data;
        }
        private string GetAppUri(AppNameEnum appName)
        {
            string empty = string.Empty;
            try
            {
                string str = appName.ToString();
                DebugUtil.CollectDebugInfo(str, "WJOP.Foundation.Location.LocationHelper.GetAppUri");
                if (!LocationHelper._appUriDic.TryGetValue(str, out empty))
                {
                    empty = this.GetAppLocation(str);
                    LocationHelper._appUriDic.TryAdd(str, empty);
                }
                else
                {
                    empty = LocationHelper._appUriDic[str];
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                DebugUtil.LogException(exception);
                DebugUtil.CollectDebugInfo(exception, "WJOP.Foundation.Location.LocationHelper.GetAppUri");
            }
            return empty;
        }
        public void Refresh()
        {
            if (!LocationHelper._appUriDic.IsEmpty)
            {
                List<string> list = LocationHelper._appUriDic.Keys.ToList<string>();
                IDictionary<string, string> data = null;
                try
                {
                    string str = string.Concat(AppContext.LocationServerURI, "api/Location/BatchGetAppLocations");
                    DebugUtil.CollectDebugInfo(str, "WJOP.Foundation.Location.LocationHelper.Refresh");
                    BatchGetLocationRequest batchGetLocationRequest = new BatchGetLocationRequest()
                    {
                        AppKey = AppContext.AppKey,
                        AppNameList = list
                    };
                    EnvInfo envInfo = new EnvInfo()
                    {
                        IP = AppContext.IPv4,
                        Env = AppContext.Environment
                    };
                    batchGetLocationRequest.EnvInfo = envInfo;
                    BatchGetLocationRequest batchGetLocationRequest1 = batchGetLocationRequest;
                    FoundationResponse<IDictionary<string, string>> result = str.PostWebApi<FoundationResponse<IDictionary<string, string>>, BatchGetLocationRequest>(batchGetLocationRequest1, null, 180000).Result;
                    if (!result.IsSuccess)
                    {
                        DebugUtil.Log(string.Format("LocationHelper: Failed to Refresh error Message: {0}", result.ErrMsg));
                    }
                    else
                    {
                        data = result.Data;
                        foreach (string key in data.Keys)
                        {
                            DebugUtil.Log(string.Format("WJOP.Foundation.Location.LocationHelper.Refresh key:[{0}],old value [{1}] , new Value:[{2}]", key, LocationHelper._appUriDic[key], data[key]));
                            LocationHelper._appUriDic[key] = data[key];
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    DebugUtil.LogException(exception);
                    DebugUtil.CollectDebugInfo(exception, "WJOP.Foundation.Location.LocationHelper.Refresh");
                }
            }
            else
            {
                DebugUtil.Log("WJOP.Foundation.Location.LocationHelper.Refresh: don't need to refresh");
            }
        }
    }
}
