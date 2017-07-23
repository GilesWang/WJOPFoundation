using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WJOP.Foundation.APIStore.SDK.Consul
{
    public class ConsulConfigurator
    {
        private List<ServiceDiscovery> serviceDiscoveryList = new List<ServiceDiscovery>();

        private bool consulAgentAvailable = false;

        public readonly static ConsulConfigurator Instance;

        private string consulAgentUrl = "http://127.0.0.1:8500/";

        public bool ConsulAgentAvailable
        {
            get
            {
                return this.consulAgentAvailable;
            }
            set
            {
                this.consulAgentAvailable = value;
            }
        }

        public string ConsulAgentUrl
        {
            get
            {
                return this.consulAgentUrl;
            }
            set
            {
                this.consulAgentUrl = value;
            }
        }

        public List<ServiceDiscovery> ServiceDiscoveryItems
        {
            get
            {
                return this.serviceDiscoveryList;
            }
        }

        static ConsulConfigurator()
        {
            ConsulConfigurator.Instance = new ConsulConfigurator();
        }

        private ConsulConfigurator()
        {
        }

        private bool CheckConsulAgentAvailability()
        {
            bool flag = true;
            try
            {
                (new WebClient()).DownloadData(this.consulAgentUrl);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public void ConfigureConsulAgentUrl(string consulAgentIP, string consulAgentPort)
        {
            this.consulAgentUrl = string.Format("http://{0}:{1}/", consulAgentIP, consulAgentPort);
        }

        public ServiceDiscoveryOperationResult InitializeConfiguration(string configFilePath)
        {
            ServiceDiscoveryOperationResult serviceDiscoveryOperationResult = new ServiceDiscoveryOperationResult()
            {
                OperationMethod = "ServiceDiscovery-Initialize-Config",
                IsSuccess = true
            };
            try
            {
                FileStream fileStream = new FileStream(configFilePath, FileMode.Open);
                try
                {
                    ServiceDiscoveryConfiguration serviceDiscoveryConfiguration = (new XmlSerializer(typeof(ServiceDiscoveryConfiguration))).Deserialize(fileStream) as ServiceDiscoveryConfiguration;
                    if (serviceDiscoveryConfiguration != null)
                    {
                        this.serviceDiscoveryList = serviceDiscoveryConfiguration.ConfigItems;
                        if ((string.IsNullOrEmpty(serviceDiscoveryConfiguration.AgentIP) ? false : !string.IsNullOrEmpty(serviceDiscoveryConfiguration.AgentPort)))
                        {
                            this.ConfigureConsulAgentUrl(serviceDiscoveryConfiguration.AgentIP, serviceDiscoveryConfiguration.AgentPort);
                        }
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        ((IDisposable)fileStream).Dispose();
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                serviceDiscoveryOperationResult.IsSuccess = false;
                serviceDiscoveryOperationResult.ErrorMessage = exception.ToString();
            }
            return serviceDiscoveryOperationResult;
        }

        public void RefreshConsulAgentAvailability()
        {
            this.ConsulAgentAvailable = this.CheckConsulAgentAvailability();
            (new Thread(() => {
                while (true)
                {
                    this.ConsulAgentAvailable = this.CheckConsulAgentAvailability();
                    Thread.Sleep(5000);
                }
            })).Start();
        }
    }

}
