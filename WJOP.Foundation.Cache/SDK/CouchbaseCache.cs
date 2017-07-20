using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Cache.SDK
{
    public class CouchbaseCache : ICache
    {
        public static CouchbaseCache Instance
        {
            get
            {
                return InnerCouchbaseCache.instance;
            }
        }

        private CouchbaseCache()
        {
        }

        private class InnerCouchbaseCache
        {
            internal readonly static CouchbaseCache instance;

            static InnerCouchbaseCache()
            {
                instance = new CouchbaseCache();
            }

            public InnerCouchbaseCache()
            {
            }
        }

        public bool Exists(string key)
        {
            bool flag = false;
            try
            {
                SafeBucket instance = SafeBucket.Instance;
                try
                {
                    flag = instance.Bucket.Exists(KeyGen.AttachPrefixToKey(key));
                }
                finally
                {
                    if (instance != null)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
                DebugUtil.CollectDebugInfo(string.Format("Exists('{0}'), IsSuccess:{1}", key, flag));
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                DebugUtil.CollectDebugInfo(string.Format("Exists('{0}'), Exception:{1}", key, exception.Message));
            }
            return flag;
        }

        public IDictionary<string, ICacheResult<T>> Get<T>(IList<string> keys)
        {
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            SafeBucket instance = SafeBucket.Instance;
            try
            {
                foreach (KeyValuePair<string, IOperationResult<T>> keyValuePair in instance.Bucket.Get<T>(KeyGen.AttachPrefixToKey(keys)))
                {
                    strs.Add(KeyGen.DetachPrefixFromKey(keyValuePair.Key), new CacheResult<T>(keyValuePair.Value));
                }
            }
            finally
            {
                if (instance != null)
                {
                    ((IDisposable)instance).Dispose();
                }
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
                SafeBucket instance = SafeBucket.Instance;
                try
                {
                    IOperationResult<T> operationResult = instance.Bucket.Get<T>(KeyGen.AttachPrefixToKey(key));
                    object[] objArray = new object[] { key, operationResult.Status, null, null };
                    objArray[2] = (operationResult.Value == null ? "null" : operationResult.Value.ToString());
                    objArray[3] = (operationResult.Exception == null ? "null" : operationResult.Exception.Message);
                    DebugUtil.CollectDebugInfo(string.Format("Get<T>('{0}'), Status:{1}, Value:{2}, Exception:{3}", objArray));
                    cacheResult1 = new CacheResult<T>(operationResult);
                }
                finally
                {
                    if (instance != null)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                cacheResult1.Exception = exception;
                DebugUtil.CollectDebugInfo(string.Format("Get<T>('{0}'), Exception:{1}", key, exception.Message));
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
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    cacheResult.Exception = exception;
                    cacheResult.Success = false;
                    cacheResult.Value = default(T);
                    cacheResult.Status = Status.None;
                    DebugUtil.CollectDebugInfo(string.Format("GetOrSet<T>('{0}'), Exception:{1}", key, exception.Message));
                }
            }
            return cacheResult;
        }

        public ICacheResult<TResult> GetOrSet<TParam, TResult>(string key, Func<TParam, TResult> func, TParam param, int expiredTime = 3600) where TResult:class
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
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    cacheResult.Exception = exception;
                    cacheResult.Success = false;
                    cacheResult.Value = default(TResult);
                    cacheResult.Status = Status.None;
                    DebugUtil.CollectDebugInfo(string.Format("GetOrSet<TPara, TResult>('{0}'), Exception:{1}", key, exception.Message));
                }
            }
            return cacheResult;
        }

        public IDictionary<string, ICacheResult> Remove(IList<string> keys)
        {
            Dictionary<string, ICacheResult> strs = new Dictionary<string, ICacheResult>();
            SafeBucket instance = SafeBucket.Instance;
            try
            {
                foreach (KeyValuePair<string, IOperationResult> keyValuePair in instance.Bucket.Remove(KeyGen.AttachPrefixToKey(keys)))
                {
                    strs.Add(KeyGen.DetachPrefixFromKey(keyValuePair.Key), new CacheResult(keyValuePair.Value));
                }
            }
            finally
            {
                if (instance != null)
                {
                    ((IDisposable)instance).Dispose();
                }
            }
            return strs;
        }

        public ICacheResult Remove(string key)
        {
            CacheResult cacheResult = new CacheResult()
            {
                Success = false,
                Status = Status.None
            };
            CacheResult cacheResult1 = cacheResult;
            try
            {
                SafeBucket instance = SafeBucket.Instance;
                try
                {
                    IOperationResult operationResult = instance.Bucket.Remove(KeyGen.AttachPrefixToKey(key));
                    DebugUtil.CollectDebugInfo(string.Format("Remove('{0}'), Status:{1}, Exception:{2}", key, operationResult.Status, (operationResult.Exception == null ? "null" : operationResult.Exception.Message)));
                    cacheResult1 = new CacheResult(operationResult);
                }
                finally
                {
                    if (instance != null)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                cacheResult1.Exception = exception;
                DebugUtil.CollectDebugInfo(string.Format("Remove('{0}'), Exception:{1}", key, exception.Message));
            }
            return cacheResult1;
        }

        public ICacheResult ResetKeys(string keyPrefix = "")
        {
            CacheResult cacheResult = new CacheResult()
            {
                Success = false,
                Status = Status.None
            };
            CacheResult cacheResult1 = cacheResult;
            try
            {
                SafeBucket instance = SafeBucket.Instance;
                try
                {
                    IBucket bucket = instance.Bucket;
                    object[] appKey = new object[] { AppContext.AppKey, keyPrefix, SafeBucket.Instance.Bucket.Name, KeyGen.AttachPrefixToKey(keyPrefix) };
                    bucket.Query<object>(string.Format("CREATE INDEX {0}_{1}_KeyPrefix_Index ON {2}(position((meta().id), \"{3}\")) USING GSI", appKey));
                    IQueryResult<object> queryResult = instance.Bucket.Query<object>(string.Format("DELETE FROM {0} WHERE POSITION(META().id, \"{1}\") = 0", SafeBucket.Instance.Bucket.Name, KeyGen.AttachPrefixToKey(keyPrefix)));
                    instance.Bucket.Query<object>(string.Format("DROP INDEX {0}.{1}_{2}_KeyPrefix_Index USING GSI", SafeBucket.Instance.Bucket.Name, AppContext.AppKey, keyPrefix));
                    DebugUtil.CollectDebugInfo(string.Format("Reset keys by prefix('{0}'), Status:{1}, Exception:{2}", keyPrefix, queryResult.Status, (queryResult.Exception == null ? "null" : queryResult.Exception.Message)));
                    cacheResult1 = new CacheResult(queryResult);
                }
                finally
                {
                    if (instance != null)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                cacheResult1.Exception = exception;
                DebugUtil.CollectDebugInfo(string.Format("Reset keys by prefix('{0}'), Exception:{1}", keyPrefix, exception.Message));
            }
            return cacheResult1;
        }

        public IDictionary<string, ICacheResult<T>> Set<T>(IDictionary<string, T> items, int expiredTime = 3600)
        {
            Dictionary<string, ICacheResult<T>> strs = new Dictionary<string, ICacheResult<T>>();
            expiredTime = (expiredTime <= 0 ? 3600 : expiredTime);
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, expiredTime);
            SafeBucket instance = SafeBucket.Instance;
            try
            {
                foreach (KeyValuePair<string, T> item in items)
                {
                    IOperationResult<T> operationResult = instance.Bucket.Upsert<T>(KeyGen.AttachPrefixToKey(item.Key), item.Value, timeSpan);
                    DebugUtil.CollectDebugInfo(string.Format("Set<T>('{0}'), Status:{1}, Exception:{2}", item.Key, operationResult.Status, (operationResult.Exception == null ? "null" : operationResult.Exception.Message)));
                    strs.Add(item.Key, new CacheResult<T>(operationResult));
                }
            }
            finally
            {
                if (instance != null)
                {
                    ((IDisposable)instance).Dispose();
                }
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
                SafeBucket instance = SafeBucket.Instance;
                try
                {
                    IOperationResult<T> operationResult = instance.Bucket.Upsert<T>(KeyGen.AttachPrefixToKey(key), value, new TimeSpan(0, 0, 0, expiredTime));
                    DebugUtil.CollectDebugInfo(string.Format("Set<T>('{0}'), Status:{1}, Exception:{2}", key, operationResult.Status, (operationResult.Exception == null ? "null" : operationResult.Exception.Message)));
                    cacheResult1 = new CacheResult<T>(operationResult);
                }
                finally
                {
                    if (instance != null)
                    {
                        ((IDisposable)instance).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                cacheResult1.Exception = exception;
                DebugUtil.CollectDebugInfo(string.Format("Set<T>('{0}'), Exception:{1}", key, exception.Message));
            }
            return cacheResult1;
        }
    }
}
