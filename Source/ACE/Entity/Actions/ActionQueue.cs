using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public class ActionQueue : IActor
    {
        protected object queueLock = new object();
        protected List<IAction> queue = new List<IAction>();

        public ActionQueue()
        {
        }

        public void RunActions()
        {
            List<IAction> tmp;
            lock (queueLock)
            {
                tmp = queue;
                queue = new List<IAction>();
            }

            foreach (var next in queue)
            {
                Tuple<IActor, IAction> enqueue = next.Act();

                enqueue.Item1.EnqueueAction(enqueue.Item2);
            }
        }

        public virtual void EnqueueAction(IAction action)
        {
            lock (queueLock)
            {
                queue.Add(action);
            }
        }
    }
}
