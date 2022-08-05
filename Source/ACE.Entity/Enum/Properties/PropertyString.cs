using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyString : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi
        // description attributes are used by the weenie editor for a cleaner display name
        Undef                           = 0,
        [SendOnLogin]
        Name                            = 1,
        /// <summary>
        /// default "Adventurer"
        /// </summary>
        Title                           = 2,
        Sex                             = 3,
        HeritageGroup                   = 4,
        Template                        = 5,
        AttackersName                   = 6,
        Inscription                     = 7,
        [Description("Scribe Name")]
        ScribeName                      = 8,
        VendorsName                     = 9,
        Fellowship                      = 10,
        MonarchsName                    = 11,
        [ServerOnly]
        LockCode                        = 12,
        [ServerOnly]
        KeyCode                         = 13,
        Use                             = 14,
        ShortDesc                       = 15,
        LongDesc                        = 16,
        ActivationTalk                  = 17,
        [ServerOnly]
        UseMessage                      = 18,
        ItemHeritageGroupRestriction    = 19,
        PluralName                      = 20,
        MonarchsTitle                   = 21,
        ActivationFailure               = 22,
        ScribeAccount                   = 23,
        TownName                        = 24,
        CraftsmanName                   = 25,
        UsePkServerError                = 26,
        ScoreCachedText                 = 27,
        ScoreDefaultEntryFormat         = 28,
        ScoreFirstEntryFormat           = 29,
        ScoreLastEntryFormat            = 30,
        ScoreOnlyEntryFormat            = 31,
        ScoreNoEntry                    = 32,
        [ServerOnly]
        Quest                           = 33,
        GeneratorEvent                  = 34,
        PatronsTitle                    = 35,
        HouseOwnerName                  = 36,
        QuestRestriction                = 37,
        AppraisalPortalDestination      = 38,
        TinkerName                      = 39,
        ImbuerName                      = 40,
        HouseOwnerAccount               = 41,
        DisplayName                     = 42,
        DateOfBirth                     = 43,
        ThirdPartyApi                   = 44,
        KillQuest                       = 45,
        [Ephemeral]
        Afk                             = 46,
        AllegianceName                  = 47,
        AugmentationAddQuest            = 48,
        KillQuest2                      = 49,
        KillQuest3                      = 50,
        UseSendsSignal                  = 51,

        [Description("Gear Plating Name")]
        GearPlatingName                 = 52,

        [ServerOnly]
        PCAPRecordedCurrentMotionState  = 8006,
        [ServerOnly]
        PCAPRecordedServerName          = 8031,
        [ServerOnly]
        PCAPRecordedCharacterName       = 8032,

        /* custom */
        [ServerOnly]
        AllegianceMotd                  = 9001,
        [ServerOnly]
        AllegianceMotdSetBy             = 9002,
        [ServerOnly]
        AllegianceSpeakerTitle          = 9003,
        [ServerOnly]
        AllegianceSeneschalTitle        = 9004,
        [ServerOnly]
        AllegianceCastellanTitle        = 9005,
        [ServerOnly]
        GodState                        = 9006,
        [ServerOnly]
        TinkerLog                       = 9007,
    }

    public static class PropertyStringExtensions
    {
        public static string GetDescription(this PropertyString prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }
    }
}
