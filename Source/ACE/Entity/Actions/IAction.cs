using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Actions
{
    public interface IAction
    {
        Tuple<IActor, IAction> Act();

        void RunOnFinish(IActor actor, IAction action);
    }
}
