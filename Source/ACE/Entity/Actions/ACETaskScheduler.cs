using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    /// <summary>
    /// Coroutine scheduler.  Only runs task when main loop calls the "DoWork" function
    /// </summary>
    public class ACETaskScheduler : TaskScheduler
    {
        /// <summary>
        /// Protected by lock(tasks)
        /// </summary>
        private LinkedList<Task> tasks = new LinkedList<Task>();

        public ACETaskScheduler()
        {
        }

        protected override void QueueTask(Task t)
        {
            lock (tasks)
            {
                tasks.AddLast(t);
            }
        }

        public void DoWork()
        {
            LinkedList<Task> tmp;
            lock (tasks)
            {
                tmp = tasks;
                tasks = new LinkedList<Task>();
            }

            foreach (Task t in tmp)
            {
                base.TryExecuteTask(t);

                // FIXME(ddevec): Need to find a way to better report exceptions :(
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }
            }
        }

        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued)
            {
                lock (tasks)
                {
                    bool success = tasks.Remove(task);
                    if (!success)
                    {
                        return false;
                    }
                }
            }

            return base.TryExecuteTask(task);
        }

        public sealed override int MaximumConcurrencyLevel => 1;

        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(tasks, ref lockTaken);
                if (lockTaken)
                {
                    return tasks;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(tasks);
                }
            }
        }
    }
}
