using System;

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

        public static LootTables.ArmorType ToACEArmor(this TreasureItemType_Orig treasureItemType_orig)
        {
            switch (treasureItemType_orig)
            {
                case TreasureItemType_Orig.LeatherArmor:
                    return LootTables.ArmorType.LeatherArmor;
                case TreasureItemType_Orig.StuddedLeatherArmor:
                    return LootTables.ArmorType.StuddedLeatherArmor;
                case TreasureItemType_Orig.ChainMailArmor:
                    return LootTables.ArmorType.ChainmailArmor;
                case TreasureItemType_Orig.CovenantArmor:
                    return LootTables.ArmorType.CovenantArmor;
                case TreasureItemType_Orig.PlateMailArmor:
                    return LootTables.ArmorType.PlatemailArmor;
                case TreasureItemType_Orig.CeldonArmor:
                    return LootTables.ArmorType.CeldonArmor;
                case TreasureItemType_Orig.AmuliArmor:
                    return LootTables.ArmorType.AmuliArmor;
                case TreasureItemType_Orig.KoujiaArmor:
                    return LootTables.ArmorType.KoujiaArmor;
                case TreasureItemType_Orig.LoricaArmor:
                    return LootTables.ArmorType.LoricaArmor;
                case TreasureItemType_Orig.NariyidArmor:
                    return LootTables.ArmorType.NariyidArmor;
                case TreasureItemType_Orig.ChiranArmor:
                    return LootTables.ArmorType.ChiranArmor;
            }
            Console.WriteLine($"TreasureItemType_Orig.ToACEArmor({treasureItemType_orig}): unknown armor type");
            return LootTables.ArmorType.Undef;
        }
    }
}
