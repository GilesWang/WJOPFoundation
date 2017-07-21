using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public class KapacitorAlert
    {
        public KapacitorAlertData Data
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public long Duration
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string Level
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Time
        {
            get;
            set;
        }

        public string Version
        {
            get
            {
                return MMConstant.SDKVersion;
            }
        }
    }
}
