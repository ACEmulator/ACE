using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.StateMachines
{
    public enum ContainerStates : int
    {
        Locked,
        Unlocked,
        InUse,
    }
}