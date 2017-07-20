using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Cache.SDK
{
    public class CacheProviderFactory
    {  
        public static ICache Create(CacheTypeEnum cacheType)
        {
            ICache instance;
            switch (cacheType)
            {
                case CacheTypeEnum.Local:
                    {
                        instance = LocalCache.Instance;
                        break;
                    }
                case CacheTypeEnum.Couchbase:
                    {
                        instance = CouchbaseCache.Instance;
                        break;
                    }
                case CacheTypeEnum.Redis:
                    {
                        instance = RedisCache.Instance;
                        break;
                    }
                default:
                    {
                        instance = CouchbaseCache.Instance;
                        break;
                    }
            }
            return instance;
        }
        public static ICache CreateInstance()
        {
            return Create(GetCacheTypeFromConfig());
        }
        private static CacheTypeEnum GetCacheTypeFromConfig()
        {
            CacheTypeEnum cacheTypeEnum = CacheTypeEnum.Couchbase;
            if (!string.IsNullOrWhiteSpace(AppContext.CacheType))
            {
                string str = AppContext.CacheType.ToLower().Trim();
                if (str != null)
                {
                    if (str == "local")
                    {
                        cacheTypeEnum = CacheTypeEnum.Local;
                    }
                    else if (str == "redis")
                    {
                        cacheTypeEnum = CacheTypeEnum.Redis;
                    }
                    else
                    {
                        cacheTypeEnum = CacheTypeEnum.Couchbase;
                    }
                }
            }
            return cacheTypeEnum;
        }
    }
}
