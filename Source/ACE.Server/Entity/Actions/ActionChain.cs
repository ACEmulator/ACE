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
            nextTime += timeInSeconds;
        }

        public void AddAction(WorldObject wo, Action action)
        {
            WorldObject = wo;

            if (!Actions.TryGetValue(nextTime, out var existing))
                Actions.Add(nextTime, new List<Action>() { action });
            else
                existing.Add(action);
        }

        public void EnqueueChain()
        {
            WorldObject.EnqueueChain(this);
        }
    }
}
