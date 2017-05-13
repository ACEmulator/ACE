namespace ACE.Entity.Enum.Properties
{
    public enum PropertyString : ushort
    {
        Undef                           = 0,
        Name                            = 1,
        [PersistedProperty(true, typeof(Character), "Adventurer")]
        Title                           = 3,
        Sex                             = 4,
        HeritageGroup                   = 5,
        Template                        = 6,
        AttackersName                   = 7,
        Inscription                     = 8,
        ScribeName                      = 9,
        VendorsName                     = 10,
        Fellowship                      = 11,
        [PersistedProperty(true, typeof(Character), null)]
        MonarchsName                    = 12,
        LockCode                        = 13,
        KeyCode                         = 14,
        Use                             = 15,
        ShortDesc                       = 16,
        LongDesc                        = 17,
        ActivationTalk                  = 18,
        UseMessage                      = 19,
        ItemHeritageGroupRestriction    = 20,
        PluralName                      = 21,
        MonarchsTitle                   = 22,
        ActivationFailure               = 23,
        ScribeAccount                   = 24,
        TownName                        = 25,
        CraftsmanName                   = 26,
        UsePkServerError                = 27,
        ScoreCachedText                 = 28,
        ScoreDefaultEntryFormat         = 29,
        ScoreFirstEntryFormat           = 30,
        ScoreLastEntryFormat            = 31,
        ScoreOnlyEntryFormat            = 32,
        ScoreNoEntry                    = 33,
        Quest                           = 34,
        GeneratorEvent                  = 35,
        PatronsTitle                    = 36,
        HouseOwnerName                  = 37,
        QuestRestriction                = 38,
        AppraisalPortalDestination      = 39,
        TinkerName                      = 40,
        ImbuerName                      = 41,
        HouseOwnerAccount               = 42,
        [PersistedProperty(true, typeof(Character), "[ Name ]")]
        DisplayName                     = 43,
        DateOfBirth                     = 44,
        ThirdPartyApi                   = 45,
        KillQuest                       = 46,
        Afk                             = 47,
        AllegianceName                  = 48,
        AugmentationAddQuest            = 49,
        KillQuest2                      = 50,
        KillQuest3                      = 51,
        UseSendsSignal                  = 52,
        GearPlatingName                 = 53,
        Count                           = 54
    }

    public static class PropertyStringExtensions
    {
        public static PersistedPropertyAttribute GetPersistedPropertyAttribute(this PropertyString val)
        {
            return Enum.EnumHelper.GetAttributeOfType<PersistedPropertyAttribute>(val);
        }
    }
}
