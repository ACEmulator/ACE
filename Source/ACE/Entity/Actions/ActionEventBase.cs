using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public abstract class ActionEventBase : IAction
    {
        protected Tuple<IActor, IAction> nextAct;

        public ActionEventBase()
        {
            nextAct = null;
        }

        // Cannot be accessed by concurrent threads
        public virtual Tuple<IActor, IAction> Act()
        {
            var ret = nextAct;
            return ret;
        }

        // Cannot be accessed by concurrent threads
        public void RunOnFinish(IActor nextActor, IAction nextAction)
        {
            nextAct = new Tuple<IActor, IAction>(nextActor, nextAction);
        }
    }
}
