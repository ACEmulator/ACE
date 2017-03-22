using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Enum
{
    public enum MotionStance
    {
        UANoShieldAttack = 0x3C,
        Standing = 0x3D,
        MeleeNoShieldAttack = 0x3E,
        BowAttack = 0x3F,
        MeleeShieldAttack = 0x40,
        Spellcasting = 0x49
    }
}
