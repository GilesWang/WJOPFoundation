using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Common.Queue
{
    public sealed class WorkerQueue<T> : IDisposable
    {
        #region fields
        public const int DefaultQueueCapacity = 100000;
        public const int SystemClockResolution = 15;
        public event Action<object, List<T>> Flush;
        private string _queueName;
        private WorkerQueueCounters PC;
        private ConcurrentQueue<T> _innerQueue;
        private Task _processQueueTask;
        private int _maxBatchDequeueSize;
        private int _maxQueueLength;
        private int _flushInterval;
        private Timer _timer;
        private int _dequeueFlag;
        #endregion

        #region properties
        public int Capactity { get { return _maxQueueLength; } }
        public int Length { get { return _innerQueue.Count; } }
        public string QueueName { get { return _queueName; } }
        #endregion

        #region ctor
        public WorkerQueue(int batchSize, int flushInterval, string queueName = "")
        {
            Init(batchSize, flushInterval, DefaultQueueCapacity, queueName);
        }
        public WorkerQueue(int batchSize, int flushInterval, int queueCapacity, string queueName = "")
        {
            Init(batchSize, flushInterval, queueCapacity, queueName);
        }
        #endregion

        #region private methods
        private void Init(int batchSize, int flushInterval, int maxQueueLength, string queueName = "")
        {
            try
            {
                _innerQueue = new ConcurrentQueue<T>();
                _maxBatchDequeueSize = batchSize;
                _flushInterval = flushInterval;
                _maxQueueLength = maxQueueLength;
                _queueName = (string.IsNullOrEmpty(queueName) ? "DefaultWorkerQueue" : queueName);
                if (!AppContext.WorkerQueuePerfCounterEnabled)
                {
                    PC = new WorkerQueueCounters(_queueName, true, false);
                }
                else
                {
                    PC = new WorkerQueueCounters(_queueName, false, false);
                    PC.MaxQueueSize.RawValue = (long)maxQueueLength;
                    PC.FlushSize.RawValue = (long)batchSize;
                    PC.FlushInterval.RawValue = (long)flushInterval;
                }
                _processQueueTask = Task.Factory.StartNew(new Action(ProcessQueue));
                StartRepeatFlushQueueTimer();
            }
            catch (Exception ex)
            {

                DebugUtil.LogException(ex);
            }
        }
        private void StartRepeatFlushQueueTimer()
        {
            _timer = new Timer(new TimerCallback(TimerCallback), null, _flushInterval, -1);
        }
        private void TimerCallback(object state)
        {
            SetDequeueEvent();
            RestartTimer();
        }
        private void RestartTimer()
        {
            _timer.Change(_flushInterval, -1);
        }
        private void SetDequeueEvent()
        {
            Interlocked.Exchange(ref _dequeueFlag, 1);
        }
        private void ProcessQueue()
        {
            while (true)
            {
                WaitDequeueEvent();
                DequeueAll();
            }
        }
        private void DequeueAll()
        {
            while (!_innerQueue.IsEmpty)
            {
                BatchDequeue(_maxBatchDequeueSize);
            }
        }
        private void BatchDequeue(int batchSize)
        {
            T t;
            List<T> ts = new List<T>();
            while (true)
            {
                if (ts.Count >= batchSize ? true : _innerQueue.Count <= 0)
                {
                    break;
                }
                if (_innerQueue.TryDequeue(out t))
                {
                    ts.Add(t);
                    PCIncrementValue(this.PC.DequeuedItemsPerSec);
                    PCSetRawValue(this.PC.QueueLength, (long)this._innerQueue.Count);
                }
            }
            OnFlush(ts);
        }
        private void OnFlush(List<T> items)
        {
            try
            {
                if (items != null)
                {
                    this.PCIncrementValue(this.PC.NumberOfFlushPerSec);
                    if (this.Flush != null)
                    {
                        this.Flush(this, items);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugUtil.LogException(ex);
            }
        }
        private void PCIncrementValue(PerformanceCounter pc)
        {
            if ((pc == null ? false : AppContext.WorkerQueuePerfCounterEnabled))
            {
                pc.Increment();
            }
        }
        private void PCSetRawValue(PerformanceCounter pc, long value)
        {
            if ((pc == null ? false : AppContext.WorkerQueuePerfCounterEnabled))
            {
                pc.RawValue = value;
            }
        }
        private void WaitDequeueEvent()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref _dequeueFlag, 0) == 1)
                {
                    break;
                }
                Thread.Sleep(_flushInterval);
            }
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                BatchDequeue(_maxBatchDequeueSize);
                if (PC != null)
                {
                    PC.Dispose();
                }
            }
        }
        #endregion

        #region public methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Enqueue(T item)
        {
            try
            {
                if (this._innerQueue.Count >= this._maxQueueLength)
                {
                    this.PCIncrementValue(this.PC.DroppedItemsPerSec);
                    DebugUtil.Log(string.Format("WorkerQueue: Queue full, there are {0} items in queue.", this._innerQueue.Count));
                }
                else
                {
                    this._innerQueue.Enqueue(item);
                    this.PCIncrementValue(this.PC.EnqueuedItemsPerSec);
                    this.PCSetRawValue(this.PC.QueueLength, (long)this._innerQueue.Count);
                    if (this._innerQueue.Count > this._maxBatchDequeueSize)
                    {
                        this.SetDequeueEvent();
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }
        #endregion

    }
}
