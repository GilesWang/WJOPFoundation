using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Redis
{
    internal class RedisConfiguration
    {
        public bool AbortOnConnectFail { get; set; }
        public bool AllowAdmin { get; set; }

    }
}
