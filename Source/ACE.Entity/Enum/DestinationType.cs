using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum DestinationType
    { 
        Undef           = 0,
        Contain         = 1,
        Wield           = 2,
        Shop            = 4,
        Treasure        = 8,
        HouseBuy        = 16,
        HouseRent       = 32,
        Checkpoint      = Contain | Wield | Shop,
        ContainTreasure = Contain | Treasure,
        WieldTreasure   = Wield | Treasure,
        ShopTreasure    = Shop | Treasure 
    }
}
