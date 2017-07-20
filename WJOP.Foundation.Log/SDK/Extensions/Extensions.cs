using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Log.Common;

namespace WJOP.Foundation.Log.SDK.Extensions
{
    public static class Extensions
    {
        internal static string DumpException(Exception ex)
        {
            string str;
            string empty = string.Empty;
            if (ex != null)
            {
                object[] message = new object[] { ex.Message, Environment.NewLine, ex.StackTrace, Extensions.DumpException(ex.InnerException) };
                empty = string.Format("{0}{1}{2}{1}{3}", message);
                str = empty;
            }
            else
            {
                str = empty;
            }
            return str;
        }

        internal static string ToLogText(this Exception ex)
        {
            return Extensions.DumpException(ex);
        }

        internal static string ToLogText(this LogContent logContent)
        {
            string str;
            str = (logContent != null ? logContent.ToJsonString() : string.Empty);
            return str;
        }
    }
}
