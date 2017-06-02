using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
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
