using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // No properties are sent to the client unless they featured an attribute.
    // SendOnLogin gets sent to players in the PlayerDescription event
    // AssessmentProperty gets sent in successful appraisal
    public enum PropertyString : ushort
    {
        Undef                          = 0,
        [SendOnLogin]
        Name                           = 1,
        /// <summary>
        /// default "Adventurer"
        /// </summary
        Title                          = 2,
        Sex                            = 3,
        HeritageGroup                  = 4,
        [SendOnLogin][AssessmentProperty]
        Template                       = 5,
        AttackersName                  = 6,
        [AssessmentProperty]
        Inscription                    = 7,
        [AssessmentProperty]
        ScribeName                     = 8,
        VendorsName                    = 9,
        [AssessmentProperty]
        Fellowship                     = 10,
        MonarchsName                   = 11,
        LockCode                       = 12,
        KeyCode                        = 13,
        [AssessmentProperty]
        Use                            = 14,
        [AssessmentProperty]
        ShortDesc                      = 15,
        [AssessmentProperty]
        LongDesc                       = 16,
        ActivationTalk                 = 17,
        UseMessage                     = 18,
        ItemHeritageGroupRestriction   = 19,
        PluralName                     = 20,
        [AssessmentProperty]
        MonarchsTitle                  = 21,
        ActivationFailure              = 22,
        [AssessmentProperty]
        ScribeAccount                  = 23,
        TownName                       = 24,
        [AssessmentProperty]
        CraftsmanName                  = 25,
        UsePkServerError               = 26,
        ScoreCachedText                = 27,
        ScoreDefaultEntryFormat        = 28,
        ScoreFirstEntryFormat          = 29,
        ScoreLastEntryFormat           = 30,
        ScoreOnlyEntryFormat           = 31,
        ScoreNoEntry                   = 32,
        Quest                          = 33,
        GeneratorEvent                 = 34,
        [AssessmentProperty]
        PatronsTitle                   = 35,
        HouseOwnerName                 = 36,
        QuestRestriction               = 37,
        [AssessmentProperty]
        AppraisalPortalDestination     = 38,
        [AssessmentProperty]
        TinkerName                     = 39,
        [AssessmentProperty]
        ImbuerName                     = 40,
        HouseOwnerAccount              = 41,
        DisplayName                    = 42,
        [AssessmentProperty]
        DateOfBirth                    = 43,
        ThirdPartyApi                  = 44,
        KillQuest                      = 45,
        [Ephemeral]
        Afk                            = 46,
        [AssessmentProperty]
        AllegianceName                 = 47,
        AugmentationAddQuest           = 48,
        KillQuest2                     = 49,
        KillQuest3                     = 50,
        UseSendsSignal                 = 51,
        [AssessmentProperty]
        GearPlatingName                = 52,

        /* Custom Properties */
        PCAPRecordedCurrentMotionState = 8006,
        PCAPRecordedServerName         = 8031,
        PCAPRecordedCharacterName      = 8032,

        AllegianceMotd                 = 9001,
        AllegianceMotdSetBy            = 9002,
        AllegianceSpeakerTitle         = 9003,
        AllegianceSeneschalTitle       = 9004,
        AllegianceCastellanTitle       = 9005,
        GodState                       = 9006,
        TinkerLog                      = 9007,
    }
}
