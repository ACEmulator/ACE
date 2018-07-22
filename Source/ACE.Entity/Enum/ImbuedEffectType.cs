using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ImbuedEffectType
    {
        Undef                           = 0,
        CriticalStrike                  = (1 << 0),
        CripplingBlow                   = (1 << 1),
        ArmorRending                    = (1 << 2),
        SlashRending                    = (1 << 3),
        PierceRending                   = (1 << 4),
        BludgeonRending                 = (1 << 5),
        AcidRending                     = (1 << 6),
        ColdRending                     = (1 << 7),
        ElectricRending                 = (1 << 8),
        FireRending                     = (1 << 9),
        MeleeDefense                    = (1 << 10),
        MissileDefense                  = (1 << 11),
        MagicDefense                    = (1 << 12),
        Spellbook                       = (1 << 13),
        NetherRending                   = (1 << 14),

        IgnoreSomeMagicProjectileDamage = (1 << 29),
        AlwaysCritical                  = (1 << 30),
        IgnoreAllArmor                  = (1 << 31)
    }
}
