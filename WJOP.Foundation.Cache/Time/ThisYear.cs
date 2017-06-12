using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Time
{
    public class ThisYear : IModelQuery<DateTime, CacheTime>
    {
        private readonly int month,day,hour,minute,second;

        public ThisYear(int month, int day, int hour, int minute, int second)
        {
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public CacheTime Execute(DateTime model)
        {
            DateTimeOffset absoluteExpiration;
            CacheTime cacheTime = new CacheTime()
            {
                AbsoluteExpiration = new DateTime(model.Year, this.month, this.day, this.hour, this.minute, this.second)
            };
            CacheTime totalSeconds = cacheTime;
            if (totalSeconds.AbsoluteExpiration <= model)
            {
                absoluteExpiration = totalSeconds.AbsoluteExpiration;
                totalSeconds.AbsoluteExpiration = absoluteExpiration.AddYears(1);
            }
            absoluteExpiration = totalSeconds.AbsoluteExpiration;
            totalSeconds.ClientTimeSpan = absoluteExpiration.Subtract(model);
            totalSeconds.ClientTimeInSeconds = (int)totalSeconds.ClientTimeSpan.TotalSeconds;
            totalSeconds.ServerTimeInSeconds = totalSeconds.ClientTimeInSeconds;
            return totalSeconds;
        }
    }
}
