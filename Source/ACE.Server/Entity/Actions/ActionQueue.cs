using System;
using System.Collections.Concurrent;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue : IActor
    {
        protected ConcurrentQueue<IAction> Queue { get; } = new ConcurrentQueue<IAction>();

        public void RunActions()
        {
            if (Queue.IsEmpty)
                return;

            var count = Queue.Count;

            for (int i = 0; i < count; i++)
            {
                if (Queue.TryDequeue(out var result))
                {
                    Tuple<IActor, IAction> enqueue = result.Act();

                    if (enqueue != null)
                        enqueue.Item1.EnqueueAction(enqueue.Item2);
                }
            }
        }

        public void EnqueueAction(IAction action)
        {
            Queue.Enqueue(action);
        }

        public void Clear()
        {
            Queue.Clear();
        }
    }
}
