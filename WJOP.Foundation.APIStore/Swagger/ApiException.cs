using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.Swagger
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }
        public dynamic ErrorContent { get; private set; }
        public ApiException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
        public ApiException(int errorCode, dynamic errorContent, string message) : base(message)
        {
            ErrorCode = errorCode;
            ErrorContent = errorContent;
        }
    }
}
