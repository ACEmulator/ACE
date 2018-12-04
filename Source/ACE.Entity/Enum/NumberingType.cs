namespace ACE.Entity.Enum
{
    public enum NumberingType: byte
    {
        Undefined   = 0x0,
        Normal      = 0x1,
        Sequential  = 0x1,
        Bitfield    = 0x2,
        Bitfield32  = 0x3,
        Bitfield64  = 0x4
    }
}
