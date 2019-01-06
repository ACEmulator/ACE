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

        public double StartTime;
        public double NextTime;

        public Queue<ChainElement> Actions;

        public double NextActionTime => Actions.Count > 0 ? Actions.First().RunTime : double.MaxValue;

        public bool IsComplete => !Actions.Any();

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
            StartTime = Time.GetUnixTime();
            NextTime = StartTime;

            Actions = new Queue<ChainElement>();
        }

        public void AddDelaySeconds(double timeInSeconds)
        {
            if (double.IsNaN(timeInSeconds))
            {
                Console.WriteLine($"ActionChain.AddDelaySeconds({timeInSeconds}: NaN");
                return;
            }
            NextTime = NextTime + timeInSeconds;
        }

        public void AddAction(Action action)
        {
            Actions.Enqueue(new ChainElement(action, NextTime));
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

        public void RunActions()
        {
            var success = Actions.TryPeek(out var nextAction);

            while (success && nextAction.RunTime <= Time.GetUnixTime())
            {
                nextAction.Action.Invoke();
                Actions.Dequeue();

                success = Actions.TryPeek(out nextAction);
            }
        }
    }
}
