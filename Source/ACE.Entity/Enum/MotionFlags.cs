using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MotionFlags
    {
        None = 0x0,
        HasTarget = 0x1,
        Jumping = 0x2 // Needs to be investigated
    }
}
