using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Cache.SDK
{
    public static class CacheAspectEx
    {
        public static Aspect Cache<TReturnType>(this Aspect aspect, ICache cacheResolver, string key, int expiredTimes = 3600)
        {
            Func<TReturnType, TReturnType> func = null;
            Aspect aspect1 = aspect.Combine((Action work) =>
            {
                Aspect aspect2 = aspect;
                ICache cache = cacheResolver;
                string str = key;
                Action action = work;
                if (func == null)
                {
                    func = (TReturnType cached) => cached;
                }
                CacheAspectEx.Cache<TReturnType>(aspect2, cache, str, action, func, expiredTimes);
            });
            return aspect1;
        }

        public static void Cache<TReturnType>(this Aspect aspect, ICache cacheResolver, string key, Action work, Func<TReturnType, TReturnType> foundInCache, int expiredTime = 3600)
        {
            ICacheResult<TReturnType> cacheResult = cacheResolver.Get<TReturnType>(key);
            if (cacheResult.Value != null)
            {
                TReturnType tReturnType = foundInCache(cacheResult.Value);
                if (tReturnType != null)
                {
                    aspect.WorkDelegate = new Func<TReturnType>(() => tReturnType);
                }
                else
                {
                    CacheAspectEx.GetResult<TReturnType>(aspect, cacheResolver, key, expiredTime);
                }
            }
            else
            {
                CacheAspectEx.GetResult<TReturnType>(aspect, cacheResolver, key, expiredTime);
            }
            work();
        }

        public static void GetResult<TReturnType>(Aspect aspect, ICache cacheResolver, string key, int expiredTime)
        {
            TReturnType workDelegate = (aspect.WorkDelegate as Func<TReturnType>)();
            cacheResolver.Set<TReturnType>(key, workDelegate, expiredTime);
            aspect.WorkDelegate = new Func<TReturnType>(() => workDelegate);
        }
    }
}
