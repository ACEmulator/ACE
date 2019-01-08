using System;
using System.Collections.Generic;
using ACE.Common;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue
    {
        public SortedDictionary<double, List<Action>> Actions = new SortedDictionary<double, List<Action>>();

        public Dictionary<double, List<Action>> pendingActions = new Dictionary<double, List<Action>>();

        private bool isPending;

        public ActionQueue() { }

        public void EnqueueChain(ActionChain actionChain)
        {
            foreach (var action in actionChain.Actions)
            {
                if (pendingActions.TryGetValue(action.Key, out var existing))
                    existing.AddRange(action.Value);
                else
                    pendingActions.Add(action.Key, action.Value);
            }
            isPending = true;
        }

        public void EnqueueAction(Action action)
        {
            var currentTime = Time.GetUnixTime();

            if (pendingActions.TryGetValue(currentTime, out var existing))
                existing.Add(action);
            else
                pendingActions.Add(currentTime, new List<Action>() { action });

            isPending = true;
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
            isPending = true;
        }

        public void RunActions()
        {
            if (isPending)
            {
                foreach (var kvp in pendingActions)
                {
                    if (Actions.TryGetValue(kvp.Key, out var existing))
                        existing.AddRange(kvp.Value);
                    else
                        Actions.Add(kvp.Key, kvp.Value);
                }
                isPending = false;
                pendingActions.Clear();
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
