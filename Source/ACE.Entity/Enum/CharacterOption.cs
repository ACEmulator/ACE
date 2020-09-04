using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is a combination of the CharacterOption1 and CharacterOption2 enums. For the client, these are split into two groups because they can't be contained in a single uint field.<para />
    /// Only some of these have values, which is intentional.<para />
    /// Used with F7B1 0005: GameAction -> Set Single Character Option - Only those that have values will trigger that GameAction.<para />
    /// In the client, this is named PlayerOption.
    /// </summary>
    public enum CharacterOption
    {
        [CharacterOptions1(CharacterOptions1.AutoRepeatAttacks)]
        AutoRepeatAttacks                       = 0x00, // AutoRepeatAttack_PlayerOption

        [CharacterOptions1(CharacterOptions1.IgnoreAllegianceRequests)]
        IgnoreAllegianceRequests                = 0x01, // IgnoreAllegianceRequests_PlayerOption

        [CharacterOptions1(CharacterOptions1.IgnoreFellowshipRequests)]
        IgnoreFellowshipRequests                = 0x02, // IgnoreFellowshipRequests_PlayerOption

        [CharacterOptions1(CharacterOptions1.IgnoreAllTradeRequests)]
        IgnoreAllTradeRequests                  = 0x03, // IgnoreTradeRequests_PlayerOption

        [CharacterOptions1(CharacterOptions1.DisableMostWeatherEffects)]
        DisableMostWeatherEffects               = 0x04, // DisableMostWeatherEffects_PlayerOption

        [CharacterOptions2(CharacterOptions2.AlwaysDaylightOutdoors)]
        AlwaysDaylightOutdoors                  = 0x05, // PersistentAtDay_PlayerOption

        [CharacterOptions1(CharacterOptions1.LetOtherPlayersGiveYouItems)]
        LetOtherPlayersGiveYouItems             = 0x06, // AllowGive_PlayerOption

        [CharacterOptions1(CharacterOptions1.KeepCombatTargetsInView)]
        KeepCombatTargetsInView                 = 0x07, // ViewCombatTarget_PlayerOption

        [CharacterOptions1(CharacterOptions1.Display3dTooltips)]
        Display3dTooltips                       = 0x08, // ShowTooltips_PlayerOption

        [CharacterOptions1(CharacterOptions1.AttemptToDeceiveOtherPlayers)]
        AttemptToDeceiveOtherPlayers            = 0x09, // UseDeception_PlayerOption

        [CharacterOptions1(CharacterOptions1.RunAsDefaultMovement)]
        RunAsDefaultMovement                    = 0x0A, // ToggleRun_PlayerOption

        [CharacterOptions1(CharacterOptions1.StayInChatModeAfterSendingMessage)]
        StayInChatModeAfterSendingMessage       = 0x0B, // StayInChatMode_PlayerOption

        [CharacterOptions1(CharacterOptions1.AdvancedCombatInterface)]
        AdvancedCombatInterface                 = 0x0C, // AdvancedCombatUI_PlayerOption

        [CharacterOptions1(CharacterOptions1.AutoTarget)]
        AutoTarget                              = 0x0D, // AutoTarget_PlayerOption

        [CharacterOptions1(CharacterOptions1.VividTargetingIndicator)]
        VividTargetingIndicator                 = 0x0E, // VividTargetingIndicator_PlayerOption

        [CharacterOptions1(CharacterOptions1.ShareFellowshipExpAndLuminance)]
        ShareFellowshipExpAndLuminance          = 0x0F, // FellowshipShareXP_PlayerOption

        [CharacterOptions1(CharacterOptions1.AcceptCorpseLootingPermissions)]
        AcceptCorpseLootingPermissions          = 0x10, // AcceptLootPermits_PlayerOption

        [CharacterOptions1(CharacterOptions1.ShareFellowshipLoot)]
        ShareFellowshipLoot                     = 0x11, // FellowshipShareLoot_PlayerOption

        [CharacterOptions1(CharacterOptions1.AutomaticallyAcceptFellowshipRequests)]
        AutomaticallyAcceptFellowshipRequests   = 0x12, // FellowshipAutoAcceptRequests_PlayerOption

        [CharacterOptions1(CharacterOptions1.SideBySideVitals)]
        SideBySideVitals                        = 0x13, // SideBySideVitals_PlayerOption

        [CharacterOptions1(CharacterOptions1.ShowCoordinatesByTheRadar)]
        ShowCoordinatesByTheRadar               = 0x14, // CoordinatesOnRadar_PlayerOption

        [CharacterOptions1(CharacterOptions1.DisplaySpellDurations)]
        DisplaySpellDurations                   = 0x15, // SpellDuration_PlayerOption

        [CharacterOptions1(CharacterOptions1.DisableHouseRestrictionEffects)]
        DisableHouseRestrictionEffects          = 0x16, // DisableHouseRestrictionEffects_PlayerOption

        [CharacterOptions1(CharacterOptions1.DragItemToPlayerOpensTrade)]
        DragItemToPlayerOpensTrade              = 0x17, // DragItemOnPlayerOpensSecureTrade_PlayerOption

        [CharacterOptions1(CharacterOptions1.ShowAllegianceLogons)]
        ShowAllegianceLogons                    = 0x18, // DisplayAllegianceLogonNotifications_PlayerOption

        [CharacterOptions1(CharacterOptions1.UseChargeAttack)]
        UseChargeAttack                         = 0x19, // UseChargeAttack_PlayerOption

        [CharacterOptions1(CharacterOptions1.UseCraftingChanceOfSuccessDialog)]
        UseCraftingChanceOfSuccessDialog        = 0x1A, // UseCraftSuccessDialog_PlayerOption

        [CharacterOptions1(CharacterOptions1.ListenToAllegianceChat)]
        ListenToAllegianceChat                  = 0x1B, // HearAllegianceChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourDateOfBirth)]
        AllowOthersToSeeYourDateOfBirth         = 0x1C, // DisplayDateOfBirth_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourAge)]
        AllowOthersToSeeYourAge                 = 0x1D, // DisplayAge_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourChessRank)]
        AllowOthersToSeeYourChessRank           = 0x1E, // DisplayChessRank_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourFishingSkill)]
        AllowOthersToSeeYourFishingSkill        = 0x1F, // DisplayFishingSkill_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourNumberOfDeaths)]
        AllowOthersToSeeYourNumberOfDeaths      = 0x20, // DisplayNumberDeaths_PlayerOption

        [CharacterOptions2(CharacterOptions2.DisplayTimestamps)]
        DisplayTimestamps                       = 0x21, // DisplayTimeStamps_PlayerOption

        [CharacterOptions2(CharacterOptions2.SalvageMultipleMaterialsAtOnce)]
        SalvageMultipleMaterialsAtOnce          = 0x22, // SalvageMultiple_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToGeneralChat)]
        ListenToGeneralChat                     = 0x23, // HearGeneralChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToTradeChat)]
        ListenToTradeChat                       = 0x24, // HearTradeChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToLFGChat)]
        ListenToLFGChat                         = 0x25, // HearLFGChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToRoleplayChat)]
        ListenToRoleplayChat                    = 0x26, // HearRoleplayChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.AppearOffline)]
        AppearOffline                           = 0x27, // AppearOffline_PlayerOption

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourNumberOfTitles)]
        AllowOthersToSeeYourNumberOfTitles      = 0x28, // DisplayNumberCharacterTitles_PlayerOption

        [CharacterOptions2(CharacterOptions2.UseMainPackAsDefaultForPickingUpItems)]
        UseMainPackAsDefaultForPickingUpItems   = 0x29, // MainPackPreferred_PlayerOption

        [CharacterOptions2(CharacterOptions2.LeadMissileTargets)]
        LeadMissileTargets                      = 0x2A, // LeadMissileTargets_PlayerOption

        [CharacterOptions2(CharacterOptions2.UseFastMissiles)]
        UseFastMissiles                         = 0x2B, // UseFastMissiles_PlayerOption

        [CharacterOptions2(CharacterOptions2.FilterLanguage)]
        FilterLanguage                          = 0x2C, // FilterLanguage_PlayerOption

        [CharacterOptions2(CharacterOptions2.ConfirmUseOfRareGems)]
        ConfirmUseOfRareGems                    = 0x2D, // ConfirmVolatileRareUse_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToSocietyChat)]
        ListenToSocietyChat                     = 0x2E, // HearSocietyChat_PlayerOption

        [CharacterOptions2(CharacterOptions2.ShowYourHelmOrHeadGear)]
        ShowYourHelmOrHeadGear                  = 0x2F, // ShowHelm_PlayerOption

        [CharacterOptions2(CharacterOptions2.DisableDistanceFog)]
        DisableDistanceFog                      = 0x30, // DisableDistanceFog_PlayerOption

        [CharacterOptions2(CharacterOptions2.UseMouseTurning)]
        UseMouseTurning                         = 0x31, // UseMouseTurning_PlayerOption

        [CharacterOptions2(CharacterOptions2.ShowYourCloak)]
        ShowYourCloak                           = 0x32, // ShowCloak_PlayerOption

        [CharacterOptions2(CharacterOptions2.LockUI)]
        LockUI                                  = 0x33, // LockUI_PlayerOption

        [CharacterOptions2(CharacterOptions2.ListenToPKDeathMessages)]
        ListenToPKDeathMessages                 = 0x34, // HearPKDeath_PlayerOption

    }

    public static class CharacterOptionExtensions
    {
        public static CharacterOptions1Attribute GetCharacterOptions1Attribute(this CharacterOption val)
        {
            return val.GetAttributeOfType<CharacterOptions1Attribute>();
        }

        public static CharacterOptions2Attribute GetCharacterOptions2Attribute(this CharacterOption val)
        {
            return val.GetAttributeOfType<CharacterOptions2Attribute>();
        }

        public static uint GetCharacterOptions1Flag(this ReadOnlyDictionary<CharacterOption, bool> options)
        {
            return GetCharacterOptions1Flag(options.ToDictionary(k => k.Key, v => v.Value));
        }

        public static uint GetCharacterOptions1Flag(this Dictionary<CharacterOption, bool> options)
        {
            uint flags = 0;
            foreach (var option in options.Where(o => o.Key.GetCharacterOptions1Attribute() != null))
            {
                if (option.Value)
                    flags |= (uint)option.Key.GetCharacterOptions1Attribute().Option;
            }

            return flags;
        }

        public static uint GetCharacterOptions2Flag(this ReadOnlyDictionary<CharacterOption, bool> options)
        {
            return GetCharacterOptions2Flag(options.ToDictionary(k => k.Key, v => v.Value));
        }

        public static uint GetCharacterOptions2Flag(this Dictionary<CharacterOption, bool> options)
        {
            uint flags = 0;
            foreach (var option in options.Where(o => o.Key.GetCharacterOptions2Attribute() != null))
            {
                if (option.Value)
                    flags |= (uint)option.Key.GetCharacterOptions2Attribute().Option;
            }

            return flags;
        }
    }
}
