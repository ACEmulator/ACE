using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue
    {
        public WorldObject WorldObject;

        //private ReaderWriterLockSlim actionQueueLock = new ReaderWriterLockSlim();

        public SortedDictionary<double, List<Action>> Actions = new SortedDictionary<double, List<Action>>();

        public Dictionary<double, List<Action>> pendingActions = new Dictionary<double, List<Action>>();

        public bool IsPending;

        //public double NextActionTime;

        public ActionQueue()
        {
        }

        public ActionQueue(WorldObject wo)
        {
            WorldObject = wo;
        }

        public void EnqueueChain(ActionChain actionChain)
        {
            //lock (actionQueueLock)
            //{
                foreach (var action in actionChain.Actions)
                {
                    if (pendingActions.TryGetValue(action.Key, out var existing))
                        existing.AddRange(action.Value);
                    else
                        pendingActions.Add(action.Key, action.Value);
                }

                IsPending = true;
            //}
        }

        public void EnqueueAction(Action action)
        {
            var currentTime = Time.GetUnixTime();

            //lock (actionQueueLock)
            //{
                if (pendingActions.TryGetValue(currentTime, out var existing))
                    existing.Add(action);
                else
                    pendingActions.Add(currentTime, new List<Action>() { action });
            //}

            IsPending = true;
        }

        public void Enqueue(ActionQueue actionQueue)
        {
            foreach (var action in actionQueue.pendingActions)
            {
                if (pendingActions.TryGetValue(action.Key, out var existing))
                    existing.AddRange(action.Value);
                else
                    pendingActions.Add(action.Key, action.Value);
            }
            IsPending = true;
        }

        public void RunActions()
        {
            if (IsPending)
            {
                foreach (var kvp in pendingActions)
                {
                    if (Actions.TryGetValue(kvp.Key, out var existing))
                        existing.AddRange(kvp.Value);
                    else
                        Actions.Add(kvp.Key, kvp.Value);
                }
                pendingActions.Clear();
                IsPending = false;
            }

            var processed = new List<double>();
            foreach (var action in Actions)
            {
                if (action.Key > Time.GetUnixTime())
                    break;

                foreach (var act in action.Value)
                    act();

                processed.Add(action.Key);
            }

            foreach (var p in processed)
                Actions.Remove(p);
        }
    }
}
