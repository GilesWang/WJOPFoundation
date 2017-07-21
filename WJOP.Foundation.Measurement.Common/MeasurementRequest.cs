using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public sealed class MeasurementRequest
    {
        public string DBName
        {
            get;
            set;
        }

        public List<MetricPoint> MetricPoints
        {
            get;
        }

        public string RetentionPolicy
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
