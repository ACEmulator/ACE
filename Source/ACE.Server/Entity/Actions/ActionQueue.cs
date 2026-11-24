// Uncomment this if you want to measure the time actions take to execute and report slow ones
//#define WRAP_AND_MEASURE_ACT_WITH_STOPWATCH

using System;
using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

using ACE.Common.Logging;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue : IActor
    {
        protected ConcurrentQueue<IAction> Queue { get; } = new ConcurrentQueue<IAction>();

        #if WRAP_AND_MEASURE_ACT_WITH_STOPWATCH
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        #endif

        public void RunActions()
        {
            if (Queue.IsEmpty)
                return;

            var count = Queue.Count;

            for (int i = 0; i < count; i++)
            {
                if (Queue.TryDequeue(out var result))
                {
                    #if WRAP_AND_MEASURE_ACT_WITH_STOPWATCH
                    sw.Restart();
                    #endif

                    Tuple<IActor, IAction> enqueue = result.Act();

                    #if WRAP_AND_MEASURE_ACT_WITH_STOPWATCH
                    sw.Stop();

                    if (sw.Elapsed.TotalSeconds > 1)
                    {
                        if (result is ActionEventDelegate actionEventDelegate)
                        {
                            if (actionEventDelegate.Action.Target is WorldObjects.WorldObject worldObject)
                                log.LogWarning("ActionQueue Act() took {TotalSeconds:N0}s. Method.Name: {MethodName}, Target: {Target} 0x{Guid}:{WorldObjectName}", sw.Elapsed.TotalSeconds, actionEventDelegate.Action.Method.Name, actionEventDelegate.Action.Target, worldObject.Guid, worldObject.Name);
                            else
                                log.LogWarning("ActionQueue Act() took {TotalSeconds:N0}s. Method.Name: {Name}, Target: {Target}", sw.Elapsed.TotalSeconds, actionEventDelegate.Action.Method.Name, actionEventDelegate.Action.Target);
                        }
                        else
                            log.LogWarning("ActionQueue Act() took {TotalSeconds:N0}s.", sw.Elapsed.TotalSeconds);
                    }
                    #endif

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
