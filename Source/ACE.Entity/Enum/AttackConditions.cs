using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AttackConditions
    {
        None                           = 0x0,
        CriticalProtectionAugmentation = 0x1,
        Recklessness                   = 0x2,
        SneakAttack                    = 0x4,
        Overpower                      = 0x8
    };
}
