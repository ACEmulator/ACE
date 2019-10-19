using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ExperienceHandlingType
    {
        Undef                  = 0x0,   // 000 0000
        ApplyLevelMod          = 0x1,   // 000 0001
        ShareWithFellows       = 0x2,   // 000 0010
        AddFellowshipBonus     = 0x4,   // 000 0100
        ShareWithAllegiance    = 0x8,   // 000 1000
        ApplyToVitae           = 0x10,  // 001 0000
        EarnsCP                = 0x20,  // 010 0000
        ReducedByDistance      = 0x40,  // 100 0000
        Monster                = 0x5F,  // ReducedByDistance | ApplyToVitae | ShareWithAllegiance | AddFellowshipBonus | ShareWithFellows | ApplyLevelMod
        NormalQuest            = 0x1A,  // ShareWithFellows | ShareWithAllegiance | ApplyToVitae
        NoShareQuest           = 0x10,  // ApplyToVitae
        PassupQuest            = 0x18,  // ApplyToVitae | ShareWithAllegiance
        ReceivedFromFellowship = 0x18,  // ApplyToVitae | ShareWithAllegiance
        PPEarnedFromUse        = 0x7F,  // ReducedByDistance | EarnsCP | ApplyToVitae | ShareWithAllegiance | AddFellowshipBonus | ShareWithFellows | ApplyLevelMod
        AdminRaiseXP           = 0x10,  // ApplyToVitae
        AdminRaiseSkillXP      = 0x10,  // ApplyToVitae
        ReceivedFromAllegiance = 0x0,
    };
}
