namespace ACE.Server.Factories.Enum
{
    public enum TreasureHeritageGroup
    {
        // base HeritageGroup enum
        Invalid     = 0,
        Aluvian     = 1,
        Gharundim   = 2,
        Sho         = 3,
        Viamontian  = 4,
        Shadowbound = 5,
        Gearknight  = 6,
        Tumerok     = 7,
        Lugian      = 8,
        Empyrean    = 9,
        Penumbraen  = 10,
        Undead      = 11,
        Olthoi      = 12,
        OlthoiAcid  = 13,

        // not sure if retail made use of TreasureDeath.UnknownChances (HeritageChances)
        // for societies, but this would be one way to do it..
        CelestialHand = 14,
        EldrytchWeb   = 15,
        RadiantBlood  = 16,
    }
}
