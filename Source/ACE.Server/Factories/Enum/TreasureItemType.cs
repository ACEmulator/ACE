namespace ACE.Server.Factories.Enum
{
    public enum TreasureItemType
    {
        // current ace
        Undef,
        Gem,
        Armor,
        Clothing,
        Cloak,
        Weapon,
        Jewelry,
        Dinnerware
    }

    public static class TreasureItemTypeHelper
    {
        public static TreasureItemType_Orig ToRetail(this TreasureItemType treasureItemType)
        {
            switch (treasureItemType)
            {
                case TreasureItemType.Gem:
                    return TreasureItemType_Orig.Gem;
                case TreasureItemType.Armor:
                    return TreasureItemType_Orig.Armor;
                case TreasureItemType.Clothing:
                    return TreasureItemType_Orig.Clothing;
                case TreasureItemType.Cloak:
                    return TreasureItemType_Orig.Undef;     // undefined?
                case TreasureItemType.Weapon:
                    return TreasureItemType_Orig.Weapon;
                case TreasureItemType.Jewelry:
                    return TreasureItemType_Orig.Jewelry;
                case TreasureItemType.Dinnerware:
                    return TreasureItemType_Orig.ArtObject; // ??
            }
            return TreasureItemType_Orig.Undef;
        }

        public static TreasureItemType ToACE(this TreasureItemType_Orig treasureItemType_orig)
        {
            switch (treasureItemType_orig)
            {
                case TreasureItemType_Orig.Gem:
                    return TreasureItemType.Gem;
                case TreasureItemType_Orig.Armor:
                    return TreasureItemType.Armor;
                case TreasureItemType_Orig.Clothing:
                    return TreasureItemType.Clothing;
                case TreasureItemType_Orig.Weapon:
                    return TreasureItemType.Weapon;
                case TreasureItemType_Orig.Jewelry:
                    return TreasureItemType.Jewelry;
                case TreasureItemType_Orig.ArtObject:
                    return TreasureItemType.Dinnerware; // ??
            }
            return TreasureItemType.Undef;
        }
    }
}
