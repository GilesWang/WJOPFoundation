using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    public interface ICache
    {
        bool Exists(string key);
        ICacheResult<T> Get<T>(string key);
        IDictionary<string, ICacheResult<T>> Get<T>(IList<string> keys);
        ICacheResult<T> GetOrSet<T>(string key, Func<T> func, int expiredTime = 3600) where T:class;
        ICacheResult<TResult> GetOrSet<TParam, TResult>(string key, Func<TParam, TResult> func, TParam param, int expiredTime = 3600) where TResult : class;
        ICacheResult Remove(string key);
        IDictionary<string, ICacheResult> Remove(IList<string> keys);
        ICacheResult ResetKeys(string keyPrefix = "");
        ICacheResult<T> Set<T>(string key, T value, int expiredTime = 3600);
        IDictionary<string, ICacheResult<T>> Set<T>(IDictionary<string, T> items, int expiredTime = 3600);
    }
}
