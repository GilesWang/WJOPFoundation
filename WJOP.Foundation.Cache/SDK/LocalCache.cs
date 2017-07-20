using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    public class LocalCache : ICache
    {
        private readonly static MemoryCache Cache;

        public static LocalCache Instance
        {
            get { return InnerLocalCache.instance; }
        }

        static LocalCache()
        {
            Cache = MemoryCache.Default;
        }

        private class InnerLocalCache
        {
            internal readonly static LocalCache instance;
            static InnerLocalCache()
            {
                instance = new LocalCache();
            }
        }
        public bool Exists(string key)
        {
            return Cache.Contains(KeyGen.AttachPrefixToKey(key), null);
        }

        public IDictionary<string, ICacheResult<T>> Get<T>(IList<string> keys)
        {
            object obj;
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            keys = KeyGen.AttachPrefixToKey(keys);
            IDictionary<string, object> values = LocalCache.Cache.GetValues(keys, null);
            foreach (string key in keys)
            {
                obj = (values == null || !values.Keys.Contains(key) ? null : values[key]);
                strs.Add(KeyGen.DetachPrefixFromKey(key), new CacheResult<T>(obj));
            }
            return strs;
        }

        public ICacheResult<T> Get<T>(string key)
        {
            object obj = Cache.Get(KeyGen.AttachPrefixToKey(key), null);
            return new CacheResult<T>(obj);
        }

        public ICacheResult<T> GetOrSet<T>(string key, Func<T> func, int expiredTime = 3600) where T : class
        {
            ICacheResult<T> cacheResult = Get<T>(key);
            expiredTime = (expiredTime < 0 ? 3600 : expiredTime);
            if (cacheResult.Value == null || !cacheResult.Success ? true : cacheResult.Status != Status.Success)
            {
                try
                {
                    T t = func();
                    Set<T>(key, t, expiredTime);
                    cacheResult.Value = t;
                    cacheResult.Success = true;
                }
                catch (Exception ex)
                {
                    cacheResult.Exception = ex;
                    cacheResult.Success = false;
                    cacheResult.Value = default(T);
                }
            }
            return cacheResult;
        }

        public ICacheResult<TResult> GetOrSet<TParam, TResult>(string key, Func<TParam, TResult> func, TParam param, int expiredTime = 3600) where TResult : class
        {
            ICacheResult<TResult> cacheResult = this.Get<TResult>(key);
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            if ((cacheResult.Value == null || !cacheResult.Success ? true : cacheResult.Status != Status.Success))
            {
                try
                {
                    TResult tResult = func(param);
                    this.Set<TResult>(key, tResult, expiredTime);
                    cacheResult.Value = tResult;
                    cacheResult.Success = true;
                }
                catch (Exception exception)
                {
                    cacheResult.Exception = exception;
                    cacheResult.Success = false;
                    cacheResult.Value = default(TResult);
                }
            }
            return cacheResult;
        }

        public IDictionary<string, ICacheResult> Remove(IList<string> keys)
        {
            Dictionary<string, ICacheResult> strs = new Dictionary<string, ICacheResult>();
            lock (Cache)
            {
                foreach (string key in keys)
                {
                    strs.Add(key, Remove(key));
                }
            }
            return strs;
        }

        public ICacheResult Remove(string key)
        {
            ICacheResult cacheResult;
            lock (Cache)
            {
                object obj = Cache.Remove(KeyGen.AttachPrefixToKey(key), null);
                cacheResult = new CacheResult(obj);
            }
            return cacheResult;
        }

        public ICacheResult ResetKeys(string keyPrefix = "")
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ICacheResult<T>> Set<T>(IDictionary<string, T> items, int expiredTime = 3600)
        {
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            foreach (KeyValuePair<string, T> item in items)
            {
                lock (Cache)
                {
                    Set(item.Key, item.Value, expiredTime);
                }
                strs.Add(item.Key, new CacheResult<T>());
            }
            return strs;
        }

        public ICacheResult<T> Set<T>(string key, T value, int expiredTime = 3600)
        {
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds((double)expiredTime)
            };
            lock (Cache)
            {
                Cache.Set(KeyGen.AttachPrefixToKey(key), cacheItemPolicy, null);
            }
            return new CacheResult<T>();
        }
    }
}
