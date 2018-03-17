using System;

namespace ACE.Server.Entity.Actions
{
    public class LoopAction : ActionEventBase
    {
        public Func<bool> Condition { get; }
        public ActionChain Body { get; }

        public LoopAction(IActor conditionActor, Func<bool> condition, ActionChain body)
        {
            Body = body;
            Condition = condition;
            body.AddAction(conditionActor, this);
        }

        public override Tuple<IActor, IAction> Act()
        {
            if (Condition())
                return new Tuple<IActor, IAction>(Body.FirstElement.Actor, Body.FirstElement.Action);

            return base.Act();
        }
    }
}
