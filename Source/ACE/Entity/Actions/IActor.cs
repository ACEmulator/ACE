using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
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
