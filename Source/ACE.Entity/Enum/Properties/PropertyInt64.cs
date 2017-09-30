namespace ACE.Entity.Enum.Properties
{
    // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
    // http://ac.yotesfan.com/ace_object/not_used_enums.php
    // source: @OptimShi

    // description attributes are used by the weenie editor for a cleaner display name
    public enum PropertyInt64 : ushort
    {
        [ServerOnly]
        Undef               = 0,
        [ServerOnly]
        TotalExperience     = 1,
        [ServerOnly]
        AvailableExperience = 2,
        AugmentationCost    = 3,
        ItemTotalXp         = 4,
        ItemBaseXp          = 5,
        [ServerOnly]
        AvailableLuminance  = 6,
        [ServerOnly]
        MaximumLuminance    = 7,
        [ServerOnly]
        InteractionReqs     = 8,
        [ServerOnly]
        DeleteTime          = 9001
    }
}
