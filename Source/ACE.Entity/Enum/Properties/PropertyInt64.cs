using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
    // http://ac.yotesfan.com/ace_object/not_used_enums.php
    // source: @OptimShi
    // description attributes are used by the weenie editor for a cleaner display name
    public enum PropertyInt64 : ushort
    {
        Undef               = 0,
        [SendOnLogin]
        TotalExperience     = 1,
        [SendOnLogin]
        AvailableExperience = 2,
        AugmentationCost    = 3,
        ItemTotalXp         = 4,
        ItemBaseXp          = 5,
        [SendOnLogin]
        AvailableLuminance  = 6,
        [SendOnLogin]
        MaximumLuminance    = 7,
        InteractionReqs     = 8,

        /* custom */
        [ServerOnly]
        AllegianceXPCached    = 9000,
        [ServerOnly]
        AllegianceXPGenerated = 9001,
        [ServerOnly]
        AllegianceXPReceived  = 9002,
        [ServerOnly]
        VerifyXp              = 9003
    }

    public static class PropertyInt64Extensions
    {
        public static string GetDescription(this PropertyInt64 prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }
    }
}
