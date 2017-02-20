using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The GroupChatType identifies the type of group chat message.<para />
    /// Used with F7B0 0147: Game Event -> Group Chat
    /// </summary>
    [Flags]
    public enum GroupChatType
    {
        /// <summary>
        /// @f - Tell Fellowship
        /// </summary>
        TellFellowshi       = 0x00000800,

        /// <summary>
        /// @v - Tell Vassals
        /// </summary>
        TellVassals         = 0x00001000,

        /// <summary>
        /// @p - Tell Patron
        /// </summary>
        TellPatron          = 0x00002000,

        /// <summary>
        /// @m - Tell Monarch
        /// </summary>
        TellMonarch         = 0x00004000,

        /// <summary>
        /// @c - Tell Co-Vassals
        /// </summary>
        TellCoVassals       = 0x01000000,

        /// <summary>
        /// @allegiance broadcast - Tell All Allegiance Members
        /// </summary>
        AllegianceBroadcast = 0x02000000,

        /// <summary>
        /// Player is now the leader of this fellowship.
        /// </summary>
        FellowshipBroadcast = 0x04000000
    }
}
