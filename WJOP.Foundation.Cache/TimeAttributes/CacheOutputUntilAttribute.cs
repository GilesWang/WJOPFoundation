using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.TimeAttributes
{
    public class CacheOutputUntilAttribute:CacheOutputAttribute
    {
        public CacheOutputUntilAttribute(int year,int month,int day,int hour=0, int minute = 0, int second = 0)
        {

        }
    }
}
