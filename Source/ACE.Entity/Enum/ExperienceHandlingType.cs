using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ExperienceHandlingType
    {
        Undef                  = 0x0,
        ApplyLevelMod          = 0x1,
        ShareWithFellows       = 0x2,
        AddFellowshipBonus     = 0x4,
        ShareWithAllegiance    = 0x8,
        ApplyToVitae           = 0x10,
        EarnsCP                = 0x20,
        ReducedByDistance      = 0x40,
        Monster                = 0x5F,
        NormalQuest            = 0x1A,
        NoShareQuest           = 0x10,
        PassupQuest            = 0x18,
        ReceivedFromFellowship = 0x18,
        PPEarnedFromUse        = 0x7F,
        AdminRaiseXP           = 0x10,
        AdminRaiseSkillXP      = 0x10,
        ReceivedFromAllegiance = 0x0,
    };
}
