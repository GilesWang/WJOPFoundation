using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common.Queue
{
    internal sealed class WorkerQueueCounters : IDisposable
    {
        #region fields
        internal const string CategoryName = "WJOP Worker Queue";
        private string _instanceName;
        internal readonly PerformanceCounter MaxQueueSize;
        internal readonly PerformanceCounter FlushSize;
        internal readonly PerformanceCounter FlushInterval;
        internal readonly PerformanceCounter QueueLength;
        internal readonly PerformanceCounter NumberOfFlushPerSec;
        internal readonly PerformanceCounter EnqueuedItemsPerSec;
        internal readonly PerformanceCounter DequeuedItemsPerSec;
        internal readonly PerformanceCounter DroppedItemsPerSec;
        #endregion


        #region ctor
        internal WorkerQueueCounters(string instanceName, bool asEmptyObject = false, bool deleteIfExists = false)
        {
            if (!asEmptyObject)
            {
                _instanceName = instanceName;
                bool flag = PerformanceCounterCategory.Exists("TGOP Worker Queue");
                if ((!deleteIfExists ? false : flag))
                {
                    PerformanceCounterCategory.Delete("TGOP Worker Queue");
                    flag = false;
                }
                if (!flag)
                {
                    CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
                    counterCreationDataCollection.Add(CreationData.MaxQueueSize);
                    counterCreationDataCollection.Add(CreationData.FlushSize);
                    counterCreationDataCollection.Add(CreationData.FlushInterval);
                    counterCreationDataCollection.Add(CreationData.QueueLength);
                    counterCreationDataCollection.Add(CreationData.NumberOfFlushPerSec);
                    counterCreationDataCollection.Add(CreationData.EnqueuedItemsPerSec);
                    counterCreationDataCollection.Add(CreationData.DequeuedItemsPerSec);
                    counterCreationDataCollection.Add(CreationData.DroppedItemsPerSec);
                    PerformanceCounterCategory.Create("TGOP Worker Queue", "TGOP Worker Queue", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
                }
                MaxQueueSize = NewCounter(CreationData.MaxQueueSize);
                FlushSize = NewCounter(CreationData.FlushSize);
                FlushInterval = NewCounter(CreationData.FlushInterval);
                QueueLength = NewCounter(CreationData.QueueLength);
                NumberOfFlushPerSec = NewCounter(CreationData.NumberOfFlushPerSec);
                EnqueuedItemsPerSec = NewCounter(CreationData.EnqueuedItemsPerSec);
                DequeuedItemsPerSec = NewCounter(CreationData.DequeuedItemsPerSec);
                DroppedItemsPerSec = NewCounter(CreationData.DroppedItemsPerSec);
            }

        }
        #endregion
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                MaxQueueSize.Dispose();
                FlushSize.Dispose();
                FlushInterval.Dispose();
                QueueLength.Dispose();
                NumberOfFlushPerSec.Dispose();
                EnqueuedItemsPerSec.Dispose();
                DequeuedItemsPerSec.Dispose();
                DroppedItemsPerSec.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        ~WorkerQueueCounters()
        {
            Dispose(false);
        }

        internal PerformanceCounter NewCounter(CounterCreationData cd)
        {
            PerformanceCounter counter = new PerformanceCounter() {
                CategoryName = "TGOP Worker Queue",
                CounterName = cd.CounterName,
                InstanceName = _instanceName,
                InstanceLifetime = PerformanceCounterInstanceLifetime.Process,
                ReadOnly = false,
                RawValue = (long)0
            };
            return counter;
        }

        private static class CreationData
        {
            internal readonly static CounterCreationData MaxQueueSize;

            internal readonly static CounterCreationData FlushSize;

            internal readonly static CounterCreationData FlushInterval;

            internal readonly static CounterCreationData QueueLength;

            internal readonly static CounterCreationData NumberOfFlushPerSec;

            internal readonly static CounterCreationData EnqueuedItemsPerSec;

            internal readonly static CounterCreationData DequeuedItemsPerSec;

            internal readonly static CounterCreationData DroppedItemsPerSec;
            static CreationData()
            {
                MaxQueueSize = new CounterCreationData("MaxQueueSize", "MaxQueueSize", PerformanceCounterType.NumberOfItems32);
                FlushSize = new CounterCreationData("FlushSize", "FlushSize", PerformanceCounterType.NumberOfItems32);
                FlushInterval = new CounterCreationData("FlushInterval", "FlushInterval", PerformanceCounterType.NumberOfItems32);
                QueueLength = new CounterCreationData("QueueLength", "QueueLength", PerformanceCounterType.NumberOfItems32);
                NumberOfFlushPerSec = new CounterCreationData("NumberOfFlushPerSec", "NumberOfFlushPerSec", PerformanceCounterType.RateOfCountsPerSecond32);
                EnqueuedItemsPerSec = new CounterCreationData("EnqueuedItemsPerSec", "EnqueuedItemsPerSec", PerformanceCounterType.RateOfCountsPerSecond32);
                DequeuedItemsPerSec = new CounterCreationData("DequeuedItemsPerSec", "DequeuedItemsPerSec", PerformanceCounterType.RateOfCountsPerSecond32);
                DroppedItemsPerSec = new CounterCreationData("DroppedItemsPerSec", "DroppedItemsPerSec", PerformanceCounterType.RateOfCountsPerSecond32);
            }
        }
    }
}
