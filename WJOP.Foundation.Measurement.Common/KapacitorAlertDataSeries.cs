using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public class KapacitorAlertDataSeries
    {
        public List<string> Columns
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<List<string>> Values
        {
            get;
            set;
        }
    }
}
