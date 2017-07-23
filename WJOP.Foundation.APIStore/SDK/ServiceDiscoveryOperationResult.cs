using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.SDK
{
    public class ServiceDiscoveryOperationResult
    {
        public string ErrorMessage
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string OperationMethod
        {
            get;
            set;
        }
    }
}
