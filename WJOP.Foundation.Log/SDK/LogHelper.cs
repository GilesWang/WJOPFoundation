using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Log.SDK
{
    public sealed class LogHelper
    {
        public static ILog GetLogger(string logProvider)
        {
            ILog instance;
            if (!AppContext.LogEnabled)
            {
                instance = NullLogger.Instance;
            }
            else
            {
                instance = new Logger(logProvider);
            }
            return instance;
        }
    }
}
