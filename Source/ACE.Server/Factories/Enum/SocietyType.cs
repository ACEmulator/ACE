namespace ACE.Server.Factories.Enum
{
    public enum SocietyType
    {
        Undef,
        CelestialHand,
        EldrytchWeb,
        RadiantBlood
    }

    public static class SocietyTypeExtensions
    {
        public static SocietyType ToSociety(this TreasureHeritageGroup treasureHeritageGroup)
        {
            switch (treasureHeritageGroup)
            {
                case TreasureHeritageGroup.CelestialHand:
                    return SocietyType.CelestialHand;

                case TreasureHeritageGroup.EldrytchWeb:
                    return SocietyType.EldrytchWeb;

                case TreasureHeritageGroup.RadiantBlood:
                    return SocietyType.RadiantBlood;
            }
            return SocietyType.Undef;
        }
    }
}
