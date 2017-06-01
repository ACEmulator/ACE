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
        void EnqueueAction(IAction action);
    }
}
