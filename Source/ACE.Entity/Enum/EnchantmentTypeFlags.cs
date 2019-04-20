using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// These flags are used to determine what enchantments stack.
    /// </summary>
    [Flags]
    public enum EnchantmentTypeFlags
    {
        Undef                  = 0x0000000,
        Attribute              = 0x0000001,
        SecondAtt              = 0x0000002,
        Int                    = 0x0000004,
        Float                  = 0x0000008,
        Skill                  = 0x0000010,
        BodyDamageValue        = 0x0000020,
        BodyDamageVariance     = 0x0000040,
        BodyArmorValue         = 0x0000080,
        SingleStat             = 0x0001000,
        MultipleStat           = 0x0002000,
        Multiplicative         = 0x0004000,
        Additive               = 0x0008000,
        AttackSkills           = 0x0010000,
        DefenseSkills          = 0x0020000,
        Multiplicative_Degrade = 0x0100000,
        Additive_Degrade       = 0x0200000,
        Vitae                  = 0x0800000,
        Cooldown               = 0x1000000,
        Beneficial             = 0x2000000,
        StatTypes              = 0x00000FF,
    }
}
