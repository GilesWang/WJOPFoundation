using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJOP.Foundation.Common.Utility;

namespace WJOP.Foundation.Common.Queue
{
    public class WorkerQueueGroup<T>
    {
        private object _queuelocker;

        private Dictionary<string, WorkerQueue<T>> _queues;

        private string _queueGroupName;

        private int _batchSize;

        private int _flushInterval;

        public WorkerQueueGroup(int batchSize, int flushInterval, string queueGroupName = "")
        {
            this._queuelocker = new object();
            this._queues = new Dictionary<string, WorkerQueue<T>>();
            this._batchSize = batchSize;
            this._flushInterval = flushInterval;
            this._queueGroupName = queueGroupName;
        }

        public void Enqueue(string queueID, T item)
        {
            ParamterUtil.CheckEmptyString("queueID", queueID);
            ParamterUtil.CheckNull("item", item);
            try
            {
                WorkerQueue<T> workerQueue = this.PickupQueue(queueID);
                if (workerQueue == null)
                {
                    DebugUtil.Log(string.Format("WorkerQueueGroup: failed to pick up queue with id '{0}'.", queueID));
                }
                else
                {
                    workerQueue.Enqueue(item);
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        public void Enqueue(string queueID, IList<T> items)
        {
            ParamterUtil.CheckEmptyString("queueID", queueID);
            ParamterUtil.CheckNull("items", items);
            try
            {
                if (items.Count > 0)
                {
                    WorkerQueue<T> workerQueue = this.PickupQueue(queueID);
                    if (workerQueue == null)
                    {
                        DebugUtil.Log(string.Format("WorkerQueueGroup: failed to pick up queue with id '{0}'.", queueID));
                    }
                    else
                    {
                        foreach (T item in items)
                        {
                            workerQueue.Enqueue(item);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        private void OnInnerQueueFlush(object queueObj, List<T> items)
        {
            try
            {
                if (this.Flush != null)
                {
                    WorkerQueue<T> workerQueue = queueObj as WorkerQueue<T>;
                    if (workerQueue != null)
                    {
                        this.Flush(workerQueue, items);
                    }
                }
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
        }

        private WorkerQueue<T> PickupQueue(string queueID)
        {
            WorkerQueue<T> workerQueue = null;
            try
            {
                if (!this._queues.ContainsKey(queueID))
                {
                    DebugUtil.Log("WorkerQueueGroup: >>> Try to acquire queue locker.");
                    lock (this._queuelocker)
                    {
                        DebugUtil.Log("WorkerQueueGroup: Get queue locker.");
                        if (!this._queues.ContainsKey(queueID))
                        {
                            workerQueue = new WorkerQueue<T>(this._batchSize, this._flushInterval, queueID);
                            workerQueue.Flush += new Action<object, List<T>>(this.OnInnerQueueFlush);
                            this._queues.Add(queueID, workerQueue);
                        }
                    }
                    DebugUtil.Log("WorkerQueueGroup: <<< Release queue locker.");
                }
                workerQueue = this._queues[queueID];
            }
            catch (Exception exception)
            {
                DebugUtil.LogException(exception);
            }
            return workerQueue;
        }

        public event Action<WorkerQueue<T>, List<T>> Flush;
    }
}
