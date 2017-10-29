﻿using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
    // http://ac.yotesfan.com/ace_object/not_used_enums.php
    // source: @OptimShi
    // description attributes are used by the weenie editor for a cleaner display name
    public enum PropertyInt64 : ushort
    {
        Undef               = 0,
        TotalExperience     = 1,
        AvailableExperience = 2,
        AugmentationCost    = 3,
        ItemTotalXp         = 4,
        ItemBaseXp          = 5,
        AvailableLuminance  = 6,
        MaximumLuminance    = 7,
        InteractionReqs     = 8,
        [ServerOnly]
        DeleteTime          = 9001
    }

    public static class PropertyInt64Extensions
    {
        public static string GetDescription(this PropertyInt64 prop)
        {
            var description = EnumHelper.GetAttributeOfType<DescriptionAttribute>(prop);
            return description?.Description ?? prop.ToString();
        }
    }
}
