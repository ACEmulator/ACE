using System.Collections.Generic;

namespace ACE.Server.Entity.Actions
{
    public interface IActor
    {
        void RunActions();

        /// <summary>
        /// Returns the next action to be run
        /// </summary>
        LinkedListNode<IAction> EnqueueAction(IAction action);

        /// <summary>
        /// Not thread safe
        /// </summary>
        void DequeueAction(LinkedListNode<IAction> node);
    }
}
