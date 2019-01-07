using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Actions
{
    public class ActionChain
    {
        public WorldObject WorldObject;

        private double nextTime;

        public SortedDictionary<double, List<Action>> Actions;

        public double NextActionTime => Actions.Count != 0 ? Actions.Keys.First() : double.MaxValue;

        public bool IsComplete => Actions.Count == 0;

        public ActionChain()
        {
            Init();
        }

        public ActionChain(Action action)
        {
            Init();

            AddAction(action);
        }

        public ActionChain(WorldObject wo)
        {
            Init();

            WorldObject = wo;
        }

        public ActionChain(WorldObject wo, Action action)
        {
            Init();

            WorldObject = wo;

            AddAction(action);
        }

        public void Init()
        {
            nextTime = Time.GetUnixTime();

            Actions = new SortedDictionary<double, List<Action>>();
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

        public void AddAction(Action action)
        {
            Actions.TryGetValue(nextTime, out var existing);

            if (existing == null)
                Actions.Add(nextTime, new List<Action>() { action });
            else
                existing.Add(action);
        }

        public void AddAction(WorldObject wo, Action action)
        {
            WorldObject = wo;
            AddAction(action);
        }

        public void AddChain(ActionChain chain)
        {

        }

        public void AddLoop(WorldObject wo, Action action)
        {

        }

        public void AddLoop(WorldObject wo, Action action, ActionChain actionChain)
        {

        }

        public void EnqueueChain()
        {
            WorldObject.EnqueueChain(this);
        }
    }
}
