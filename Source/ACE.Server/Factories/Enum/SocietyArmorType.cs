namespace ACE.Server.Factories.Enum
{
    public enum SocietyArmorType
    {
        Undef,
        Breastplate,
        Gauntlets,
        Girth,
        Greaves,
        Helm,
        Pauldrons,
        Tassets,
        Vambraces,
        Sollerets,
    }

    public static class SocietyArmorTypeExtensions
    {
        public static SocietyArmorType GetSocietyArmorType(this TreasureItemType_Orig treasureItemType)
        {
            switch (treasureItemType)
            {
                case TreasureItemType_Orig.SocietyBreastplate:
                    return SocietyArmorType.Breastplate;

                case TreasureItemType_Orig.SocietyGauntlets:
                    return SocietyArmorType.Gauntlets;

                case TreasureItemType_Orig.SocietyGirth:
                    return SocietyArmorType.Girth;

                case TreasureItemType_Orig.SocietyGreaves:
                    return SocietyArmorType.Greaves;

                case TreasureItemType_Orig.SocietyHelm:
                    return SocietyArmorType.Helm;

                case TreasureItemType_Orig.SocietyPauldrons:
                    return SocietyArmorType.Pauldrons;

                case TreasureItemType_Orig.SocietyTassets:
                    return SocietyArmorType.Tassets;

                case TreasureItemType_Orig.SocietyVambraces:
                    return SocietyArmorType.Vambraces;

                case TreasureItemType_Orig.SocietySollerets:
                    return SocietyArmorType.Sollerets;
            }
            return SocietyArmorType.Undef;
        }
    }
}
