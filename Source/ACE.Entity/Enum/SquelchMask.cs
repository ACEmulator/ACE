namespace ACE.Entity.Enum
{
    public enum SquelchMask: uint
    {
        // this is not a client enum,
        // but is equivalent to 1 << ChatMessageType
        Speech              = 0x4,
        Tell                = 0x8,
        Combat              = 0x40,
        Magic               = 0x80,
        Emote               = 0x1000,
        Appraisal           = 0x10000,
        Spellcasting        = 0x20000,
        Allegiance          = 0x40000,
        Fellowship          = 0x80000,
        CombatEnemy         = 0x200000,
        CombatSelf          = 0x400000,
        Recall              = 0x800000,
        Craft               = 0x1000000,
        Salvaging           = 0x2000000,
        AllChannels         = 0xFFFFFFFF
    }
}
