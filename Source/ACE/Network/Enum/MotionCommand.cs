using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Enum
{
    public enum MotionCommand
    {
        MotionInvalid = 0,
        Run = 7,
        NonCombat = 61,
        FistJump = 76,
        ShakeFist = 121,
        Bow = 125,
        Logout1 = 286,
        Logout2 = 885
    }
}
