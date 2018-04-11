using System;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum AttackConditions
    {
        CriticalProtectionAugmentation = 0x1,
        Recklessness = 0x2,
        SneakAttack = 0x4
    };
}
