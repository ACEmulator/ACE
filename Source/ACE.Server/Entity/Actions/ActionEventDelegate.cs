using System;

namespace ACE.Server.Entity.Actions
{
    public class ActionEventDelegate : ActionEventBase
    {
        private Action action;

        public ActionEventDelegate(Action action) : base()
        {
            this.action = action;
        }

        public override Tuple<IActor, IAction> Act() {
            action();

            return base.Act();
        }
    }
}
