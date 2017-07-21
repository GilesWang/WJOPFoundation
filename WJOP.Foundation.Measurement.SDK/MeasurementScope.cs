using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using WJOP.Foundation.Common;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Measurement.SDK
{
    public sealed class MeasurementScope:IDisposable
    {
        private string _scopeName;

        private IDictionary<string, string> _userTags;

        private Stopwatch _sw;

        private DateTime _start;

        private DateTime _end;

        private long _scopeExecTime;

        private long _sequenceNum;

        private long _depth;

        public long ScopeExecTime
        {
            get
            {
                return this._sw.ElapsedMilliseconds;
            }
        }

        public MeasurementScope(string scopeName, IDictionary<string, string> userTags = null)
        {
            ParamterUtil.CheckEmptyString("scopeName", scopeName);
            try
            {
                this._scopeName = scopeName;
                this._userTags = new Dictionary<string, string>();
                if (userTags != null)
                {
                    foreach (string key in userTags.Keys)
                    {
                        this._userTags.Add(key, userTags[key]);
                    }
                }
                this._scopeExecTime = (long)-1;
                this._sw = Stopwatch.StartNew();
                this.BeginTrace();
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            this.EndTrace();
            if (isDisposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        ~MeasurementScope()
        {
            this.Dispose(false);
        } 
        #endregion

        private void BeginTrace()
        {
            try
            {
                this._start = DateTime.UtcNow;
                this._sw.Start();
                this.FillInDefaultTags();
                if (AppContext.MeasurementTraceEnabled)
                {
                    WJOPRequestContext current = WJOPRequestContext.Current;
                    current.SequenceNum = current.SequenceNum + (long)1;
                    WJOPRequestContext depth = WJOPRequestContext.Current;
                    depth.Depth = depth.Depth + (long)1;
                    this._sequenceNum = WJOPRequestContext.Current.SequenceNum;
                    this._depth = WJOPRequestContext.Current.Depth;
                    Dictionary<string, float> strs = new Dictionary<string, float>()
                    {
                        { "method_in", 1f },
                        { "depth", (float)this._depth },
                        { "sequence_num", (float)this._sequenceNum }
                    };
                    Dictionary<string, float> strs1 = strs;
                    DebugUtil.Log(string.Format(">>>>>BeginTrace {0}, CtxDetail {1}", this._scopeName, WJOPRequestContext.Current.DumpString));
                    MeasurementHelper.WritePoint("mscope_request_trace", strs1, this._userTags, this._start);
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }
        private void EndTrace()
        {
            try
            {
                if (this._sw.IsRunning)
                {
                    this._sw.Stop();
                }
                this._scopeExecTime = this._sw.ElapsedMilliseconds;
                this._end = DateTime.UtcNow;
                Dictionary<string, float> strs = new Dictionary<string, float>()
                {
                    { "hit", 1f },
                    { "scope_exec_time", (float)this._scopeExecTime }
                };
                MeasurementHelper.WritePoint("mscope_statistic", strs, this._userTags, this._end);
                if (AppContext.MeasurementTraceEnabled)
                {
                    Dictionary<string, float> strs1 = new Dictionary<string, float>()
                    {
                        { "method_in", -1f },
                        { "depth", (float)this._depth },
                        { "sequence_num", (float)this._sequenceNum },
                        { "method_exec_time", (float)this._scopeExecTime }
                    };
                    Dictionary<string, float> strs2 = strs1;
                    object[] requestId = new object[] { this._scopeName, WJOPRequestContext.Current.RequestId, this._depth, this._sequenceNum };
                    DebugUtil.Log(string.Format("<<<<<EndTrace {0}, ReqID:{1}, Depth:{2}, SeqNO:{3}", requestId));
                    MeasurementHelper.WritePoint("mscope_request_trace", strs2, this._userTags, this._end);
                    WJOPRequestContext current = WJOPRequestContext.Current;
                    current.Depth = current.Depth - (long)1;
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }
        private void FillInDefaultTags()
        {
            this._userTags["method_name"] = this._scopeName;
            if (AppContext.MeasurementTraceEnabled)
            {
                this._userTags["request_id"] = WJOPRequestContext.Current.RequestId;
                this._userTags["service_name"] = WJOPRequestContext.Current.ExecutingService;
                this._userTags["server_name"] = Environment.MachineName;
                this._userTags["server_ip"] = AppContext.IPv4;
                IDictionary<string, string> str = this._userTags;
                int id = Process.GetCurrentProcess().Id;
                str["process_id"] = id.ToString();
                IDictionary<string, string> strs = this._userTags;
                id = Thread.CurrentThread.ManagedThreadId;
                strs["thread_id"] = id.ToString();
            }
        }
    }
}
