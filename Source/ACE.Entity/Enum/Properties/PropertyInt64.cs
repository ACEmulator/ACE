﻿namespace ACE.Entity.Enum.Properties
{
    public enum PropertyInt64 : ushort
    {
        Undef = 0,
        TotalExperience = 1,
        AvailableExperience = 2,
        AugmentationCost = 3,
        ItemTotalXp = 4,
        ItemBaseXp = 5,
        AvailableLuminance = 6,
        MaximumLuminance = 7,
        InteractionReqs = 8,

        // values over 9000 are ones that we have added and should not be sent to the client
        DeleteTime = 9001
    }
}
