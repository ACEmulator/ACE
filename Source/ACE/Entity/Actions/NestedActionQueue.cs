using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public class NestedActionQueue : ActionQueue, IAction
    {
        private IActor parent;
        private bool enqueued;
        private Tuple<IActor, IAction> nextAct;

        public NestedActionQueue(IActor parent)
        {
            this.parent = parent;
            enqueued = false;
            nextAct = null;
        }

        public override void EnqueueAction(IAction action)
        {
            lock (queueLock)
            {
                queue.Add(action);

                if (enqueued == false)
                {
                    parent.EnqueueAction(this);
                    enqueued = true;
                }
            }
        }

        public Tuple<IActor, IAction> Act()
        {
            RunActions();

            return nextAct;
        }

        public void RunOnFinish(IActor nextActor, IAction nextAction)
        {
            nextAct = new Tuple<IActor, IAction>(nextActor, nextAction);
        }
    }
}
