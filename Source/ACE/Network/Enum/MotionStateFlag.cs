using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Enum
{
    [Flags]
    public enum MotionStateFlag
    {
        None = 0x0000,
        CurrentHoldKey = 0x0001,
        CurrentStyle = 0x0002,
        ForwardCommand = 0x0004,
        ForwardHoldKey = 0x0008,
        ForwardSpeed = 0x0010,
        SideStepCommand = 0x0020,
        SideStepHoldKey = 0x0040,
        SideStepSpeed = 0x0080,
        TurnCommand = 0x0100,
        TurnHoldKey = 0x0200,
        TurnSpeed = 0x0400
    }
}
