using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum CombatMode
    {
        Undef       = 0x00,
        NonCombat   = 0x01,
        Melee       = 0x02,
        Missile     = 0x04,
        Magic       = 0x08,

        ValidCombat     = NonCombat | Melee | Missile | Magic,
        CombatCombat    = Melee | Missile | Magic
    }
}
