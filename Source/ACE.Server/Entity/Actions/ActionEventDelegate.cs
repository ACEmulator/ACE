using System;

namespace ACE.Server.Entity.Actions
{
    public class ActionEventDelegate : ActionEventBase
    {
        public readonly Action Action;

        public ActionEventDelegate(Action action)
        {
            Action = action;
        }

        public override Tuple<IActor, IAction> Act()
        {
            Action();

            return base.Act();
        }
    }
}
