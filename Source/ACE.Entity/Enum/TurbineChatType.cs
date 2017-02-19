
namespace ACE.Entity.Enum
{
    /// <summary>
    /// The TurbineChatType identifies the type of Turbine Chat message.<para />
    /// Used with F7DE: Turbine Chat
    /// </summary>
    public enum TurbineChatType
    {
        InboundMessage                  = 0x01,
        OutboundMessage                 = 0x03,
        OutboundMessageAcknowledgement  = 0x05
    }
}
