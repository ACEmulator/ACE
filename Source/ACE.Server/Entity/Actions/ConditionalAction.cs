﻿using System;

namespace ACE.Server.Entity.Actions
{
    public class ConditionalAction : IAction
    {
        public ActionChain TrueChain { get; private set; }
        public ActionChain FalseChain { get; private set; }
        public Func<bool> Condition { get; private set; }

        public ConditionalAction(Func<bool> condition, ActionChain trueChain, ActionChain falseChain)
        {
            TrueChain = trueChain;
            FalseChain = falseChain;
        }

        public void RunOnFinish(IActor actor, IAction action)
        {
            TrueChain.AddAction(actor, action);
            FalseChain.AddAction(actor, action);
        }

        public Tuple<IActor, IAction> Act()
        {
            bool cond = Condition();

            if (cond)
            {
                return new Tuple<IActor, IAction>(TrueChain.FirstElement.Actor, TrueChain.FirstElement.Action);
            }
            else
            {
                return new Tuple<IActor, IAction>(FalseChain.FirstElement.Actor, FalseChain.FirstElement.Action);
            }
        }
    }
}
