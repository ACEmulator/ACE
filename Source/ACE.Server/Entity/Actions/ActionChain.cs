using System;
using System.Collections.Generic;
using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Actions
{
    public class ActionChain
    {
        public WorldObject WorldObject;

        public Dictionary<double, List<Action>> Actions = new Dictionary<double, List<Action>>();

        private double nextTime;

        /// <summary>
        /// This is getting messy here - adding this for the current EmoteManager,
        /// which makes heavy use of nested action chains.
        /// Ideally these should be refactored, which will fix some other issues in EmoteManager as well...
        /// </summary>
        private bool queued;

        public ActionChain()
        {
            nextTime = Time.GetUnixTime();
        }

        public void AddDelaySeconds(double timeInSeconds)
        {
            if (double.IsNaN(timeInSeconds))
            {
                Console.WriteLine($"ActionChain.AddDelaySeconds({timeInSeconds}: NaN");
                return;
            }

            if (queued)
            {
                Console.WriteLine($"ActionChain.AddDelaySeconds({timeInSeconds}) - nested action chain detected! Please report this, along with actions being performed when it happened");
                nextTime = Time.GetUnixTime();
            }

            nextTime += timeInSeconds;
        }

        public void AddAction(WorldObject wo, Action action)
        {
            WorldObject = wo;

            if (!Actions.TryGetValue(nextTime, out var existing))
                Actions.Add(nextTime, new List<Action>() { action });
            else
                existing.Add(action);

            if (queued)
            {
                Console.WriteLine($"ActionChain.AddAction({wo.Name}) - nested action chain detected! Please report this, along with actions being performed when it happened");
                EnqueueChain();
            }
        }

        public void EnqueueChain()
        {
            if (WorldObject != null)
                WorldObject.EnqueueChain(this);

            // for nested chains - refactor this
            Actions.Clear();
            queued = true;
        }
    }
}
