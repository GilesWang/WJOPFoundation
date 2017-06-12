using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Cache.SDK
{
    internal sealed class SafeBucket : IDisposable
    {
        private const int MAX_THREAD_COUNT = 20;

        private const long EXPIRED_TIME_MICROSECOND = 1200000000L;

        private readonly static ConcurrentQueue<SafeBucket> bucketPool;

        private readonly static ConcurrentQueue<SafeBucket> garbagePool;

        private readonly static ClientConfiguration config;

        private readonly static object syncLock;

        private readonly ICluster cluster;

        private readonly IBucket bucket;

        private readonly long expiredTime;

        private readonly int maxThreadCount;

        private int currentThreadCount;

        public IBucket Bucket
        {
            get
            {
                return this.bucket;
            }
        }

        public static SafeBucket Instance
        {
            get
            {
                SafeBucket safeBucket = null;
                lock (SafeBucket.syncLock)
                {
                    bool flag = false;
                    List<SafeBucket> safeBuckets = new List<SafeBucket>();
                    while (!flag)
                    {
                        if (!SafeBucket.bucketPool.TryDequeue(out safeBucket))
                        {
                            SafeBucket.bucketPool.Enqueue(new SafeBucket());
                        }
                        else if (!(DateTime.Now.Ticks >= safeBucket.expiredTime ? true : safeBucket.currentThreadCount >= safeBucket.maxThreadCount))
                        {
                            safeBucket.Enter();
                            SafeBucket.bucketPool.Enqueue(safeBucket);
                            flag = true;
                        }
                        else if (DateTime.Now.Ticks <= safeBucket.expiredTime)
                        {
                            safeBuckets.Add(safeBucket);
                        }
                        else
                        {
                            safeBucket.Exit();
                            SafeBucket.garbagePool.Enqueue(safeBucket);
                        }
                    }
                    safeBuckets.ForEach((SafeBucket o) => SafeBucket.bucketPool.Enqueue(o));
                }
                DebugUtil.CollectDebugInfo(string.Format("bucketPool.Count={0}, garbagePool.Count={1}", SafeBucket.bucketPool.Count, SafeBucket.garbagePool.Count));
                return safeBucket;
            }
        }

        static SafeBucket()
        {
            SafeBucket.bucketPool = new ConcurrentQueue<SafeBucket>();
            SafeBucket.garbagePool = new ConcurrentQueue<SafeBucket>();
            SafeBucket.config = CouchbaseConfiguration.GetConfig();
            SafeBucket.syncLock = new object();
            SafeBucket.bucketPool.Enqueue(new SafeBucket());
        }

        private SafeBucket()
        {
            this.cluster = new Cluster(SafeBucket.config);
            this.bucket = this.cluster.OpenBucket();
            this.expiredTime = DateTime.Now.Ticks + (long)1200000000;
            this.maxThreadCount = SafeBucket.config.BucketConfigs[this.bucket.Name].PoolConfiguration.MaxSize;
        }

        private static void CloseBuckets(object status)
        {
            SafeBucket safeBucket;
            try
            {
                int count = SafeBucket.garbagePool.Count;
                for (int i = 0; i < count; i++)
                {
                    if (SafeBucket.garbagePool.TryDequeue(out safeBucket))
                    {
                        if (safeBucket.currentThreadCount != -1)
                        {
                            SafeBucket.garbagePool.Enqueue(safeBucket);
                        }
                        else
                        {
                            safeBucket.bucket.Dispose();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.CollectDebugInfo(string.Format("CloseBuckets Exception:{0}", exception.Message));
            }
        }

        public void Dispose()
        {
            this.Exit();
            SafeBucket.CloseBuckets(null);
        }

        private int Enter()
        {
            return Interlocked.Increment(ref this.currentThreadCount);
        }

        private int Exit()
        {
            return Interlocked.Decrement(ref this.currentThreadCount);
        }
    }
}
