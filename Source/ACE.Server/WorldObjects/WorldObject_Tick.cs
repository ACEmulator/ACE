using System.Collections.Generic;

using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        public virtual void Tick(double lastTickDuration, long currentTimeTick)
        {
            actionQueue.RunActions();
        }

        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        public void RunActions()
        {
            actionQueue.RunActions();
        }

        /// <summary>
        /// Prepare new action to run on this object
        /// </summary>
        public LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            return actionQueue.EnqueueAction(action);
        }

        /// <summary>
        /// Satisfies action interface
        /// </summary>
        /// <param name="node"></param>
        public void DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }
    }
}
