using Couchbase;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    public class CacheResult : ICacheResult
    {
        public Exception Exception{get;set;}

        public string Message { get; set; }

        public Status Status { get; set; }

        public bool Success { get; set; }

        public CacheResult()
        {
            this.Success = true;
            this.Status = Status.Success;
            this.Exception = null;
            this.Message = string.Empty;
        }

        public CacheResult(object obj)
        {
            if (obj != null)
            {
                this.Success = true;
                this.Status = Status.Success;
                this.Exception = null;
                this.Message = string.Empty;
            }
            else
            {
                this.Status = Status.KeyNotFound;
                this.Success = false;
                this.Message = "Not found";
            }

        }

        public CacheResult(IOperationResult result)
        {
            Status = (Status)result.Status;
            Exception = result.Exception;
            Message = result.Message;
            Success = result.Success;
        }

        public CacheResult(IQueryResult<dynamic> result)
        {
            this.Status = (Status)result.Status;
            this.Exception = result.Exception;
            if ((result.Errors == null ? true : result.Errors.Count <= 0))
            {
                this.Message = result.Message;
            }
            else
            {
                this.Message = result.Errors[0].Message;
            }
            this.Success = result.Success;
        }
    }

    public class CacheResult<T> : CacheResult, ICacheResult<T>, ICacheResult
    {
        public T Value { get; set; }

        public CacheResult()
        {
            this.Value = default(T);
        }
        public CacheResult(object obj)
        {
            if (obj == null)
            {
                base.Status = Status.KeyNotFound;
                base.Success = false;
                base.Message = "Not found";
            }
            else
            {
                this.Value = (T)obj;
            }
        }        

        public CacheResult(IOperationResult<T> result)
        {
            base.Status = (Status)result.Status;
            base.Exception = result.Exception;
            base.Message = (base.Status == Status.Success ? string.Empty : result.Message);
            base.Success = result.Success;
            this.Value = result.Value;
        }
    }
}
