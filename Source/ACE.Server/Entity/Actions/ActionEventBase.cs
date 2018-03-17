using System;

namespace ACE.Server.Entity.Actions
{
    public abstract class ActionEventBase : IAction
    {
        protected Tuple<IActor, IAction> NextAct { get; private set; }

        protected ActionEventBase()
        {
            NextAct = null;
        }

        // Cannot be accessed by concurrent threads
        public virtual Tuple<IActor, IAction> Act()
        {
            var ret = NextAct;
            return ret;
        }

        // Cannot be accessed by concurrent threads
        public void RunOnFinish(IActor nextActor, IAction nextAction)
        {
            NextAct = new Tuple<IActor, IAction>(nextActor, nextAction);
        }
    }
}
