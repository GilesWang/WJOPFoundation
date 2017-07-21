using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common;
using WJOP.Foundation.Common.Queue;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;
using WJOP.Foundation.Measurement.Common;

namespace WJOP.Foundation.Measurement.SDK
{
    internal sealed class MeasurementHost
    {
        private const string WorkerQueueGroupName = "WorkerQueueGroup";

        private int _batchSize = 5000;

        private int _flushIntervalInMS = 10000;

        private WorkerQueueGroup<MetricPoint> _queueGroup;

        public readonly static MeasurementHost Instance;

        static MeasurementHost()
        {
            Instance = new MeasurementHost();
        }

        private MeasurementHost()
        {
            this._queueGroup = new WorkerQueueGroup<MetricPoint>(this._batchSize, this._flushIntervalInMS, "WorkerQueueGroup");
            this._queueGroup.Flush += (new Action<WorkerQueue<MetricPoint>, List<MetricPoint>>(this.ProcessPoints));
        }

        public List<MetricSerie> PerformQuery(string db, string query)
        {
            List<MetricSerie> metricSeries;
            ParamterUtil.CheckEmptyString("db", db);
            ParamterUtil.CheckEmptyString("query", query);
            List<MetricSerie> metricSeries1 = new List<MetricSerie>();
            try
            {
                DebugUtil.Log(string.Format("MeasurementHost: performing query '{0}' on db '{1}'.", query, db));
                string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.MeasurementApi);
                if (string.IsNullOrEmpty(appUri))
                {
                    DebugUtil.Log("MeasurementHost: failed to fetch measurementServerInfo.Uri from location.");
                }
                else
                {
                    string str = string.Format("{0}api/measurement/run", appUri);
                    Command command = new Command()
                    {
                        Type = CommandType.Read
                    };
                    command.Parameters.Add("dbname", db);
                    command.Parameters.Add("query", query);
                    FoundationResponse<string> result = ProxyBase.Call<FoundationResponse<string>, Command>(str, command, ApiHttpMethod.POST, 180000, null).Result;
                    if ((result == null ? true : string.IsNullOrEmpty(result.Data)))
                    {
                        DebugUtil.Log(string.Format("MeasurementHost: query '{0}' on db '{1}' return empty.", query, db));
                    }
                    else
                    {
                        metricSeries1 = JsonSerializerUtil.Deserialize<List<MetricSerie>>(result.Data);
                    }
                    metricSeries = metricSeries1;
                    return metricSeries;
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
            metricSeries = metricSeries1;
            return metricSeries;
        }

        private void ProcessPoints(object sender, List<MetricPoint> points)
        {
            try
            {
                if ((points == null ? false : points.Count > 0))
                {
                    if ((sender == null ? false : sender is WorkerQueue<MetricPoint>))
                    {
                        string[] strArrays = MetricPoint.ParseDBRP((sender as WorkerQueue<MetricPoint>).QueueName);
                        string str = strArrays[0];
                        PushToServer(points, str, strArrays[1]);
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        private static FoundationResponse<string> PushToServer(List<MetricPoint> points, string db, string rp)
        {
            string str;
            FoundationResponse<string> foundationResponse = null;
            if ((points == null ? false : points.Count > 0))
            {
                try
                {
                    string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.MeasurementApi);
                    if (string.IsNullOrEmpty(appUri))
                    {
                        DebugUtil.Log("MeasurementHost: failed to fetch measurementServerInfo.Uri from location.");
                        foundationResponse = new FoundationResponse<string>()
                        {
                            IsSuccess = false
                        };
                    }
                    else
                    {
                        str = (!appUri.EndsWith("/") ? string.Format("{0}/api/measurement/write/", appUri) : string.Format("{0}api/measurement/write/", appUri));
                        MeasurementRequest measurementRequest = new MeasurementRequest();
                        measurementRequest.MetricPoints.AddRange(points);
                        measurementRequest.DBName = db;
                        measurementRequest.RetentionPolicy = rp;
                        DebugUtil.Log(string.Format("MeasurementHost: Sending {0} points to db '{1}' via {2}.", points.Count, measurementRequest.DBName, str));
                        foundationResponse = ProxyBase.Call<FoundationResponse<string>, object>(str, measurementRequest, ApiHttpMethod.POST, 180000, null).Result;
                        if (foundationResponse == null)
                        {
                            foundationResponse = new FoundationResponse<string>();
                        }
                        foundationResponse.IsSuccess = true;
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    foundationResponse.IsSuccess = false;
                    foundationResponse.ErrMsg = string.Format("{0}\r\n{1}", exception.Message, exception.StackTrace);
                    DebugUtil.LogException(exception);
                }
            }
            return foundationResponse;
        }

        public void Submit(MetricPoint point)
        {
            if ((point == null ? false : point.IsValid))
            {
                this._queueGroup.Enqueue(MetricPoint.CombineDBRP(point.DB, point.RP), point);
            }
        }
    }
}
