using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Extensions;

namespace WJOP.Foundation.Common.Configuration
{
    public class MessageQueueConnection
    {
        public string ConnectionName { get; set; }
        public string HostUrl { get; set; }
        public string Password { get; set; }
        public int Prefetchcount { get; set; }
        public int RequestedHeartbeat { get; set; }
        public string Space { get; set; }
        public int Timeout { get; set; }
        public string UserName { get; set; }
        public string ToConnectionString()
        {
            object[] hostUrl = new object[] { this.HostUrl, this.Space, this.UserName, this.Password, this.RequestedHeartbeat, this.Prefetchcount, this.Timeout };
            return string.Format("host={0};virtualHost={1};username={2};password={3};requestedHeartbeat={4};prefetchcount={5};timeout={6}", hostUrl);
        }
        public static MessageQueueConnection Parse(string mqConnectionString)
        {
            MessageQueueConnection num = new MessageQueueConnection();
            if (!string.IsNullOrWhiteSpace(mqConnectionString))
            {
                char[] chrArray = new char[] { ';' };
                string[] strArrays = mqConnectionString.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> strs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                string[] strArrays1 = strArrays;
                for (int i = 0; i < strArrays1.Length; i++)
                {
                    string str = strArrays1[i];
                    chrArray = new char[] { '=' };
                    string[] strArrays2 = str.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                    if ((int)strArrays2.Length == 2)
                    {
                        strs.Add(strArrays2[0], strArrays2[1]);
                    }
                }
                string empty=string.Empty;
                if(strs.TryGetValue("host",out empty))
                {
                    num.HostUrl = empty;
                }

                if (!strs.TryGetValue("virtualHost", out empty))
                {
                    num.Space = "/";
                }
                else
                {
                    num.Space = empty;
                }

                if (strs.TryGetValue("username", out empty))
                {
                    num.UserName = empty;
                }
                if (strs.TryGetValue("password", out empty))
                {
                    num.Password = empty;
                }
                if (!strs.TryGetValue("requestedHeartbeat", out empty))
                {
                    num.RequestedHeartbeat = 10;
                }
                else
                {
                    num.RequestedHeartbeat = empty.ToInt(10);
                }
                if (!strs.TryGetValue("prefetchcount", out empty))
                {
                    num.Prefetchcount = 50;
                }
                else
                {
                    num.Prefetchcount = empty.ToInt(50);
                }
                if (!strs.TryGetValue("timeout", out empty))
                {
                    num.Timeout = 10;
                }
                else
                {
                    num.Timeout = empty.ToInt(10);
                }
                if (strs.TryGetValue("ConnectionName", out empty))
                {
                    num.ConnectionName = empty;
                }
            }
            return num;
        }
    }
}
