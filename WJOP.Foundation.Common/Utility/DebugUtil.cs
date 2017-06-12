using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WJOP.Foundation.Common.Utility
{
    public static class DebugUtil
    {
        public static void CollectDebugInfo(object obj, string description)
        {
            try
            {
                if (AppContext.IsDebug)
                {
                    string empty = string.Empty;
                    try
                    {
                        empty = (obj == null ? string.Empty : (new JavaScriptSerializer()).Serialize(obj));
                    }
                    catch (Exception exception)
                    {
                        empty = exception.Message;
                    }
                    object[] id = new object[] { Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId, null, null, null };
                    id[2] = DateTime.Now.ToString("HH:mm:ss.fff");
                    id[3] = description;
                    id[4] = empty;
                    Debug.WriteLine(string.Format("[{0},{1}]\t{2}\t{3}\t{4}", id));
                }
            }
            catch
            {

            }

        }
        public static void CollectDebugInfo(string msg)
        {
            try
            {
                if (AppContext.IsDebug)
                {
                    object[] id = new object[] { Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId, null, null };
                    id[2] = DateTime.Now.ToString("HH:mm:ss.fff");
                    id[3] = msg;
                    Debug.WriteLine(string.Format("[{0},{1}]\t{2}\t{3}", id));
                }
            }
            catch
            {
            }
        }

        public static void Log(string msg)
        {
            try
            {
                object[] id = new object[] { Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId, null, null };
                id[2] = DateTime.Now.ToString("HH:mm:ss.fff");
                id[3] = msg;
                Debug.WriteLine(string.Format("[{0},{1}]\t{2}\t{3}", id));
            }
            catch
            {
            }
        }

        public static void LogException(Exception ex)
        {
            try
            {
                DebugUtil.Log(string.Format("Exception: {0}", ex.Message));
                Debug.WriteLine(string.Format("{0}", ex.StackTrace));
            }
            catch
            {
            }
        }
    }
}
