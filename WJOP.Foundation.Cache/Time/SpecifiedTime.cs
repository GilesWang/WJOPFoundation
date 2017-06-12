using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.Time
{
    public class SpecifiedTime : IModelQuery<DateTime, CacheTime>
    {
        private readonly int year, month, day, hour, minute, second;
        public SpecifiedTime(int year,int month,int day,int hour,int minute,int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
        public CacheTime Execute(DateTime model)
        {
            CacheTime cacheTime = new CacheTime() {
                AbsoluteExpiration=new DateTime(year,month, day, hour, minute, second)
            };
            CacheTime totalSeconds = cacheTime;
            totalSeconds.ClientTimeSpan = totalSeconds.AbsoluteExpiration.Subtract(model);
            totalSeconds.ClientTimeInSeconds = (int)totalSeconds.ClientTimeSpan.TotalSeconds;
            totalSeconds.ServerTimeInSeconds = totalSeconds.ClientTimeInSeconds;
            return totalSeconds;
        }
    }
}
