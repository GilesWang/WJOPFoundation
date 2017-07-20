using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Log.Common;
using WJOP.Foundation.Log.SDK.Extensions;

namespace WJOP.Foundation.Log.SDK
{
    internal sealed class Logger : ILog
    {
        private string _logProvider;
        public Logger(string logProvider)
        {
            Init(logProvider);
        }

        private void Init(string logProvider)
        {
            this._logProvider = logProvider;
        }

        public void Debug(Exception exception)
        {
            Debug(null, exception.ToLogText(), null);
        }

        public void Debug(string content)
        {
            Debug(null, content, null);
        }

        public void Debug(string title, Exception exception)
        {
            Debug(title, exception.ToLogText(), null);
        }

        public void Debug(string title, string content)
        {
            Debug(title, content, null);
        }

        public void Debug(Exception exception, IDictionary<string, Tag> tags)
        {
            Debug(null, exception.ToLogText(), tags);
        }

        public void Debug(string content, IDictionary<string, Tag> tags)
        {
            Debug(null, content, tags);
        }

        public void Debug(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Debug(title, exception.ToLogText(), tags);
        }

        public void Debug(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Debug, _logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }

        public void Error(Exception exception)
        {
            Error(null, exception.ToLogText(), null);
        }

        public void Error(string content)
        {
            Error(null, content, null);
        }

        public void Error(string title, Exception exception)
        {
            Error(title, exception.ToLogText(), null);
        }

        public void Error(string title, string content)
        {
            Error(title, content, null);
        }

        public void Error(Exception exception, IDictionary<string, Tag> tags)
        {
            Error(null, exception.ToLogText(), tags);
        }

        public void Error(string content, IDictionary<string, Tag> tags)
        {
            Error(null, content, tags);
        }

        public void Error(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Error(title, exception.ToLogText(), tags);
        }

        public void Error(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Error, this._logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }

        public void Fatal(Exception exception)
        {
            Fatal(null, exception.ToLogText(), null);
        }

        public void Fatal(string content)
        {
            Fatal(null, content, null);
        }

        public void Fatal(string title, string content)
        {
            Fatal(title, content, null);
        }

        public void Fatal(string title, Exception exception)
        {
            Fatal(title, exception.ToLogText(), null);
        }

        public void Fatal(Exception exception, IDictionary<string, Tag> tags)
        {
            Fatal(null, exception.ToLogText(), tags);
        }

        public void Fatal(string content, IDictionary<string, Tag> tags)
        {
            Fatal(null, content, tags);
        }

        public void Fatal(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Fatal(title, exception.ToLogText(), tags);
        }

        public void Fatal(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Fatal, this._logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }

        public void Info(Exception exception)
        {
            Info(null, exception.ToLogText(), null);
        }

        public void Info(string content)
        {
            Info(null, content, null);
        }

        public void Info(string title, string content)
        {
            Info(title, content, null);
        }

        public void Info(string title, Exception exception)
        {
            Info(title, exception.ToLogText(), null);
        }

        public void Info(Exception exception, IDictionary<string, Tag> tags)
        {
            Info(null, exception.ToLogText(), tags);
        }

        public void Info(string content, IDictionary<string, Tag> tags)
        {
            Info(null, content, tags);
        }

        public void Info(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Info(title, exception.ToLogText(), tags);
        }

        public void Info(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Info, this._logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }

        public void Trace(Exception exception)
        {
            Trace(null, exception.ToLogText(), null);
        }

        public void Trace(string content)
        {
            Trace(null, content, null);
        }

        public void Trace(string title, string content)
        {
            Trace(title, content, null);
        }

        public void Trace(string title, Exception exception)
        {
            Trace(title, exception.ToLogText(), null);
        }

        public void Trace(Exception exception, IDictionary<string, Tag> tags)
        {
            Trace(null, exception.ToLogText(), tags);
        }

        public void Trace(string content, IDictionary<string, Tag> tags)
        {
            Trace(null, content, tags);
        }

        public void Trace(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Trace(title, exception.ToLogText(), tags);
        }

        public void Trace(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Trace, this._logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }

        public void Warn(Exception exception)
        {
            Warn(null, exception.ToLogText(), null);
        }

        public void Warn(string content)
        {
            Warn(null, content, null);
        }

        public void Warn(string title, string content)
        {
            Warn(title, content, null);
        }

        public void Warn(string title, Exception exception)
        {
            Warn(title, exception.ToLogText(), null);
        }

        public void Warn(Exception exception, IDictionary<string, Tag> tags)
        {
            Warn(null, exception.ToLogText(), tags);
        }

        public void Warn(string content, IDictionary<string, Tag> tags)
        {
            Warn(null, content, tags);
        }

        public void Warn(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            Warn(title, exception.ToLogText(), tags);
        }

        public void Warn(string title, string content, IDictionary<string, Tag> tags)
        {
            LogContent logContent = LogContent.CreateLog(LogLevelEnum.Warn, this._logProvider, title, content, tags);
            LogHost.Instance.Write(logContent);
        }
    }
}
