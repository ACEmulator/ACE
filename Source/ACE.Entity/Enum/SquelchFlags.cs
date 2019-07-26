namespace ACE.Entity.Enum
{
    public enum SquelchFlags: uint
    {
        // this is not a client enum,
        // but is equivalent to 1 << ChatMessageType

        Broadcast      = 0x0,
        AllChannels    = 0x1,
        Speech         = 0x2,
        Tell           = 0x4,
        OutgoingTell   = 0x8,
        System         = 0x10,
        Combat         = 0x20,
        Magic          = 0x40,
        Channel        = 0x80,
        ChannelSend    = 0x100,
        Social         = 0x200,
        SocialSend     = 0x400,
        Emote          = 0x800,
        Advancement    = 0x1000,
        Abuse          = 0x2000,
        Help           = 0x4000,
        Appraisal      = 0x8000,
        Spellcasting   = 0x10000,
        Allegiance     = 0x20000,
        Fellowship     = 0x40000,
        WorldBroadcast = 0x80000,
        CombatEnemy    = 0x100000,
        CombatSelf     = 0x200000,
        Recall         = 0x400000,
        Craft          = 0x800000,
        Salvaging      = 0x1000000,
        Unknown1       = 0x2000000,
        Unknown2       = 0x4000000,
        Unkonwn3       = 0x8000000,
        Unknown4       = 0x10000000,
        Unknown5       = 0x20000000,
        AdminTell      = 0x40000000
    }

    public enum SquelchChannel
    {
        None         = 0x0,
        Speech       = 0x1,
        Tell         = 0x2,
        Combat       = 0x4,
        Magic        = 0x8,
        Emote        = 0x10,
        Appraisal    = 0x20,
        Spellcasting = 0x40,
        Allegiance   = 0x80,
        Fellowship   = 0x100,
        Combat_Enemy = 0x200,
        Combat_Self  = 0x400,
        Recall       = 0x800,
        Craft        = 0x1000,
        Salvaging    = 0x2000,
    }
}
