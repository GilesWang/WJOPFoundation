using Consul;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Location;

namespace WJOP.Foundation.APIStore.SDK.Consul
{
    public class ConsulHost
    {
        private const string ServiceCacheKeyFormat = "{0}-{1}";

        private const string ServiceCacheBufferValue = "ServiceUrlCacheBuffer";

        private ConcurrentDictionary<string, string> autoRegisteredServiceCache = new ConcurrentDictionary<string, string>();

        private MemoryCache serviceCache = new MemoryCache("ServiceNodeServiceDiscoveryCache", null);

        public readonly static ConsulHost Instance;

        static ConsulHost()
        {
            ConsulHost.Instance = new ConsulHost();
        }

        private ConsulHost()
        {
            ConsulConfigurator.Instance.RefreshConsulAgentAvailability();
        }

        private bool DeregisterService(string serviceName)
        {
            return ConsulClientSDK.DeregisterService(ConsulConfigurator.Instance.ConsulAgentUrl, serviceName).Result;
        }

        public string DiscoverService(string serviceName, string tags = "", bool supportFailoverSite = false, int cacheExpirePeriod = 1000)
        {
            DateTimeOffset now;
            string empty = string.Empty;
            string str = this.FormatTags(tags);
            string str1 = string.Format("{0}-{1}", serviceName, str);
            string str2 = this.serviceCache.Get(str1, null) as string;
            if (string.IsNullOrEmpty(str2))
            {
                List<string> strs = new List<string>();
                strs = (!ConsulConfigurator.Instance.ConsulAgentAvailable ? this.QueryServiceURLs(LocationHelper.Instance.GetAppUri(AppNameEnum.ServiceDiscoveryPrimaryApi), serviceName, str) : this.QueryServiceURLs(ConsulConfigurator.Instance.ConsulAgentUrl, serviceName, str));
                if ((strs == null ? true : strs.Count == 0))
                {
                    if (supportFailoverSite)
                    {
                        strs = this.QueryServiceURLs(LocationHelper.Instance.GetAppUri(AppNameEnum.ServiceDiscoveryBackupApi), serviceName, str);
                    }
                }
                if ((strs == null ? false : strs.Count > 0))
                {
                    empty = (strs.Count <= 1 ? strs[0] : strs[(new Random()).Next(strs.Count)]);
                }
                if (!string.IsNullOrEmpty(empty))
                {
                    MemoryCache memoryCaches = this.serviceCache;
                    now = DateTimeOffset.Now;
                    memoryCaches.Set(str1, empty, now.AddMilliseconds((double)cacheExpirePeriod), null);
                }
                else
                {
                    MemoryCache memoryCaches1 = this.serviceCache;
                    now = DateTimeOffset.Now;
                    memoryCaches1.Set(str1, "ServiceUrlCacheBuffer", now.AddMilliseconds((double)cacheExpirePeriod), null);
                }
            }
            else if (!string.Equals(str2, "ServiceUrlCacheBuffer"))
            {
                empty = str2;
            }
            return empty;
        }

        private string FormatTags(string originalTags)
        {
            string empty = string.Empty;
            if (!string.IsNullOrEmpty(originalTags))
            {
                StringBuilder stringBuilder = new StringBuilder();
                char[] chrArray = new char[] { ',' };
                string[] strArrays = originalTags.Split(chrArray);
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    stringBuilder.Append(str.Trim()).Append(",");
                }
                string str1 = stringBuilder.ToString();
                chrArray = new char[] { ',' };
                empty = str1.TrimEnd(chrArray);
            }
            return empty;
        }

        private List<string> QueryServiceURLs(string consulServiceURL, string serviceName, string tags = "")
        {
            List<string> strs = new List<string>();
            if (!string.IsNullOrEmpty(consulServiceURL))
            {
                ServiceEntry[] result = ConsulClientSDK.AvaliableServices(consulServiceURL, serviceName, tags).Result;
                if ((result == null ? false : (int)result.Length > 0))
                {
                    ServiceEntry[] serviceEntryArray = result;
                    for (int i = 0; i < (int)serviceEntryArray.Length; i++)
                    {
                        ServiceEntry serviceEntry = serviceEntryArray[i];
                        string empty = string.Empty;
                        empty = (string.IsNullOrEmpty(serviceEntry.Service.Address) ? serviceEntry.Node.Address : serviceEntry.Service.Address);
                        strs.Add(string.Format("http://{0}:{1}/", empty, serviceEntry.Service.Port));
                    }
                }
            }
            return strs;
        }

        private bool RegisterService(string serviceName, string tags, string serviceAddress, int servicePort, string healthCheckAddress = "", int healthCheckInterval = 60)
        {
            bool result = ConsulClientSDK.RegsterService(ConsulConfigurator.Instance.ConsulAgentUrl, serviceName, tags, serviceAddress, servicePort, healthCheckInterval, healthCheckAddress, string.Empty).Result;
            return result;
        }

        public ServiceDiscoveryOperationResult TriggerAutoDeregistry()
        {
            ServiceDiscoveryOperationResult serviceDiscoveryOperationResult = new ServiceDiscoveryOperationResult()
            {
                OperationMethod = "ServiceDiscovery-Auto-Deregistry",
                IsSuccess = true
            };
            if (!ConsulConfigurator.Instance.ConsulAgentAvailable)
            {
                serviceDiscoveryOperationResult.Message = string.Concat("Consul Agent is unavailable. ", ConsulConfigurator.Instance.ConsulAgentUrl);
            }
            else
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (KeyValuePair<string, string> keyValuePair in this.autoRegisteredServiceCache)
                    {
                        serviceDiscoveryOperationResult.IsSuccess = ConsulHost.Instance.DeregisterService(keyValuePair.Key);
                        if (serviceDiscoveryOperationResult.IsSuccess)
                        {
                            stringBuilder.AppendLine(string.Format("Service {0} has successfully deregistered. Detail Service Info: {1}", keyValuePair.Key, keyValuePair.Value));
                        }
                    }
                    serviceDiscoveryOperationResult.Message = stringBuilder.ToString();
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    serviceDiscoveryOperationResult.IsSuccess = false;
                    serviceDiscoveryOperationResult.ErrorMessage = exception.ToString();
                }
            }
            return serviceDiscoveryOperationResult;
        }

        public ServiceDiscoveryOperationResult TriggerAutoRegistry()
        {
            char[] chrArray;
            ServiceDiscoveryOperationResult serviceDiscoveryOperationResult = new ServiceDiscoveryOperationResult()
            {
                OperationMethod = "ServiceDiscovery-Auto-Registry",
                IsSuccess = true
            };
            if (!ConsulConfigurator.Instance.ConsulAgentAvailable)
            {
                serviceDiscoveryOperationResult.Message = string.Concat("Consul Agent is unavailable. ", ConsulConfigurator.Instance.ConsulAgentUrl);
            }
            else
            {
                try
                {
                    if ((ConsulConfigurator.Instance.ServiceDiscoveryItems == null ? true : ConsulConfigurator.Instance.ServiceDiscoveryItems.Count <= 0))
                    {
                        serviceDiscoveryOperationResult.Message = string.Concat("No services configured.", ConsulConfigurator.Instance.ConsulAgentUrl);
                    }
                    else
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (ServiceDiscovery serviceDiscoveryItem in ConsulConfigurator.Instance.ServiceDiscoveryItems)
                        {
                            if (serviceDiscoveryItem.AutoRegisry)
                            {
                                string empty = string.Empty;
                                if (!(serviceDiscoveryItem.HealthCheckURL.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? false : !serviceDiscoveryItem.HealthCheckURL.StartsWith("https", StringComparison.OrdinalIgnoreCase)))
                                {
                                    empty = serviceDiscoveryItem.HealthCheckURL;
                                }
                                else if (!string.IsNullOrEmpty(serviceDiscoveryItem.ServiceAddress))
                                {
                                    string str = string.Format("http://{0}:{1}/", serviceDiscoveryItem.ServiceAddress, serviceDiscoveryItem.ServicePort);
                                    string healthCheckURL = serviceDiscoveryItem.HealthCheckURL;
                                    chrArray = new char[] { '/' };
                                    empty = Path.Combine(str, healthCheckURL.TrimStart(chrArray));
                                }
                                else
                                {
                                    string str1 = string.Format("http://{0}:{1}/", "localhost", serviceDiscoveryItem.ServicePort);
                                    string healthCheckURL1 = serviceDiscoveryItem.HealthCheckURL;
                                    chrArray = new char[] { '/' };
                                    empty = Path.Combine(str1, healthCheckURL1.TrimStart(chrArray));
                                }
                                string str2 = this.FormatTags(serviceDiscoveryItem.ServiceTags);
                                serviceDiscoveryOperationResult.IsSuccess = ConsulHost.Instance.RegisterService(serviceDiscoveryItem.ServiceName, str2, serviceDiscoveryItem.ServiceAddress, serviceDiscoveryItem.ServicePort, empty, serviceDiscoveryItem.HealthInterval);
                                if (serviceDiscoveryOperationResult.IsSuccess)
                                {
                                    this.autoRegisteredServiceCache.TryAdd(serviceDiscoveryItem.ServiceName, string.Format("http://{0}:{1}/", serviceDiscoveryItem.ServiceAddress, serviceDiscoveryItem.ServicePort));
                                    object[] serviceName = new object[] { serviceDiscoveryItem.ServiceName, serviceDiscoveryItem.ServiceAddress, serviceDiscoveryItem.ServicePort, empty, serviceDiscoveryItem.HealthInterval };
                                    stringBuilder.AppendLine(string.Format("Service {0} has successfully registered with IP: {1}, Port: {2}, HealthCheckURL: {3}, HealthCheckInterval: {4}s", serviceName));
                                }
                            }
                        }
                        serviceDiscoveryOperationResult.Message = stringBuilder.ToString();
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    serviceDiscoveryOperationResult.IsSuccess = false;
                    serviceDiscoveryOperationResult.ErrorMessage = exception.ToString();
                }
            }
            return serviceDiscoveryOperationResult;
        }
    }
}
