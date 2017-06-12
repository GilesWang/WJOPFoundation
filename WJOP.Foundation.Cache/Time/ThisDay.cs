using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Time
{
    public class ThisDay : IModelQuery<DateTime, CacheTime>
    {
        private readonly int hour, minute, second;

        public ThisDay(int hour, int minute, int second)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
        public CacheTime Execute(DateTime model)
        {
            DateTimeOffset absoluteExpiration;
            CacheTime cacheTime = new CacheTime()
            {
                AbsoluteExpiration = new DateTime(model.Year, model.Month, model.Day, this.hour, this.minute, this.second)
            };
            CacheTime totalSeconds = cacheTime;
            if (totalSeconds.AbsoluteExpiration <= model)
            {
                absoluteExpiration = totalSeconds.AbsoluteExpiration;
                totalSeconds.AbsoluteExpiration = absoluteExpiration.AddDays(1);
            }
            absoluteExpiration = totalSeconds.AbsoluteExpiration;
            totalSeconds.ClientTimeSpan = absoluteExpiration.Subtract(model);
            totalSeconds.ClientTimeInSeconds = (int)totalSeconds.ClientTimeSpan.TotalSeconds;
            totalSeconds.ServerTimeInSeconds = totalSeconds.ClientTimeInSeconds;
            return totalSeconds;
        }
    }
}
