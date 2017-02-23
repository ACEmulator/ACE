
using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventTell : GameEventMessage
    {
        public GameEventTell(Session session, string messageText, string senderName, uint senderID, uint targetID, ChatMessageType chatMessageType) : base(GameEventType.Tell, 0x9, session)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(senderName);
            Writer.Write(senderID);
            Writer.Write(targetID);
            Writer.Write((uint)chatMessageType);
            Writer.Write(0u); // This is not documented in the xml's, but is found in the pcaps. The functionality seems the same with or without it.
        }
    }
}
