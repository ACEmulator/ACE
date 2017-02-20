
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The ChatMessageType categorizes chat window messages to control color and filtering.<para />
    /// Used with F7B0 02BB: Game Event -> Creature Message<para />
    /// Used with F7B0 02BC: Game Event -> Creature Message (Ranged)<para />
    /// Used with F7B0 02BD: Game Event -> Someone has sent you a @tell.<para />
    /// Used with F7E0: Server Message
    /// </summary>
    public enum ChatMessageType
    {
        /// <summary>
        /// allegiance MOTD
        /// </summary>
        Broadcast           = 0x00,

        PublicChat          = 0x02,
        PrivateTell         = 0x03,

        /// <summary>
        /// You tell ...
        /// </summary>
        OutgoingTell        = 0x04,

        MagicSpellResults   = 0x07,
        NPCChat             = 0x0C,
        PlayerSpellcasting  = 0x11,

        /// <summary>
        /// Fellow warriors, aid me!
        /// </summary>
        CreatureChat        = 0x12,

        /// <summary>
        /// Player is recalling home.
        /// </summary>
        Recall              = 0x17,
    }
}
