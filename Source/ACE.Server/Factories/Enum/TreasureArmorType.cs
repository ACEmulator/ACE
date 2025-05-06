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
        public static bool IsSocietyArmor(this TreasureArmorType armorType)
        {
            switch (armorType)
            {
                case TreasureArmorType.Society:
                case TreasureArmorType.CelestialHand:
                case TreasureArmorType.EldrytchWeb:
                case TreasureArmorType.RadiantBlood:
                    return true;
            }
            return false;
        }
    }
}
