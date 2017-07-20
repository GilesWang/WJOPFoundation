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
        public void Debug(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(string content)
        {
            throw new NotImplementedException();
        }

        public void Debug(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Debug(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Debug(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Debug(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Debug(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string content)
        {
            throw new NotImplementedException();
        }

        public void Error(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Error(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Error(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Error(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string content)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(string content)
        {
            throw new NotImplementedException();
        }

        public void Info(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Info(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Info(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Info(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Info(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(string content)
        {
            throw new NotImplementedException();
        }

        public void Trace(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Trace(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Trace(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Trace(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Trace(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Warn(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(string content)
        {
            throw new NotImplementedException();
        }

        public void Warn(string title, string content)
        {
            throw new NotImplementedException();
        }

        public void Warn(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Warn(string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Warn(string title, Exception exception, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }

        public void Warn(string title, string content, IDictionary<string, Tag> tags)
        {
            throw new NotImplementedException();
        }
    }
}
