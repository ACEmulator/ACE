namespace ACE.Entity.Enum
{
    /// <summary>
    /// The ChatDisplayMask identifies that types of chat that are displayed in each chat window.<para />
    /// Used by CharacterOptionData: The CharacterOptionData structure contains character options.
    /// </summary>
    public enum ChatDisplayMask : long
    {
        /// <summary>
        /// Gameplay (main chat window only)
        /// </summary>
        Gameplay        = 0x0000000003912021,

        /// <summary>
        /// Mandatory (main chat window only, cannot be disabled)
        /// </summary>
        Mandatory       = 0x000000000000c302,
     
        AreaChat        = 0x0000000000001004,
        Tells           = 0x0000000000000018,
        Combat          = 0x0000000000600040,
        Magic           = 0x0000000000020080,
        Allegiance      = 0x0000000000040c00,
        Fellowship      = 0x0000000000080000,
        Errors          = 0x0000000004000000,
        GeneralChannel  = 0x0000000008000000,
        TradeChannel    = 0x0000000010000000,
        LFGChannel      = 0x0000000020000000,
        RoleplayChannel = 0x0000000040000000,
    }
}
