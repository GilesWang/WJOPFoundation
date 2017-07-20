using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Log.SDK
{
    internal static class HostUtil
    {
        public static HostInfo GetHostInfo()
        {
            HostInfo hostInfo = new HostInfo()
            {
                ComputerName = AppContext.ComputerName,
                IPAddress = AppContext.IPv4,
                MacAddress = AppContext.MacAddress
            };
            return hostInfo;
        }
    }
}
