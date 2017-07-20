using Couchbase.Configuration.Client;
using Couchbase.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Cache.SDK
{
    internal sealed class CouchbaseConfiguration
    {
        public CouchbaseConfiguration()
        {
        }

        public static ClientConfiguration GetConfig()
        {
            CouchbaseConfigEntity couchbaseConfigEntity = new CouchbaseConfigEntity();
            ClientConfiguration clientConfiguration = new ClientConfiguration()
            {
                Servers = couchbaseConfigEntity.Servers,
                UseSsl = couchbaseConfigEntity.UseSsl,
                DefaultConnectionLimit = couchbaseConfigEntity.DefaultConnectionLimit,
                EnableTcpKeepAlives = couchbaseConfigEntity.EnableTcpKeepAlives,
                TcpKeepAliveTime = couchbaseConfigEntity.TcpKeepAliveTime,
                TcpKeepAliveInterval = couchbaseConfigEntity.TcpKeepAliveInterval,
                Serializer = () => {
                    JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                    };
                    return new DefaultSerializer(jsonSerializerSetting, jsonSerializerSetting);
                }
            };
            Dictionary<string, BucketConfiguration> strs = new Dictionary<string, BucketConfiguration>();
            string bucketName = couchbaseConfigEntity.BucketName;
            BucketConfiguration bucketConfiguration = new BucketConfiguration()
            {
                BucketName = couchbaseConfigEntity.BucketName,
                UseSsl = couchbaseConfigEntity.UseSsl,
                Password = couchbaseConfigEntity.Password,
                DefaultOperationLifespan = couchbaseConfigEntity.DefaultOperationLifespan
            };
            PoolConfiguration poolConfiguration = new PoolConfiguration(null)
            {
                MaxSize = couchbaseConfigEntity.PoolMaxSize,
                MinSize = couchbaseConfigEntity.PoolMinSize,
                SendTimeout = couchbaseConfigEntity.PoolSendTimeout
            };
            bucketConfiguration.PoolConfiguration = poolConfiguration;
            strs.Add(bucketName, bucketConfiguration);
            clientConfiguration.BucketConfigs = strs;
            return clientConfiguration;
        }
    }
}
