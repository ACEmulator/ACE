namespace ACE.Server.Factories.Enum
{
    public enum TreasureArmorType
    {
        Undef,
        Leather,
        StuddedLeather,
        Chainmail,
        Platemail,          // platemail - aluvian
        Scalemail,          // platemail - gharu'ndim
        Yoroi,              // platemail - sho
        HeritageLow,
        Celdon,             // heritage low - aluvian
        Amuli,              // heritage low - gharu'ndim
        Koujia,             // heritage low - sho
        Covenant,
        HeritageHigh,
        Lorica,             // heritage high - aluvian
        Nariyid,            // heritage high - gharu'ndim
        Chiran,             // heritage high - sho
        // tod+
        Diforsa,            // platemail - viamontian
        Tenassa,            // heritage low - viamontian
        Alduressa,          // heritage high - viamontian
        Olthoi,
        OlthoiHeritage,
        OlthoiCeldon,       // olthoi heritage - aluvian
        OlthoiAmuli,        // olthoi heritage - gharu'ndim
        OlthoiKoujia,       // olthoi heritage - sho
        OlthoiAlduressa,    // olthoi heritage - viamontian
        Society,
        CelestialHand,      // society - celestial hand
        EldrytchWeb,        // society - eldrytch web
        RadiantBlood,       // society - radiant blood
        Haebrean,
        KnorrAcademy,
        Sedgemail,
        Overrobe
    }

    public static class TreasureArmorTypeHelper
    {
        public static LootTables.ArmorType ToACE(this TreasureArmorType armorType)
        {
            switch (armorType)
            {
                case TreasureArmorType.Leather:
                    return LootTables.ArmorType.LeatherArmor;
                case TreasureArmorType.StuddedLeather:
                    return LootTables.ArmorType.StuddedLeatherArmor;
                case TreasureArmorType.Chainmail:
                    return LootTables.ArmorType.ChainmailArmor;
                case TreasureArmorType.Platemail:
                    return LootTables.ArmorType.PlatemailArmor;
                case TreasureArmorType.Scalemail:
                    return LootTables.ArmorType.ScalemailArmor;
                case TreasureArmorType.Yoroi:
                    return LootTables.ArmorType.YoroiArmor;
                case TreasureArmorType.Celdon:
                    return LootTables.ArmorType.CeldonArmor;
                case TreasureArmorType.Amuli:
                    return LootTables.ArmorType.AmuliArmor;
                case TreasureArmorType.Koujia:
                    return LootTables.ArmorType.KoujiaArmor;
                case TreasureArmorType.Covenant:
                    return LootTables.ArmorType.CovenantArmor;
                case TreasureArmorType.Lorica:
                    return LootTables.ArmorType.LoricaArmor;
                case TreasureArmorType.Nariyid:
                    return LootTables.ArmorType.NariyidArmor;
                case TreasureArmorType.Chiran:
                    return LootTables.ArmorType.ChiranArmor;
                case TreasureArmorType.Diforsa:
                    return LootTables.ArmorType.DiforsaArmor;
                case TreasureArmorType.Tenassa:
                    return LootTables.ArmorType.TenassaArmor;
                case TreasureArmorType.Alduressa:
                    return LootTables.ArmorType.AlduressaArmor;
                case TreasureArmorType.Olthoi:
                    return LootTables.ArmorType.OlthoiArmor;
                case TreasureArmorType.OlthoiCeldon:
                    return LootTables.ArmorType.OlthoiCeldonArmor;
                case TreasureArmorType.OlthoiAmuli:
                    return LootTables.ArmorType.OlthoiAmuliArmor;
                case TreasureArmorType.OlthoiKoujia:
                    return LootTables.ArmorType.OlthoiKoujiaArmor;
                case TreasureArmorType.OlthoiAlduressa:
                    return LootTables.ArmorType.OlthoiAlduressaArmor;
                case TreasureArmorType.Society:
                case TreasureArmorType.CelestialHand:
                case TreasureArmorType.EldrytchWeb:
                case TreasureArmorType.RadiantBlood:
                    return LootTables.ArmorType.SocietyArmor;
                case TreasureArmorType.Haebrean:
                    return LootTables.ArmorType.HaebreanArmor;
                case TreasureArmorType.KnorrAcademy:
                    return LootTables.ArmorType.KnorrAcademyArmor;
                case TreasureArmorType.Sedgemail:
                    return LootTables.ArmorType.SedgemailLeatherArmor;
                case TreasureArmorType.Overrobe:
                    return LootTables.ArmorType.OverRobes;
            }
            return LootTables.ArmorType.Undef;
        }
    }
}
