using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public static class MMConstant
    {
        public const string HTTPHeader_RequestID = "WJOPReqCtxRequestID";

        public const string HTTPHeader_Depth = "WJOPReqCtxDepth";

        public const string HTTPHeader_SequenceNum = "WJOPReqCtxSequenceNum";

        public const string HTTPHeader_CallingService = "WJOPReqCtxCallingService";

        public const string HTTPHeader_ExecutingService = "WJOPReqCtxExecutingService";

        public const string HTTPHeader_ConsumeIP = "WJOPConsumeIP";

        public const string HTTPHeader_AppKey = "AppKey";

        public const string DefaultServiceName = "Unknown Service";

        public const string RequestTimeLine_MeasurementName = "test_write4_timeline";

        public const string RequestTimeLine_RequestID = "request_id";

        public const string RequestTimeLine_Depth = "depth";

        public const string RequestTimeLine_ServerName = "server_name";

        public const string RequestTimeLine_ServerIP = "server_ip";

        public const string RequestTimeLine_ProcessID = "process_id";

        public const string RequestTimeLine_ThreadID = "thread_id";

        public const string RequestTimeLine_ServiceName = "service_name";

        public const string RequestTimeLine_MethodName = "method_name";

        public const string RequestTimeLine_MethodIn = "method_in";

        public const string RequestTimeLine_MethodExecTime = "method_exec_time";

        public const string RequestTimeLine_Time = "time";

        public const string RequestTimeLine_SequenceNum = "sequence_num";

        public const string MeasurementScope_Statistic_Hit = "hit";

        public const string MeasurementScope_Statistic_MethodExecTime = "scope_exec_time";

        public const string MeasurementScope_Placeholder_HTTP_Call = "MeasurementScope_Placeholder_HTTP_Call";

        public const string MetircNameRequestTrace = "mscope_request_trace";

        public const string MetircNameScopeStatistic = "mscope_statistic";

        public const string CommandPara_DBName = "dbname";

        public const string CommandPara_QueryText = "query";

        public const string DateTimeFormatOfMetircPoint = "yyyy/MM/dd HH:mm:ss.ffffff";

        public const string WJOPInfluxDBNull = "WJOPInfluxDBNull";

        public const string AppSettingKeyGlobalDefaultDB = "mmdefaultdb";

        public const string AppSettingKeyDBUser = "mmdbuser";

        public const string AppSettingKeyDBPwd = "mmdbpwd";

        public const string AppSettingKeyDBEngineBatchSize = "mmdbengbatchsize";

        public const string AppSettingKeyDBEngineBatchInterval = "mmdbengbatchinterval";

        public const string SDKVersion = "1.4";
    }
}
