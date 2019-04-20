using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum EnchantmentMask
    {
        LifeSpells      = 0x1,
        CreatureSpells  = 0x2,
        Vitae           = 0x4,
        Cooldown        = 0x8,
    };
}
