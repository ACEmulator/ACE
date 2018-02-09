using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected object queueLock = new object();
        protected LinkedList<IAction> queue = new LinkedList<IAction>();

        public void RunActions()
        {
            LinkedList<IAction> tmp;
            lock (queueLock)
            {
                tmp = queue;
                queue = new LinkedList<IAction>();
            }

            foreach (var next in tmp)
            {
                Tuple<IActor, IAction> enqueue = next.Act();

                if (enqueue != null)
                {
                    enqueue.Item1.EnqueueAction(enqueue.Item2);
                }
            }
        }

        // Supports running the actions in the queue in parallel
        public void RunActionsParallel()
        {
            LinkedList<IAction> tmp;
            lock (queueLock)
            {
                tmp = queue;
                queue = new LinkedList<IAction>();
            }

            Parallel.ForEach(tmp, next =>
            {
                Tuple<IActor, IAction> enqueue = next.Act();

                if (enqueue != null)
                {
                    enqueue.Item1.EnqueueAction(enqueue.Item2);
                }
            });
        }

        public virtual LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            lock (queueLock)
            {
                return queue.AddLast(action);
            }
        }

        public void DequeueAction(LinkedListNode<IAction> node)
        {
            lock (queueLock)
            {
                if (node.List == queue)
                {
                    queue.Remove(node);
                }
                else
                {
                    log.Warn("Unexpected dequeue of node not in queue?");
                }
            }
        }
    }
}
