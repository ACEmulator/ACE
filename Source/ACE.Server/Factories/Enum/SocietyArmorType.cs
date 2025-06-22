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
        public static SocietyArmorType GetSocietyArmorType(this TreasureItemType treasureItemType)
        {
            switch (treasureItemType)
            {
                case TreasureItemType.SocietyBreastplate:
                    return SocietyArmorType.Breastplate;

                case TreasureItemType.SocietyGauntlets:
                    return SocietyArmorType.Gauntlets;

                case TreasureItemType.SocietyGirth:
                    return SocietyArmorType.Girth;

                case TreasureItemType.SocietyGreaves:
                    return SocietyArmorType.Greaves;

                case TreasureItemType.SocietyHelm:
                    return SocietyArmorType.Helm;

                case TreasureItemType.SocietyPauldrons:
                    return SocietyArmorType.Pauldrons;

                case TreasureItemType.SocietyTassets:
                    return SocietyArmorType.Tassets;

                case TreasureItemType.SocietyVambraces:
                    return SocietyArmorType.Vambraces;

                case TreasureItemType.SocietySollerets:
                    return SocietyArmorType.Sollerets;
            }
            return SocietyArmorType.Undef;
        }
    }
}
