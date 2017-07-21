using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public class MetricSerie
    {
        public IList<string> Columns
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IDictionary<string, string> Tags
        {
            get;
            set;
        }

        public IList<IList<object>> Values
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
