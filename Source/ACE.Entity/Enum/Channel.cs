using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The Channel identifies the type of chat message.<para />
    /// Used with F7B0 0147: Game Event -> Group Chat (ChatChannel)
    /// </summary>
    [Flags]
    public enum Channel
    {
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 2130737152, GhostChans_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 1855, ValidChans_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 1048576, Samsur_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 33554432, AllegianceBroadcast_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 0, Undef_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 16777216, Covassals_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 134217728, SocietyCelHanBroadcast_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 64, QA1_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 512, Sentinel_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 128, QA2_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 2, Admin_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 4, Audit_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 2097152, Shoushi_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 16744448, TownChans_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 536870912, SocietyRadBloBroadcast_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 256, Debug_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 4194304, Yanshi_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 524288, Rithwic_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 1025, AllBroadcast_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 268435456, SocietyEldWebBroadcast_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 8388608, Yaraq_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 1, Abuse_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 65536, Holtburg_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 262144, Nanto_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 1073741824, Olthoi_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 2048, Fellow_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 8192, Patron_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 2130739007, AllChans_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_ULONG) 131072, Lytelthorpe_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 16384, Monarch_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 1024, Help_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: (LF_USHORT) 32768, AlArqas_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 4096, Vassals_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 32, Advocate3_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 16, Advocate2_ChannelID
        // S_CONSTANT: Type:             0x108E, Value: 8, Advocate1_ChannelID

        Undef                   = 0x00000000,

        /// <summary>
        /// @abuse - Abuse Channel
        /// </summary>
        Abuse                   = 0x00000001,
        /// <summary>
        /// @admin - Admin Channel (@ad)
        /// </summary>
        Admin                   = 0x00000002,
        /// <summary>
        /// @audit - Audit Channel (@au)
        /// This channel was used to echo copies of enforcement commands (such as: ban, gag, boot) to all other online admins
        /// </summary>
        Audit                   = 0x00000004,
        /// <summary>
        /// @av1 - Advocate Channel (@advocate) (@advocate1)
        /// </summary>
        Advocate1               = 0x00000008,
        /// <summary>
        /// @av2 - Advocate2 Channel (@advocate2)
        /// </summary>
        Advocate2               = 0x00000010,
        /// <summary>
        /// @av3 - Advocate3 Channel (@advocate3)
        /// </summary>
        Advocate3               = 0x00000020,
        QA1                     = 0x00000040,
        QA2                     = 0x00000080,
        Debug                   = 0x00000100,
        /// <summary>
        /// @sent - Sentinel Channel (@sentinel)
        /// </summary>
        Sentinel                = 0x00000200,
        /// <summary>
        /// @[command name tbd] - Help Channel
        /// </summary>
        Help                    = 0x00000400,

        AllBroadcast            = 0x00000401,
        ValidChans              = 0x0000073F,

        /// <summary>
        /// @f - Tell Fellowship
        /// </summary>
        Fellow                  = 0x00000800,
        /// <summary>
        /// @v - Tell Vassals
        /// </summary>
        Vassals                 = 0x00001000,
        /// <summary>
        /// @p - Tell Patron
        /// </summary>
        Patron                  = 0x00002000,
        /// <summary>
        /// @m - Tell Monarch
        /// </summary>
        Monarch                 = 0x00004000,

        AlArqas                 = 0x00008000,
        Holtburg                = 0x00010000,
        Lytelthorpe             = 0x00020000,
        Nanto                   = 0x00040000,
        Rithwic                 = 0x00080000,
        Samsur                  = 0x00100000,
        Shoushi                 = 0x00200000,
        Yanshi                  = 0x00400000,
        Yaraq                   = 0x00800000,

        TownChans               = 0x00FF8000,

        /// <summary>
        /// @c - Tell Co-Vassals
        /// </summary>
        CoVassals               = 0x01000000,
        /// <summary>
        /// @allegiance broadcast - Tell All Allegiance Members
        /// </summary>
        AllegianceBroadcast     = 0x02000000,
        /// <summary>
        /// Player is now the leader of this fellowship.
        /// </summary>
        FellowBroadcast         = 0x04000000,

        SocietyCelHanBroadcast  = 0x08000000,
        SocietyEldWebBroadcast  = 0x10000000,
        SocietyRadBloBroadcast  = 0x20000000,

        Olthoi                  = 0x40000000,

        GhostChans              = 0x7F007800,
        AllChans                = 0x7F007F3F,
    }
}
