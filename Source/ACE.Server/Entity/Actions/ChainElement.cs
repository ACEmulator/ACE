using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Entity.Actions
{
    public class ChainElement
    {
        public Action Action;
        public double RunTime;

        public ChainElement(Action action, double runTime)
        {
            Action = action;
            RunTime = runTime;
        }
    }
}
