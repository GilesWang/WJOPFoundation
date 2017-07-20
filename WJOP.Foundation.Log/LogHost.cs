using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Extensions;
using WJOP.Foundation.Common.Queue;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;
using WJOP.Foundation.Log.Common;
using WJOP.Foundation.Log.SDK;
using WJOP.Foundation.Log.SDK.Extensions;

namespace WJOP.Foundation.Log
{
    internal sealed class LogHost
    {
        #region fields
        private readonly static object _syncLock;
        private HostInfo _hostInfo;
        private string _appKey;
        private WorkerQueue<LogContent> _localLogQueue;
        private int _writeLogBatchSize;
        private int _writeLogCoolDownTime;
        private int _localQueueSize;
        private bool _isWriteLocalLog;
        private Lazy<string> _logAPIUrl = new Lazy<string>(new Func<string>(GetLogAPIUrl));
        private readonly static Lazy<LogHost> lazy;
        #endregion

        public static LogHost Instance
        {
            get { return lazy.Value; }
        }
        static LogHost()
        {
            _syncLock = new object();
            lazy = new Lazy<LogHost>(() => new LogHost());
        }
        private LogHost()
        {
            _hostInfo = HostUtil.GetHostInfo();
            _appKey = AppContext.AppKey;
            _writeLogBatchSize = AppContext.LogMaxMessageCount;
            _writeLogCoolDownTime = AppContext.LogSendFrequency;
            _localQueueSize = AppContext.LogMaxQueueSize;
            _isWriteLocalLog = AppContext.WriteLocalFile;
            _localLogQueue = new WorkerQueue<LogContent>(_writeLogBatchSize, _writeLogCoolDownTime,_localQueueSize, "LogSDKQueue");
            _localLogQueue.Flush += new Action<object, List<LogContent>>(WriteLog);
        }
        private void Write(LogContent logDoc)
        {
            _localLogQueue.Enqueue(logDoc);
        }

        private void WriteLog(object sender, List<LogContent> logDocs)
        {
            LogMessage logMessage = new LogMessage()
            {
                Appkey = _appKey,
                ComputerName = _hostInfo.ComputerName,
                IP = _hostInfo.IPAddress,
                Mac = _hostInfo.MacAddress,
                Body = logDocs
            };
            WriteLogToRemotingService(logMessage);
            if (_isWriteLocalLog)
            {
                WriteToLocalFile(logMessage);
            }
        }

        private void WriteLogToRemotingService(LogMessage logMessage)
        {
            string value = _logAPIUrl.Value;
            int num = 0;
            NameValueCollection collection = new NameValueCollection() {
                {"X-WJOP-AppKey",logMessage.Appkey },
                {"X-WJOP-InstanceIP",logMessage.IP }
            };
            while (true)
            {
                if (value.PostProtobufObject(logMessage, collection) ? true : num > 3)
                {
                    break;
                }
                num++;
                Thread.Sleep(300 * num);
            }
        }

        private void WriteToLocalFile(LogMessage logMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var body in logMessage.Body)
            {
                stringBuilder.AppendLine(body.ToLogText());
            }
            if (stringBuilder.Length > 0)
            {
                WriteToLocalFile(stringBuilder.ToString());
            }
        }

        private void WriteToLocalFile(string content)
        {
            Task.Run(() =>
            {
                try
                {
                    string str = string.Concat(DateTime.Now.ToString("yyyyMMdd"), "_log.txt");
                    string str1 = string.Concat(AppContext.LocalFilePath, str);
                    lock (_syncLock)
                    {
                        if (!Directory.Exists(AppContext.LocalFilePath))
                        {
                            Directory.CreateDirectory(AppContext.LocalFilePath);
                        }
                        FileStream fileStream = new FileStream(str1, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        try
                        {
                            byte[] bytes = (new UTF8Encoding()).GetBytes(content);
                            fileStream.Position = fileStream.Length;
                            fileStream.Write(bytes, 0, bytes.Length);
                            fileStream.Flush();
                        }
                        finally
                        {
                            if (fileStream != null)
                            {
                                ((IDisposable)fileStream).Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugUtil.LogException(ex);
                }
            });
        }

        private static string GetLogAPIUrl()
        {
            string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.LogApi);
            string str = string.Format("{0}{1}", appUri, "api/log/BinaryWrite");
            DebugUtil.CollectDebugInfo(str, "WJOP.Foundation.Log.SDK.LogHost.GetLogAPIUrl line:28");
            return str;
        }
    }
}
