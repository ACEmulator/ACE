using System;
using System.Collections.Generic;

namespace ACE.Server.Entity.Actions
{
    public class NestedActionQueue : ActionQueue, IAction
    {
        private IActor parent;
        private LinkedListNode<IAction> node;
        private bool enqueued;
        private Tuple<IActor, IAction> nextAct;

        public NestedActionQueue()
        {
            enqueued = false;
            nextAct = null;
        }

        public NestedActionQueue(IActor parent)
        {
            this.parent = parent;
            enqueued = false;
            nextAct = null;
        }

        /// <summary>
        /// NOTE: NOT thread-safe w.r.t. addParent, action queue running
        /// Should /ONLY/ be run when old parent cannot be running
        /// </summary>
        public void RemoveParent()
        {
            lock (queueLock)
            {
                if (enqueued && parent != null)
                {
                    parent.DequeueAction(node);
                }
                parent = null;
                node = null;
            }
        }

        /// <summary>
        /// NOT Thread safe
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(IActor parent)
        {
            lock (queueLock)
            {
                this.parent = parent;
                if (enqueued && parent != null)
                {
                    node = parent.EnqueueAction(this);
                }
            }
        }

        /// <summary>
        /// Thread safe
        /// Enqueues an action in our actionQueue, and automatically enqueues our NestedActionQueue in its parent
        ///    (If the parent is a NestedActionQueue this process repeats -- woot)
        /// </summary>
        /// <param name="action"></param>
        public override LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            bool needEnqueue = false;
            LinkedListNode<IAction> ret;
            lock (queueLock)
            {
                ret = queue.AddLast(action);

                if (enqueued == false)
                {
                    enqueued = true;
                    needEnqueue = parent != null;
                }
            }

            // Has to be done after I unlock to avoid cyclical dependencies
            // The race between adding an action to my queue, and adding an action to their queue is safe
            //   They will eventually run my action, even if more come along, and I will still enqueue only once (enqueued=true is locked)
            if (needEnqueue)
            {
                node = parent.EnqueueAction(this);
            }

            return ret;
        }

        public Tuple<IActor, IAction> Act()
        {
            // We're not in our parent's queue anymore
            enqueued = false;
            node = null;

            RunActions();

            var ret = nextAct;

            return ret;
        }

        /// <summary>
        /// NOT Thread safe, must be set before EnqueueAction
        /// </summary>
        /// <param name="nextActor"></param>
        /// <param name="nextAction"></param>
        public void RunOnFinish(IActor nextActor, IAction nextAction)
        {
            nextAct = new Tuple<IActor, IAction>(nextActor, nextAction);
        }
    }
}
