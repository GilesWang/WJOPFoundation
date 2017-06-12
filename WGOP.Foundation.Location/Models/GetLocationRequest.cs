using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Location.Models
{
    public class GetLocationRequest
    {
        public string AppKey { get; set; }
        public string AppName { get; set; }

        public EnvInfo EnvInfo { get; set; }
    }
}
