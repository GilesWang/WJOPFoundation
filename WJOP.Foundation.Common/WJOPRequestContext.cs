using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Common
{
    public class WJOPRequestContext
    {
        private static WJOPRequestContext _instanceOfCurrentThread;

        public string CallingService
        {
            get
            {
                return this.GetFromLogicalCallContext("TGOPReqCtxCallingService");
            }
            set
            {
                this.SetToLogicalCallContext("TGOPReqCtxCallingService", value);
            }
        }

        public static WJOPRequestContext Current
        {
            get
            {
                if (WJOPRequestContext._instanceOfCurrentThread == null)
                {
                    WJOPRequestContext._instanceOfCurrentThread = new WJOPRequestContext();
                }
                return WJOPRequestContext._instanceOfCurrentThread;
            }
        }

        public long Depth
        {
            get
            {
                long num;
                string fromLogicalCallContext = this.GetFromLogicalCallContext("TGOPReqCtxDepth");
                num = (!string.IsNullOrEmpty(fromLogicalCallContext) ? long.Parse(fromLogicalCallContext) : (long)0);
                return num;
            }
            set
            {
                this.SetToLogicalCallContext("TGOPReqCtxDepth", value.ToString());
            }
        }

        public string DumpCurrentHttpHeaders
        {
            get
            {
                object[] item = new object[] { "TGOPReqCtxRequestID", HttpContext.Current.Request.Headers["TGOPReqCtxRequestID"], "TGOPReqCtxDepth", HttpContext.Current.Request.Headers["TGOPReqCtxDepth"], "TGOPReqCtxSequenceNum", HttpContext.Current.Request.Headers["TGOPReqCtxSequenceNum"] };
                return string.Format("{0}:{1}, {2}:{3}, {4}:{5}", item);
            }
        }

        public string DumpString
        {
            get
            {
                object[] requestId = new object[] { WJOPRequestContext.Current.RequestId, WJOPRequestContext.Current.Depth, WJOPRequestContext.Current.SequenceNum, WJOPRequestContext.Current.ExecutingService, WJOPRequestContext.Current.CallingService };
                return string.Format("ReqID:{0}, Depth:{1}, SeqNO:{2}, ExecService:{3}, CallingService:{4}", requestId);
            }
        }

        public string ExecutingService
        {
            get
            {
                return this.GetFromLogicalCallContext("TGOPReqCtxExecutingService");
            }
            set
            {
                this.SetToLogicalCallContext("TGOPReqCtxExecutingService", value);
            }
        }

        public string RequestId
        {
            get
            {
                return this.GetFromLogicalCallContext("TGOPReqCtxRequestID");
            }
            set
            {
                this.SetToLogicalCallContext("TGOPReqCtxRequestID", value);
            }
        }

        public long SequenceNum
        {
            get
            {
                long num;
                string fromLogicalCallContext = this.GetFromLogicalCallContext("TGOPReqCtxSequenceNum");
                num = (!string.IsNullOrEmpty(fromLogicalCallContext) ? long.Parse(fromLogicalCallContext) : (long)0);
                return num;
            }
            set
            {
                this.SetToLogicalCallContext("TGOPReqCtxSequenceNum", value.ToString());
            }
        }

        private WJOPRequestContext()
        {
        }

        public static void AttachToHttpHeaders(NameValueCollection headers)
        {
            try
            {
                if (AppContext.MeasurementTraceEnabled)
                {
                    if (!string.IsNullOrEmpty(headers["TGOPReqCtxRequestID"]))
                    {
                        headers.Remove("TGOPReqCtxRequestID");
                    }
                    if (!string.IsNullOrEmpty(headers["TGOPReqCtxDepth"]))
                    {
                        headers.Remove("TGOPReqCtxDepth");
                    }
                    if (!string.IsNullOrEmpty(headers["TGOPReqCtxSequenceNum"]))
                    {
                        headers.Remove("TGOPReqCtxSequenceNum");
                    }
                    if (!string.IsNullOrEmpty(headers["TGOPReqCtxCallingService"]))
                    {
                        headers.Remove("TGOPReqCtxCallingService");
                    }
                    DebugUtil.Log(string.Format("attaching tgop request context to http headers, ctx is: {0}", WJOPRequestContext.Current.DumpString));
                    long sequenceNum = WJOPRequestContext.Current.SequenceNum;
                    headers["TGOPReqCtxSequenceNum"] = sequenceNum.ToString();
                    headers["TGOPReqCtxRequestID"] = WJOPRequestContext.Current.RequestId;
                    sequenceNum = WJOPRequestContext.Current.Depth;
                    headers["TGOPReqCtxDepth"] = sequenceNum.ToString();
                    headers["TGOPReqCtxCallingService"] = WJOPRequestContext.Current.ExecutingService;
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        public static void Cleanup()
        {
            WJOPRequestContext._instanceOfCurrentThread = null;
        }

        private string GetFromHttpRequestHeaders(string key)
        {
            string item = null;
            if (HttpContext.Current != null)
            {
                if ((HttpContext.Current.Request == null ? false : HttpContext.Current.Request.Headers.AllKeys.Contains<string>(key)))
                {
                    item = HttpContext.Current.Request.Headers[key];
                }
            }
            return item;
        }

        private string GetFromLogicalCallContext(string key)
        {
            string fromHttpRequestHeaders;
            object obj = CallContext.LogicalGetData(key);
            if (obj == null)
            {
                fromHttpRequestHeaders = this.GetFromHttpRequestHeaders(key);
                if (!string.IsNullOrEmpty(fromHttpRequestHeaders))
                {
                    this.SetToLogicalCallContext(key, fromHttpRequestHeaders);
                }
            }
            else
            {
                Debug.Assert(obj is string);
                fromHttpRequestHeaders = obj as string;
            }
            return fromHttpRequestHeaders;
        }

        private void SetToLogicalCallContext(string key, string value)
        {
            CallContext.LogicalSetData(key, value);
        }
    }
}
