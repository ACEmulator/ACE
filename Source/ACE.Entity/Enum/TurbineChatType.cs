
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The TurbineChatType identifies the type of Turbine Chat message.<para />
    /// Used with F7DE: Turbine Chat
    /// </summary>
    public enum TurbineChatType
    {
        /// <summary>
        /// Server -> Client
        /// </summary>
        InboundMessage                  = 0x01,

        /// <summary>
        /// Client -> Server
        /// </summary>
        OutboundMessage                 = 0x03,

        /// <summary>
        /// Server -> Client
        /// </summary>
        OutboundMessageAcknowledgement  = 0x05
    }
}
