using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.SDK
{
    internal sealed class NullLogger : ILog
    {
        private readonly static NullLogger _nullLogger;
        public static NullLogger Instance
        {
            get
            {
                return NullLogger._nullLogger;
            }
        }

        static NullLogger()
        {
            NullLogger._nullLogger = new NullLogger();
        }

        private NullLogger()
        {

        }
        private void DoNothing()
        {
        }
        public void Debug(Exception exception)
        {
            DoNothing();
        }

        public void Debug(string content)
        {
            DoNothing();
        }

        public void Debug(string title, Exception exception)
        {
            DoNothing();
        }

        public void Debug(string title, string content)
        {
            DoNothing();
        }

        public void Debug(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Debug(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Debug(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Debug(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Error(Exception exception)
        {
            DoNothing();
        }

        public void Error(string content)
        {
            DoNothing();
        }

        public void Error(string title, Exception exception)
        {
            DoNothing();
        }

        public void Error(string title, string content)
        {
            DoNothing();
        }

        public void Error(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Error(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Error(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Error(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Fatal(Exception exception)
        {
            DoNothing();
        }

        public void Fatal(string content)
        {
            DoNothing();
        }

        public void Fatal(string title, string content)
        {
            DoNothing();
        }

        public void Fatal(string title, Exception exception)
        {
            DoNothing();
        }

        public void Fatal(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Fatal(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Fatal(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Fatal(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Info(Exception exception)
        {
            DoNothing();
        }

        public void Info(string content)
        {
            DoNothing();
        }

        public void Info(string title, Exception exception)
        {
            DoNothing();
        }

        public void Info(string title, string content)
        {
            DoNothing();
        }

        public void Info(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Info(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Info(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Info(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Trace(Exception exception)
        {
            DoNothing();
        }

        public void Trace(string content)
        {
            DoNothing();
        }

        public void Trace(string title, string content)
        {
            DoNothing();
        }

        public void Trace(string title, Exception exception)
        {
            DoNothing();
        }

        public void Trace(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Trace(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Trace(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Trace(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Warn(Exception exception)
        {
            DoNothing();
        }

        public void Warn(string content)
        {
            DoNothing();
        }

        public void Warn(string title, string content)
        {
            DoNothing();
        }

        public void Warn(string title, Exception exception)
        {
            DoNothing();
        }

        public void Warn(Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Warn(string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Warn(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }

        public void Warn(string title, string content, IDictionary<string, Tag> tags)
        {
            DoNothing();
        }
    }
}
