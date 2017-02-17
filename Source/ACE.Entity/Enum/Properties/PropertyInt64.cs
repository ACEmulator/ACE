
namespace ACE.Entity.Enum.Properties
{
    public enum PropertyInt64 : ushort
    {
        Undef,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        TotalExperience,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        AvailableExperience,
        AugmentationCost,
        ItemTotalXp,
        ItemBaseXp,
        [PersistedProperty(true, typeof(Character), (ulong)0)]
        AvailableLuminance,
        [PersistedProperty(true, typeof(Character), null)]
        MaximumLuminance,
        InteractionReqs,
        Count
    }

    public static class PropertyInt64Extensions
    {
        public static PersistedPropertyAttribute GetPersistedPropertyAttribute(this PropertyInt64 val)
        {
            return Enum.EnumHelper.GetAttributeOfType<PersistedPropertyAttribute>(val);
        }
    }
}
