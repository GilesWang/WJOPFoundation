using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Location.Models
{
    public class BatchGetLocationRequest
    {
        public string AppKey { get; set; }
        public List<string> AppNameList { get; set; }
        public EnvInfo EnvInfo { get; set; }
    }
}
