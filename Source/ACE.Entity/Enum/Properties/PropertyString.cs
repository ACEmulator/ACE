using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyString : ushort
    {
        // properties marked as ServerOnly are properies we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi

        // description attributes are used by the weenie editor for a cleaner display name

        [ServerOnly]
        Undef                           = 0,
        Name                            = 1,

        /// <summary>
        /// default "Adventurer"
        /// </summary>
        [ServerOnly]
        Title                           = 2,
        [ServerOnly]
        Sex                             = 3,
        [ServerOnly]
        HeritageGroup                   = 4,
        Template                        = 5,
        [ServerOnly]
        AttackersName                   = 6,
        Inscription                     = 7,

        [Description("Scribe Name")]
        ScribeName                      = 8,
        [ServerOnly]
        VendorsName                     = 9,
        [ServerOnly]
        Fellowship                      = 10,
        [ServerOnly]
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
        [ServerOnly]
        ItemHeritageGroupRestriction    = 19,
        PluralName                      = 20,
        MonarchsTitle                   = 21,
        [ServerOnly]
        ActivationFailure               = 22,
        [ServerOnly]
        ScribeAccount                   = 23,
        [ServerOnly]
        TownName                        = 24,
        CraftsmanName                   = 25,
        [ServerOnly]
        UsePkServerError                = 26,
        [ServerOnly]
        ScoreCachedText                 = 27,
        [ServerOnly]
        ScoreDefaultEntryFormat         = 28,
        [ServerOnly]
        ScoreFirstEntryFormat           = 29,
        [ServerOnly]
        ScoreLastEntryFormat            = 30,
        [ServerOnly]
        ScoreOnlyEntryFormat            = 31,
        [ServerOnly]
        ScoreNoEntry                    = 32,
        [ServerOnly]
        Quest                           = 33,
        [ServerOnly]
        GeneratorEvent                  = 34,
        PatronsTitle                    = 35,
        HouseOwnerName                  = 36,
        [ServerOnly]
        QuestRestriction                = 37,
        AppraisalPortalDestination      = 38,
        TinkerName                      = 39,
        ImbuerName                      = 40,
        [ServerOnly]
        HouseOwnerAccount               = 41,
        [ServerOnly]
        DisplayName                     = 42,
        DateOfBirth                     = 43,
        [ServerOnly]
        ThirdPartyApi                   = 44,
        [ServerOnly]
        KillQuest                       = 45,
        [ServerOnly]
        Afk                             = 46,
        AllegianceName                  = 47,
        [ServerOnly]
        AugmentationAddQuest            = 48,
        [ServerOnly]
        KillQuest2                      = 49,
        [ServerOnly]
        KillQuest3                      = 50,
        [ServerOnly]
        UseSendsSignal                  = 51,
        GearPlatingName                 = 52

        // values over 9000 are ones that we have added and should not be sent to the client
    }
}
