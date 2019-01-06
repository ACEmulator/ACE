using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue
    {
        public WorldObject WorldObject;

        public List<ActionChain> ActionChains = new List<ActionChain>();

        public double NextActionTime;

        public ActionQueue()
        {
        }

        public ActionQueue(WorldObject wo)
        {
            WorldObject = wo;
        }

        public void EnqueueChain(ActionChain actionChain)
        {
            ActionChains.Add(actionChain);

            GetNextActionTime();
        }

        public void EnqueueAction(Action action)
        {
            ActionChains.Add(new ActionChain(WorldObject, action));

            GetNextActionTime();
        }

        public void GetNextActionTime()
        {
            NextActionTime = ActionChains.Count > 0 ? ActionChains.ToList().Select(i => i.NextActionTime).OrderBy(i => i).First() : double.MaxValue;
        }

        public void RunActions()
        {
            foreach (var actionChain in ActionChains.ToList())
            {
                if (!actionChain.RunActions())
                    ActionChains.Remove(actionChain);
            }
            GetNextActionTime();
        }
    }
}
