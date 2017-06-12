using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Time
{
    public class CacheTime
    {
        public DateTimeOffset AbsoluteExpiration { get; set; }
        public int ClientTimeInSeconds { get; set; }
        public TimeSpan ClientTimeSpan { get; set; }
        public int ServerTimeInSeconds { get; set; }
    }
}
