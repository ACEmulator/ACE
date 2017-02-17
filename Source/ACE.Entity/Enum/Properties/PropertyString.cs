
namespace ACE.Entity.Enum.Properties
{
    public enum PropertyString : ushort
    {
        Undef,
        Name,
        [PersistedProperty(true, typeof(Character), "Adventurer")]
        Title,
        Sex,
        HeritageGroup,
        Template,
        AttackersName,
        Inscription,
        ScribeName,
        VendorsName,
        Fellowship,
        [PersistedProperty(true, typeof(Character), null)]
        MonarchsName,
        LockCode,
        KeyCode,
        Use,
        ShortDesc,
        LongDesc,
        ActivationTalk,
        UseMessage,
        ItemHeritageGroupRestriction,
        PluralName,
        MonarchsTitle,
        ActivationFailure,
        ScribeAccount,
        TownName,
        CraftsmanName,
        UsePkServerError,
        ScoreCachedText,
        ScoreDefaultEntryFormat,
        ScoreFirstEntryFormat,
        ScoreLastEntryFormat,
        ScoreOnlyEntryFormat,
        ScoreNoEntry,
        Quest,
        GeneratorEvent,
        PatronsTitle,
        HouseOwnerName,
        QuestRestriction,
        AppraisalPortalDestination,
        TinkerName,
        ImbuerName,
        HouseOwnerAccount,
        [PersistedProperty(true, typeof(Character), "[ Name ]")]
        DisplayName,
        DateOfBirth,
        ThirdPartyApi,
        KillQuest,
        Afk,
        AllegianceName,
        AugmentationAddQuest,
        KillQuest2,
        KillQuest3,
        UseSendsSignal,
        GearPlatingName,
        Count
    }

    public static class PropertyStringExtensions
    {
        public static PersistedPropertyAttribute GetPersistedPropertyAttribute(this PropertyString val)
        {
            return Enum.EnumHelper.GetAttributeOfType<PersistedPropertyAttribute>(val);
        }
    }
}
