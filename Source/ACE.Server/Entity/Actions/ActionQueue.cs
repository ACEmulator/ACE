using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Actions
{
    public class ActionQueue
    {
        public List<ActionChain> ActionChains;

        public double NextActionTime;

        public ActionQueue()
        {
            ActionChains = new List<ActionChain>();
        }

        public void EnqueueChain(ActionChain actionChain)
        {
            ActionChains.Add(actionChain);

            GetNextActionTime();
        }

        public void EnqueueAction(WorldObject wo, Action action)
        {
            ActionChains.Add(new ActionChain(wo, action));
        }

        public void EnqueueAction(Action action)
        {
            ActionChains.Add(new ActionChain(action));
        }

        public void GetNextActionTime()
        {
            NextActionTime = ActionChains.Select(i => i.NextActionTime).OrderBy(i => i).First();
        }

        public void RunActions()
        {
            foreach (var actionChain in ActionChains.ToList())
                actionChain.RunActions();
        }
    }
}
