using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// This is a combination of the CharacterOption1 and CharacterOption2 enums. For the client, these are split into two groups because they can't be contained in a single uint field.<para />
    /// Only some of these have values, which is intentional.<para />
    /// Used with F7B1 0005: GameAction -> Set Single Character Option - Only those that have values will trigger that GameAction.
    /// </summary>
    public enum CharacterOption
    {
        [CharacterOptions1(CharacterOptions1.AutoRepeatAttacks)]
        AutoRepeatAttacks                       = 0,

        [CharacterOptions1(CharacterOptions1.IgnoreAllegianceRequests)]
        IgnoreAllegianceRequests                = 1,

        [CharacterOptions1(CharacterOptions1.IgnoreFellowshipRequests)]
        IgnoreFellowshipRequests                = 2,

        [CharacterOptions1(CharacterOptions1.ShareFellowshipExpAndLuminance)]
        ShareFellowshipExpAndLuminance          = 15,

        [CharacterOptions1(CharacterOptions1.AcceptCorpseLootingPermissions)]
        AcceptCorpseLootingPermissions          = 16,

        [CharacterOptions1(CharacterOptions1.ShareFellowshipLoot)]
        ShareFellowshipLoot                     = 17,

        [CharacterOptions1(CharacterOptions1.AutomaticallyAcceptFellowshipRequests)]
        AutomaticallyAcceptFellowshipRequests   = 18,

        [CharacterOptions1(CharacterOptions1.UseChargeAttack)]
        UseChargeAttack                         = 25,

        [CharacterOptions1(CharacterOptions1.ListenToAllegianceChat)]
        ListenToAllegianceChat                  = 27,        

        [CharacterOptions2(CharacterOptions2.ListenToGeneralChat)]
        ListenToGeneralChat                     = 35,

        [CharacterOptions2(CharacterOptions2.ListenToTradeChat)]
        ListenToTradeChat                       = 36,

        [CharacterOptions2(CharacterOptions2.ListenToLFGChat)]
        ListenToLFGChat                         = 37,

        [CharacterOptions2(CharacterOptions2.ListentoRoleplayChat)]
        ListentoRoleplayChat                    = 38,

        [CharacterOptions2(CharacterOptions2.AppearOffline)]
        AppearOffline                           = 39,

        [CharacterOptions2(CharacterOptions2.LeadMissileTargets)]
        LeadMissileTargets                      = 42,

        [CharacterOptions2(CharacterOptions2.UseFastMissiles)]
        UseFastMissiles                         = 43,

        [CharacterOptions2(CharacterOptions2.ListenToSocietyChat)]
        ListenToSocietyChat                     = 46,

        [CharacterOptions2(CharacterOptions2.ShowYourHelmOrHeadGear)]
        ShowYourHelmOrHeadGear                  = 47,

        [CharacterOptions2(CharacterOptions2.ShowYourCloak)]
        ShowYourCloak                           = 50,

        [CharacterOptions2(CharacterOptions2.LockUI)]
        LockUI = 51,

        [CharacterOptions2(CharacterOptions2.ListenToPKDeathMessages)]
        ListenToPKDeathMessages                 = 52,

        [CharacterOptions1(CharacterOptions1.KeepCombatTargetsInView)]
        KeepCombatTargetsInView,

        [CharacterOptions1(CharacterOptions1.LetOtherPlayersGiveYouItems)]
        LetOtherPlayersGiveYouItems,

        [CharacterOptions1(CharacterOptions1.Display3dTooltips)]
        Display3dTooltips,

        [CharacterOptions1(CharacterOptions1.AttemptToDeceiveOtherPlayers)]
        AttemptToDeceiveOtherPlayers,

        [CharacterOptions1(CharacterOptions1.RunAsDefaultMovement)]
        RunAsDefaultMovement,

        [CharacterOptions1(CharacterOptions1.StayInChatModeAfterSendingMessage)]
        StayInChatModeAfterSendingMessage,

        [CharacterOptions1(CharacterOptions1.AdvancedCombatInterface)]
        AdvancedCombatInterface,

        [CharacterOptions1(CharacterOptions1.AutoTarget)]
        AutoTarget,

        [CharacterOptions1(CharacterOptions1.VividTargetingIndicator)]
        VividTargetingIndicator,

        [CharacterOptions1(CharacterOptions1.DisableMostWeatherEffects)]
        DisableMostWeatherEffects,

        [CharacterOptions1(CharacterOptions1.IgnoreAllTradeRequests)]
        IgnoreAllTradeRequests,

        [CharacterOptions1(CharacterOptions1.SideBySideVitals)]
        SideBySideVitals,

        [CharacterOptions1(CharacterOptions1.ShowCoordinatesByTheRadar)]
        ShowCoordinatesByTheRadar,

        [CharacterOptions1(CharacterOptions1.DisplaySpellDurations)]
        DisplaySpellDurations,

        [CharacterOptions1(CharacterOptions1.DisableHouseRestrictionEffects)]
        DisableHouseRestrictionEffects,

        [CharacterOptions1(CharacterOptions1.DragItemToPlayerOpensTrade)]
        DragItemToPlayerOpensTrade,

        [CharacterOptions1(CharacterOptions1.ShowAllegianceLogons)]
        ShowAllegianceLogons,

        [CharacterOptions1(CharacterOptions1.UseCraftingChangeOfSuccessDialog)]
        UseCraftingChangeOfSuccessDialog,

        [CharacterOptions2(CharacterOptions2.AlwaysDaylightOutdoors)]
        AlwaysDaylightOutdoors,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourDateOfBirth)]
        AllowOthersToSeeYourDateOfBirth,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourChessRank)]
        AllowOthersToSeeYourChessRank,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourFishingSkill)]
        AllowOthersToSeeYourFishingSkill,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourNumberOfDeaths)]
        AllowOthersToSeeYourNumberOfDeaths,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourAge)]
        AllowOthersToSeeYourAge,

        [CharacterOptions2(CharacterOptions2.DisplayTimestamps)]
        DisplayTimestamps,

        [CharacterOptions2(CharacterOptions2.SalvageMultipleMaterialsAtOnce)]
        SalvageMultipleMaterialsAtOnce,

        [CharacterOptions2(CharacterOptions2.AllowOthersToSeeYourNumberOfTitles)]
        AllowOthersToSeeYourNumberOfTitles,

        [CharacterOptions2(CharacterOptions2.UseMainPackAsDefaultForPickingUpItems)]
        UseMainPackAsDefaultForPickingUpItems,

        [CharacterOptions2(CharacterOptions2.FilterLanguage)]
        FilterLanguage,

        [CharacterOptions2(CharacterOptions2.ConfirmUseOfRareGems)]
        ConfirmUseOfRareGems,

        [CharacterOptions2(CharacterOptions2.DisableDistanceFog)]
        DisableDistanceFog,

        [CharacterOptions2(CharacterOptions2.UseMouseTurning)]
        UseMouseTurning
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
