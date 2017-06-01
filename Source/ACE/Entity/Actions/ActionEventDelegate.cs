using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public class ActionEventDelegate : IAction
    {
        private Action action;
        private Tuple<IActor, IAction> nextAct;

        public ActionEventDelegate(Action action)
        {
            this.action = action;
            nextAct = null;
        }

        public Tuple<IActor, IAction> Act() {
            action();
            return nextAct;
        }

        public void RunOnFinish(IActor nextActor, IAction nextAction)
        {
            nextAct = new Tuple<IActor, IAction>(nextActor, nextAction);
        }
    }
}
