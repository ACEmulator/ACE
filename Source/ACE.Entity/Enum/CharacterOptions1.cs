using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is a list of all the options that are sent in the CharacterOptions1 flag<para />
    /// Used with F7B0 0013: GameEvent -> PlayerDescription - To send some of the options (the others are sent in the CharacterOptions2 flag)<para />
    /// Used with F7B1 01A1: GameAction -> Set Character Options - Sent as a flag with the "true" values ORed<para />
    /// /// In the client, this is named CharacterOption.
    /// </summary>
    [Flags]
    public enum CharacterOptions1 : uint
    {
        NotUsed1                                = 0x00000001,
        AutoRepeatAttack                        = 0x00000002,
        IgnoreAllegianceRequests                = 0x00000004,
        IgnoreFellowshipRequests                = 0x00000008,
        NotUsed2                                = 0x00000010,
        NotUsed3                                = 0x00000020,
        AllowGive                               = 0x00000040,
        ViewCombatTarget                        = 0x00000080,
        ShowTooltips                            = 0x00000100,
        UseDeception                            = 0x00000200,
        ToggleRun                               = 0x00000400,
        StayInChatMode                          = 0x00000800,
        AdvancedCombatUI                        = 0x00001000,
        AutoTarget                              = 0x00002000,
        NotUsed4                                = 0x00004000,
        VividTargetingIndicator                 = 0x00008000,
        DisableMostWeatherEffects               = 0x00010000,
        IgnoreTradeRequests                     = 0x00020000,
        FellowshipShareXP                       = 0x00040000,
        AcceptLootPermits                       = 0x00080000,
        FellowshipShareLoot                     = 0x00100000,
        SideBySideVitals                        = 0x00200000,
        CoordinatesOnRadar                      = 0x00400000,
        SpellDuration                           = 0x00800000,
        NotUsed5                                = 0x01000000,
        DisableHouseRestrictionEffects          = 0x02000000,
        DragItemOnPlayerOpensSecureTrade        = 0x04000000,
        DisplayAllegianceLogonNotifications     = 0x08000000,
        UseChargeAttack                         = 0x10000000,
        AutoAcceptFellowRequest                 = 0x20000000,
        HearAllegianceChat                      = 0x40000000,
        UseCraftSuccessDialog                   = 0x80000000,

        Default                                 = AutoRepeatAttack | IgnoreFellowshipRequests | AllowGive | ShowTooltips | ToggleRun | AutoTarget | VividTargetingIndicator | FellowshipShareXP | CoordinatesOnRadar | SpellDuration | UseChargeAttack | HearAllegianceChat // 1355064650
    }
}
