using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Measurement.Common
{
    public class Command
    {
        public Dictionary<string, string> Parameters { get; set; }
        public CommandType Type { get; set; }
        public string Version { get { return MMConstant.SDKVersion; } }
    }
}
