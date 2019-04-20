using System;

namespace ACE.Server.Entity.Actions
{
    public class ActionEventDelegate : ActionEventBase
    {
        private readonly Action action;

        public ActionEventDelegate(Action action)
        {
            this.action = action;
        }

        public override Tuple<IActor, IAction> Act()
        {
            action();

            return base.Act();
        }
    }
}
