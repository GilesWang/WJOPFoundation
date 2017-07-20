using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    public interface ICacheResult
    {
        Exception Exception { get; set; }
        string Message { get; set; }
        bool Success { get; set; }
        Status Status { get; set; }
    }

    public interface ICacheResult<T> : ICacheResult
    {
        T Value { get; set; }
    }
}
