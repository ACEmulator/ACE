using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public class LoopAction : ActionEventBase
    {
        public Func<bool> Condition { get; private set; }
        public ActionChain Body { get; private set; }

        public LoopAction(IActor conditionActor, Func<bool> condition, ActionChain body)
        {
            Body = body;
            Condition = condition;
            body.AddAction(conditionActor, this);
        }

        public override Tuple<IActor, IAction> Act()
        {
            if (Condition())
            {
                return new Tuple<IActor, IAction>(Body.FirstElement.Actor, Body.FirstElement.Action);
            }

            return base.Act();
        }
    }
}
