using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common
{
    public class ApiResult<T>
    {
        public string ExceptionMessage
        {
            get;
            set;
        }

        public bool IsServerError
        {
            get
            {
                return this.StatusCode != HttpStatusCode.OK;
            }
        }

        public T Result
        {
            get;
            set;
        }

        public HttpStatusCode StatusCode
        {
            get;
            set;
        }

        public ApiResult()
        {
            this.Result = default(T);
            this.ExceptionMessage = string.Empty;
        }
    }
}
