﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Managers;

using log4net;

namespace ACE.Entity.Actions
{
    public class DelayManager : IActor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SortedSet<DelayAction> delayHeap = new SortedSet<DelayAction>();

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
                        var min = delayHeap.Min();

                        // If they wanted to run before or at now
                        if (min.EndTime <= WorldManager.PortalYearTicks)
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
                    {
                        next.Item1.EnqueueAction(next.Item2);
                    }
                }
            }
        }

        public LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            DelayAction delayAction = action as DelayAction;

            if (delayAction == null)
            {
                log.Error("Non DelayAction IAction added to DelayManager");
                return null;
            }

            delayAction.Start();

            lock (delayHeap)
            {
                delayHeap.Add(delayAction);
            }

            return null;
        }

        public void DequeueAction(LinkedListNode<IAction> action)
        {
            log.Error("DelayManager Doesn't support DequeueAction");
        }
    }
}
