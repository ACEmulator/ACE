namespace ACE.Entity.Enum.Properties
{
    public enum PropertyInt64 : ushort
    {
        Undef = 0,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        TotalExperience = 1,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        AvailableExperience = 2,
        AugmentationCost = 3,
        ItemTotalXp = 4,
        ItemBaseXp = 5,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        AvailableLuminance = 6,
        [PersistedProperty(true, typeof(Character), null)]
        MaximumLuminance = 7,
        InteractionReqs = 8,
        Count = 9
    }

    public static class PropertyInt64Extensions
    {
        public static PersistedPropertyAttribute GetPersistedPropertyAttribute(this PropertyInt64 val)
        {
            return val.GetAttributeOfType<PersistedPropertyAttribute>();
        }
    }
}
