using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum EnchantmentMask
    {
        Multiplicative  = 0x1,
        Additive        = 0x2,
        Vitae           = 0x4,
        Cooldown        = 0x8,
    };
}
