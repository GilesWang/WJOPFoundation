using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Log.SDK
{
    public interface ILog
    {
        void Debug(string content);

        void Debug(string content, IDictionary<string, Tag> tags);

        void Debug(Exception exception);

        void Debug(Exception exception, IDictionary<string, Tag> tags);

        void Debug(string title, string content);

        void Debug(string title, string content, IDictionary<string, Tag> tags);

        void Debug(string title, Exception exception);

        void Debug(string title, Exception exception, IDictionary<string, Tag> tags);

        void Error(string content);

        void Error(string content, IDictionary<string, Tag> tags);

        void Error(Exception exception);

        void Error(Exception exception, IDictionary<string, Tag> tags);

        void Error(string title, string content);

        void Error(string title, string content, IDictionary<string, Tag> tags);

        void Error(string title, Exception exception);

        void Error(string title, Exception exception, IDictionary<string, Tag> tags);

        void Fatal(string content);

        void Fatal(string content, IDictionary<string, Tag> tags);

        void Fatal(Exception exception);

        void Fatal(Exception exception, IDictionary<string, Tag> tags);

        void Fatal(string title, string content);

        void Fatal(string title, string content, IDictionary<string, Tag> tags);

        void Fatal(string title, Exception exception);

        void Fatal(string title, Exception exception, IDictionary<string, Tag> tags);

        void Info(string content);

        void Info(string content, IDictionary<string, Tag> tags);

        void Info(Exception exception);

        void Info(Exception exception, IDictionary<string, Tag> tags);

        void Info(string title, string content);

        void Info(string title, string content, IDictionary<string, Tag> tags);

        void Info(string title, Exception exception);

        void Info(string title, Exception exception, IDictionary<string, Tag> tags);

        void Trace(string content);

        void Trace(string content, IDictionary<string, Tag> tags);

        void Trace(Exception exception);

        void Trace(Exception exception, IDictionary<string, Tag> tags);

        void Trace(string title, string content);

        void Trace(string title, string content, IDictionary<string, Tag> tags);

        void Trace(string title, Exception exception);

        void Trace(string title, Exception exception, IDictionary<string, Tag> tags);

        void Warn(string content);

        void Warn(string content, IDictionary<string, Tag> tags);

        void Warn(Exception exception);

        void Warn(Exception exception, IDictionary<string, Tag> tags);

        void Warn(string title, string content);

        void Warn(string title, string content, IDictionary<string, Tag> tags);

        void Warn(string title, Exception exception);

        void Warn(string title, Exception exception, IDictionary<string, Tag> tags);
    }
}
