namespace ACE.Entity.Enum
{
    public enum RadarColor : byte
    {
        Default             = 0x00,
        Blue                = 0x01,
        Gold                = 0x02,
        White               = 0x03,
        Purple              = 0x04,
        Red                 = 0x05,
        Pink                = 0x06,
        Green               = 0x07,
        Yellow              = 0x08,
        Cyan                = 0x09,
        BrightGreen         = 0x10,
        Admin               = Cyan,
        Advocate            = Pink,
        Creature            = Gold,
        LifeStone           = Blue,
        NPC                 = Yellow,
        PlayerKiller        = Red,
        Portal              = Purple,
        Sentinel            = Cyan,
        Vendor              = Yellow,
        Fellowship          = BrightGreen,
        FellowshipLeader    = BrightGreen,
        PKLite              = Pink
    }
}
