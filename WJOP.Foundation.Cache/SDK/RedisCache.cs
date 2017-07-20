using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;
using WJOP.Foundation.Location;

namespace WJOP.Foundation.Cache.SDK
{
    public class RedisCache : ICache
    {
        private readonly IDatabase db;

        private readonly ConnectionMultiplexer redis;

        public static RedisCache Instance
        {
            get
            {
                return InnerRedisCache.instance;
            }
        }

        private RedisCache()
        {
            string cacheConfigString = AppContext.CacheConfigString;
            string appUri = LocationHelper.Instance.GetAppUri(AppNameEnum.RedisSvc);
            cacheConfigString = (string.IsNullOrEmpty(cacheConfigString) ? appUri.Replace("http://", string.Empty).Trim(new char[] { '/' }) : cacheConfigString);
            ConfigurationOptions configurationOption = ConfigurationOptions.Parse(cacheConfigString);
            if (configurationOption.EndPoints.Count == 0)
            {
                configurationOption.EndPoints.Add(appUri);
            }
            this.redis = ConnectionMultiplexer.Connect(configurationOption, null);
            this.db = this.redis.GetDatabase(-1, null);
            DebugUtil.CollectDebugInfo(JsonSerializerUtil.Serialize<ConfigurationOptions>(configurationOption));
        }

        private class InnerRedisCache
        {
            internal readonly static RedisCache instance;

            static InnerRedisCache()
            {
                RedisCache.InnerRedisCache.instance = new RedisCache();
            }

            public InnerRedisCache()
            {
            }
        }

        public bool Exists(string key)
        {
            return this.db.KeyExists(KeyGen.AttachPrefixToKey(key), CommandFlags.None);
        }

        public IDictionary<string, ICacheResult<T>> Get<T>(IList<string> keys)
        {
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            foreach (string key in keys)
            {
                strs.Add(key, this.Get<T>(key));
            }
            return strs;
        }

        public ICacheResult<T> Get<T>(string key)
        {
            CacheResult<T> cacheResult = new CacheResult<T>()
            {
                Success = false,
                Status = Status.None
            };
            CacheResult<T> cacheResult1 = cacheResult;
            try
            {
                RedisValue redisValue = this.db.StringGet(KeyGen.AttachPrefixToKey(key), 0);
                cacheResult1.Value = JsonSerializerUtil.Deserialize<T>(redisValue);
                cacheResult1.Success = true;
                cacheResult1.Status = Status.Success;
            }
            catch (Exception exception)
            {
                cacheResult1.Exception = exception;
            }
            return cacheResult1;
        }

        public ICacheResult<T> GetOrSet<T>(string key, Func<T> func, int expiredTime = 3600) where T : class
        {
            ICacheResult<T> cacheResult = this.Get<T>(key);
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            if ((cacheResult.Status != Status.Success ? true : !cacheResult.Success))
            {
                try
                {
                    T t = func();
                    this.Set<T>(key, t, expiredTime);
                    cacheResult.Value = t;
                    cacheResult.Success = true;
                    cacheResult.Status = Status.Success;
                }
                catch (Exception exception)
                {
                    cacheResult.Exception = exception;
                    cacheResult.Success = false;
                    cacheResult.Value = default(T);
                    cacheResult.Status = Status.None;
                }
            }
            return cacheResult;
        }

        public ICacheResult<TResult> GetOrSet<TParam, TResult>(string key, Func<TParam, TResult> func, TParam param, int expiredTime = 3600) where TResult : class
        {
            ICacheResult<TResult> cacheResult = this.Get<TResult>(key);
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            if ((cacheResult.Status != Status.Success ? true : !cacheResult.Success))
            {
                try
                {
                    TResult tResult = func(param);
                    this.Set<TResult>(key, tResult, expiredTime);
                    cacheResult.Value = tResult;
                    cacheResult.Success = true;
                    cacheResult.Status = Status.Success;
                }
                catch (Exception exception)
                {
                    cacheResult.Exception = exception;
                    cacheResult.Success = false;
                    cacheResult.Value = default(TResult);
                    cacheResult.Status = Status.None;
                }
            }
            return cacheResult;
        }

        public IDictionary<string, ICacheResult> Remove(IList<string> keys)
        {
            Dictionary<string, ICacheResult> strs = new Dictionary<string, ICacheResult>();
            foreach (string key in keys)
            {
                strs.Add(key, this.Remove(key));
            }
            return strs;
        }

        public ICacheResult Remove(string key)
        {
            ICacheResult cacheResult = new CacheResult()
            {
                Success = false,
                Status = Status.None
            };
            ICacheResult cacheResult1 = cacheResult;
            try
            {
                cacheResult1.Success = this.db.KeyDelete(KeyGen.AttachPrefixToKey(key), 0);
                cacheResult1.Status = (cacheResult1.Success ? Status.Success : Status.None);
            }
            catch (Exception exception)
            {
                cacheResult1.Exception = exception;
            }
            return cacheResult1;
        }

        public ICacheResult ResetKeys(string keyPrefix = "")
        {
            this.RemoveKeys(keyPrefix);
            return new CacheResult()
            {
                Success = true,
                Status = Status.Success
            };
        }
        private IDictionary<string, ICacheResult> RemoveKeys(string keyPrefix)
        {
            Dictionary<string, ICacheResult> strs = new Dictionary<string, ICacheResult>();
            EndPoint[] endPoints = this.redis.GetEndPoints(false);
            for (int i = 0; i < (int)endPoints.Length; i++)
            {
                EndPoint endPoint = endPoints[i];
                IServer server = this.redis.GetServer(endPoint, null);
                IEnumerable<RedisKey> redisKeys = server.Keys(this.db.Database, string.Concat(KeyGen.AttachPrefixToKey(keyPrefix), "*"), 10, (long)0, 0, 0);
                foreach (RedisKey redisKey in redisKeys)
                {
                    string str = KeyGen.DetachPrefixFromKey(redisKey);
                    strs.Add(str, this.Remove(str));
                }
            }
            return strs;
        }

        public IDictionary<string, ICacheResult<T>> Set<T>(IDictionary<string, T> items, int expiredTime = 3600)
        {
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            foreach (KeyValuePair<string, T> item in items)
            {
                strs.Add(item.Key, this.Set<T>(item.Key, item.Value, expiredTime));
            }
            return strs;
        }

        public ICacheResult<T> Set<T>(string key, T value, int expiredTime = 3600)
        {
            CacheResult<T> cacheResult = new CacheResult<T>()
            {
                Success = false,
                Status = Status.None
            };
            CacheResult<T> cacheResult1 = cacheResult;
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            try
            {
                string str = JsonSerializerUtil.Serialize<T>(value);
                cacheResult1.Success = this.db.StringSet(KeyGen.AttachPrefixToKey(key), str, new TimeSpan?(new TimeSpan(0, 0, expiredTime)), 0, 0);
                cacheResult1.Status = Status.Success;
            }
            catch (Exception exception)
            {
                cacheResult1.Exception = exception;
            }
            return cacheResult1;
        }
    }
}
