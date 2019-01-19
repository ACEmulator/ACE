namespace ACE.Entity.Enum
{
    /// <summary>
    /// The various options for filtering the spellbook
    /// </summary>
    public enum SpellBookFilterOptions: uint
    {
        None     = 0x0000,
        Creature = 0x0001,
        Item     = 0x0002,
        Life     = 0x0004,
        War      = 0x0008,
        Level1   = 0x0010,
        Level2   = 0x0020,
        Level3   = 0x0040,
        Level4   = 0x0080,
        Level5   = 0x0100,
        Level6   = 0x0200,
        Level7   = 0x0400,
        Level8   = 0x0800,
        Level9   = 0x1000,
        Void     = 0x2000,
    }
}
