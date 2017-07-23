using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.Swagger
{
    public class ApiResponse<T>
    {
        public T Data { get; private set; }
        public IDictionary<string,string> Headers { get; private set; }
        public int StatusCode { get; private set; }
        public ApiResponse(int statusCode,IDictionary<string,string> headers,T data)
        {
            StatusCode = statusCode;
            Headers = headers;
            Data = data;
        }
    }
}
