using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Time
{
    public class ShortTime
    {
        private readonly int serverTimeInSeconds, clientTimeInSeconds;
        public ShortTime(int serverTimeInSeconds, int clientTimeInSeconds)
        {
            this.serverTimeInSeconds = (serverTimeInSeconds < 0 ? 0 : serverTimeInSeconds);
            this.clientTimeInSeconds = (clientTimeInSeconds < 0 ? 0 : clientTimeInSeconds);
        }
        public CacheTime Execute(DateTime model)
        {
            CacheTime cacheTime = new CacheTime()
            {
                AbsoluteExpiration = model.AddSeconds((double)this.serverTimeInSeconds),
                ClientTimeSpan = TimeSpan.FromSeconds((double)this.clientTimeInSeconds),
                ServerTimeInSeconds = this.serverTimeInSeconds,
                ClientTimeInSeconds = this.clientTimeInSeconds
            };
            return cacheTime;
        }
    }
}
