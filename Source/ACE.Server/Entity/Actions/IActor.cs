using System.Collections.Generic;

namespace ACE.Server.Entity.Actions
{
    public interface IActor
    {
        void RunActions();

        // Returns the next action to be run
        LinkedListNode<IAction> EnqueueAction(IAction action);

        // Not thread safe
        void DequeueAction(LinkedListNode<IAction> node);
    }
}
