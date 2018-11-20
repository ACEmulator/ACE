using System;
using System.Collections.Generic;

using log4net;

namespace ACE.Server.Entity.Actions
{
    public class DelayManager : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SortedSet<DelayAction> delayHeap = new SortedSet<DelayAction>();

        public void RunActions()
        {
            // While the minimum time of our delayHeap is > our current time, kick off actions
            bool checkNeeded = true;

            while (checkNeeded)
            {
                checkNeeded = false;
                List<DelayAction> toAct = new List<DelayAction>();

                // Actions may be added to delayHeap from network queue -- therefore this is needed
                lock (delayHeap)
                {
                    while (delayHeap.Count > 0)
                    {
                        // Find the next (O(1))
                        var min = delayHeap.Min;

                        // If they wanted to run before or at now
                        if (min.EndTime <= Timers.PortalYearTicks)
                        {
                            toAct.Add(min);

                            // O(log(n))
                            delayHeap.Remove(min);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                foreach (var action in toAct)
                {
                    Tuple<IActor, IAction> next = action.Act();

                    if (next != null)
                        next.Item1.EnqueueAction(next.Item2);
                }
            }
        }

        public void EnqueueAction(IAction action)
        {
            var delayAction = action as DelayAction;

            if (delayAction == null)
            {
                log.Error("Non DelayAction IAction added to DelayManager");
                return;
            }

            delayAction.Start();

            lock (delayHeap)
                delayHeap.Add(delayAction);
        }
    }
}
