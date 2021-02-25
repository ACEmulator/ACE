using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is a list of all the options that are sent in the CharacterOptions2 flag
    /// Used with F7B0 0013: GameEvent -> PlayerDescription - To send some of the options (the others are sent in the CharacterOptions1 flag)
    /// Used with F7B1 01A1: GameAction -> Set Character Options - Sent as a flag with the "true" values ORed
    /// </summary>
    [Flags]
    public enum CharacterOptions2 : uint
    {
        AlwaysDaylightOutdoors                  = 0x00000001,
        AllowOthersToSeeYourDateOfBirth         = 0x00000002,
        AllowOthersToSeeYourChessRank           = 0x00000004,
        AllowOthersToSeeYourFishingSkill        = 0x00000008,
        AllowOthersToSeeYourNumberOfDeaths      = 0x00000010,
        AllowOthersToSeeYourAge                 = 0x00000020,
        DisplayTimestamps                       = 0x00000040,
        SalvageMultipleMaterialsAtOnce          = 0x00000080,
        ListenToGeneralChat                     = 0x00000100,
        ListenToTradeChat                       = 0x00000200,
        ListenToLFGChat                         = 0x00000400,
        ListenToRoleplayChat                    = 0x00000800,        
        AppearOffline                           = 0x00001000,
        AllowOthersToSeeYourNumberOfTitles      = 0x00002000,
        UseMainPackAsDefaultForPickingUpItems   = 0x00004000,
        LeadMissileTargets                      = 0x00008000,
        UseFastMissiles                         = 0x00010000,
        FilterLanguage                          = 0x00020000,
        ConfirmUseOfRareGems                    = 0x00040000,
        ListenToSocietyChat                     = 0x00080000,
        ShowYourHelmOrHeadGear                  = 0x00100000,
        DisableDistanceFog                      = 0x00200000,
        UseMouseTurning                         = 0x00400000, // Not sure how to verify this in client, but this is what aclogview extracted from PDB.
        ShowYourCloak                           = 0x00800000,
        LockUI                                  = 0x01000000,
        ListenToPKDeathMessages                 = 0x02000000,
        NotUsed1                                = 0x04000000,
        NotUsed2                                = 0x08000000,
        NotUsed3                                = 0x10000000,
        NotUsed4                                = 0x20000000,
        NotUsed5                                = 0x40000000,
        NotUsed6                                = 0x80000000
    }
}
