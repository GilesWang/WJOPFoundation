using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Utility
{
    public static class ParamterUtil
    {
        public static void CheckEmptyString(string parameterName, string parameterObj)
        {
            if (string.IsNullOrEmpty(parameterObj))
            {
                throw new ArgumentNullException("String parameter '{0}' cannot be null or empty.", parameterName);
            }
        }

        public static void CheckNull(string parameterName, object parameterObj)
        {
            if (parameterObj == null)
            {
                throw new ArgumentNullException("Parameter '{0}' cannot be null.", parameterName);
            }
        }
    }
}
