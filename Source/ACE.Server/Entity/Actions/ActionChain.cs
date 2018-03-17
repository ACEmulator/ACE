using System;

using ACE.Server.Managers;

namespace ACE.Server.Entity.Actions
{
    public class ActionChain
    {
        public class ChainElement
        {
            public IAction Action { get; }
            public IActor Actor { get; }

            public ChainElement(IActor actor, IAction action)
            {
                Actor = actor;
                Action = action;
            }
        }

        public ChainElement FirstElement { get; private set; }
        private ChainElement lastElement;

        public ActionChain()
        {
            FirstElement = null;
            lastElement = null;
        }

        public ActionChain(IActor firstActor, Action firstAction)
        {
            FirstElement = new ChainElement(firstActor, new ActionEventDelegate(firstAction));
            lastElement = FirstElement;
        }

        public ActionChain(IActor firstActor, IAction firstAction)
        {
            FirstElement = new ChainElement(firstActor, firstAction);
            lastElement = FirstElement;
        }

        public ActionChain(ChainElement elm)
        {
            FirstElement = elm;
            lastElement = FirstElement;
        }

        public void AddAction(IActor actor, Action action)
        {
            ChainElement newElm = new ChainElement(actor, new ActionEventDelegate(action));
            AddAction(newElm);
        }

        public void AddAction(IActor actor, IAction action)
        {
            ChainElement newElm = new ChainElement(actor, action);
            AddAction(newElm);
        }

        public void AddAction(ChainElement elm)
        {
            if (FirstElement == null)
            {
                FirstElement = elm;
                lastElement = elm;
            }
            else
            {
                lastElement.Action.RunOnFinish(elm.Actor, elm.Action);
                lastElement = elm;
            }
        }

        public void AddChain(ActionChain chain)
        {
            if (chain != null)
            {
                // If we have a chain of our own
                if (lastElement != null)
                {
                    lastElement.Action.RunOnFinish(chain.FirstElement.Actor, chain.FirstElement.Action);
                    lastElement = chain.lastElement;
                }
                // If we're uninit'd, take their data
                else
                {
                    FirstElement = chain.FirstElement;
                    lastElement = chain.lastElement;
                }
            }
        }

        public void AddBranch(IActor conditionActor, Func<bool> condition, ChainElement trueBranch, ChainElement falseBranch)
        {
            AddBranch(conditionActor, condition, new ActionChain(trueBranch), new ActionChain(falseBranch));
        }

        public void AddBranch(IActor conditionActor, Func<bool> condition, ActionChain trueBranch, ActionChain falseBranch)
        {
            AddAction(new ChainElement(conditionActor, new ConditionalAction(condition, trueBranch, falseBranch)));
        }

        public void AddLoop(IActor conditionActor, Func<bool> condition, ActionChain body)
        {
            AddAction(new ChainElement(conditionActor, new LoopAction(conditionActor, condition, body)));
        }

        public void AddDelaySeconds(double timeInSeconds)
        {
            AddAction(WorldManager.DelayManager, new DelayAction(WorldManager.SecondsToTicks(timeInSeconds)));
        }

        public void AddDelayTicks(double timeInTicks)
        {
            AddAction(WorldManager.DelayManager, new DelayAction(timeInTicks));
        }

        public void EnqueueChain()
        {
            FirstElement.Actor.EnqueueAction(FirstElement.Action);
        }
    }
}
