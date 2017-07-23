using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.SDK.Consul
{
    public class ConsulClientSDK
    {
        public ConsulClientSDK()
        {
        }

        public static async Task<ServiceEntry[]> AvaliableServices(string consulAgentUrl, string name, string tags = "")
        {
            bool flag = false;
            int j;
            string str;
            Func<string, string> func = null;
            List<ServiceEntry> serviceEntries = new List<ServiceEntry>();
            ConsulClient consulClient = new ConsulClient((ConsulClientConfiguration config) => config.Address = new Uri(consulAgentUrl));
            try
            {
                string str1 = tags;
                char[] chrArray = new char[] { ',' };
                string[] strArrays = str1.Split(chrArray);
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str2 = strArrays[i];
                    IHealthEndpoint health = consulClient.Health;
                    string str3 = name;
                    if (!string.IsNullOrEmpty(str2))
                    {
                        str = str2;
                    }
                    else
                    {
                        str = null;
                    }
                    CancellationToken cancellationToken = new CancellationToken();
                    ConfiguredTaskAwaitable<QueryResult<ServiceEntry[]>> configuredTaskAwaitable = health.Service(str3, str, true, cancellationToken).ConfigureAwait(false);
                    ServiceEntry[] response = await configuredTaskAwaitable.Response;
                    for (j = 0; j < (int)response.Length; j++)
                    {
                        ServiceEntry serviceEntry = response[j];
                        List<ServiceEntry> serviceEntries1 = serviceEntries;
                        if (!serviceEntries1.Any<ServiceEntry>((ServiceEntry service) => (service.Service.Address != serviceEntry.Service.Address ? false : service.Service.Port == serviceEntry.Service.Port)))
                        {
                            serviceEntries.Add(serviceEntry);
                        }
                    }
                    !flag;
                }
                !flag;
                string str4 = tags;
                chrArray = new char[] { ',' };
                string[] strArrays1 = str4.Split(chrArray);
                for (j = 0; j < (int)strArrays1.Length; j++)
                {
                    string str5 = strArrays1[j];
                    if (!string.IsNullOrEmpty(str5))
                    {
                        List<ServiceEntry> serviceEntries2 = serviceEntries;
                        List<ServiceEntry> list = serviceEntries2.Where<ServiceEntry>((ServiceEntry service) => {
                            string[] strArrays2 = service.Service.Tags;
                            if (func == null)
                            {
                                func = (string t) => t.Trim().ToLower();
                            }
                            return !strArrays2.Select<string, string>(func).Contains<string>(str5.ToLower());
                        }).ToList<ServiceEntry>();
                        foreach (ServiceEntry serviceEntry1 in list)
                        {
                            serviceEntries.Remove(serviceEntry1);
                        }
                    }
                }
                !flag;
            }
            finally
            {
                if (consulClient != null)
                {
                    ((IDisposable)consulClient).Dispose();
                }
            }
            return serviceEntries.ToArray();
        }

        public static async Task<bool> DeregisterService(string consulAgentUrl, string serviceId)
        {
            bool statusCode;
            ConsulClient consulClient = new ConsulClient((ConsulClientConfiguration config) => config.Address = new Uri(consulAgentUrl));
            try
            {
                IAgentEndpoint agent = consulClient.Agent;
                string str = serviceId;
                CancellationToken cancellationToken = new CancellationToken();
                ConfiguredTaskAwaitable<WriteResult> configuredTaskAwaitable = agent.ServiceDeregister(str, cancellationToken).ConfigureAwait(false);
                statusCode = await configuredTaskAwaitable.StatusCode == HttpStatusCode.OK;
            }
            finally
            {
                if (consulClient != null)
                {
                    ((IDisposable)consulClient).Dispose();
                }
            }
            return statusCode;
        }

        public static async Task<bool> HeartBeat(string consulAgentUrl, string checkId)
        {
            bool statusCode;
            ConsulClient consulClient = new ConsulClient((ConsulClientConfiguration config) => config.Address = new Uri(consulAgentUrl));
            try
            {
                IAgentEndpoint agent = consulClient.Agent;
                string str = checkId;
                TTLStatus pass = TTLStatus.Pass;
                CancellationToken cancellationToken = new CancellationToken();
                ConfiguredTaskAwaitable<WriteResult> configuredTaskAwaitable = agent.UpdateTTL(str, null, pass, cancellationToken).ConfigureAwait(false);
                statusCode = await configuredTaskAwaitable.StatusCode == HttpStatusCode.OK;
            }
            finally
            {
                if (consulClient != null)
                {
                    ((IDisposable)consulClient).Dispose();
                }
            }
            return statusCode;
        }

        public static async Task<bool> RegsterService(string consulAgentUrl, string name, string tags, string address, int port, int inerval, string httpcheck, string tcpCheck)
        {
            bool statusCode;
            TimeSpan? nullable;
            string str;
            string str1;
            TimeSpan? nullable1;
            TimeSpan? nullable2;
            ConsulClient consulClient = new ConsulClient((ConsulClientConfiguration config) => config.Address = new Uri(consulAgentUrl));
            try
            {
                IAgentEndpoint agent = consulClient.Agent;
                AgentServiceRegistration agentServiceRegistration = new AgentServiceRegistration()
                {
                    Address = address,
                    ID = name,
                    Name = name,
                    Port = port
                };
                AgentServiceRegistration agentServiceRegistration1 = agentServiceRegistration;
                string str2 = tags;
                char[] chrArray = new char[] { ',' };
                agentServiceRegistration1.Tags = str2.Split(chrArray);
                AgentServiceRegistration agentServiceRegistration2 = agentServiceRegistration;
                AgentServiceCheck agentServiceCheck = new AgentServiceCheck();
                AgentServiceCheck agentServiceCheck1 = agentServiceCheck;
                if (!string.IsNullOrEmpty(httpcheck))
                {
                    str = httpcheck;
                }
                else
                {
                    str = null;
                }
                agentServiceCheck1.HTTP = str;
                AgentServiceCheck agentServiceCheck2 = agentServiceCheck;
                if (!string.IsNullOrEmpty(tcpCheck))
                {
                    str1 = tcpCheck;
                }
                else
                {
                    str1 = null;
                }
                agentServiceCheck2.TCP = str1;
                AgentServiceCheck agentServiceCheck3 = agentServiceCheck;
                if (!string.IsNullOrEmpty(httpcheck) || !string.IsNullOrEmpty(tcpCheck))
                {
                    nullable1 = new TimeSpan?(TimeSpan.FromSeconds((double)inerval));
                }
                else
                {
                    nullable = null;
                    nullable1 = nullable;
                }
                agentServiceCheck3.Interval = nullable1;
                AgentServiceCheck agentServiceCheck4 = agentServiceCheck;
                if (!string.IsNullOrEmpty(httpcheck) || !string.IsNullOrEmpty(tcpCheck))
                {
                    nullable = null;
                    nullable2 = nullable;
                }
                else
                {
                    nullable2 = new TimeSpan?(TimeSpan.FromSeconds((double)(2 * inerval)));
                }
                agentServiceCheck4.TTL = nullable2;
                agentServiceCheck.Status = CheckStatus.Passing;
                agentServiceRegistration2.Check = agentServiceCheck;
                AgentServiceRegistration agentServiceRegistration3 = agentServiceRegistration;
                CancellationToken cancellationToken = new CancellationToken();
                ConfiguredTaskAwaitable<WriteResult> configuredTaskAwaitable = agent.ServiceRegister(agentServiceRegistration3, cancellationToken).ConfigureAwait(false);
                statusCode = await configuredTaskAwaitable.StatusCode == HttpStatusCode.OK;
            }
            finally
            {
                if (consulClient != null)
                {
                    ((IDisposable)consulClient).Dispose();
                }
            }
            return statusCode;
        }
    }
}
