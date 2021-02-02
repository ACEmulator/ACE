using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventTell : GameEventMessage
    {
        public GameEventTell(WorldObject worldObject, string messageText, Player player, ChatMessageType chatMessageType)
            : base(GameEventType.Tell, GameMessageGroup.UIQueue, player.Session)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(worldObject.Name);
            Writer.WriteGuid(worldObject.Guid);
            Writer.WriteGuid(player.Guid);
            Writer.Write((uint)chatMessageType);
            Writer.Write(0u); // This is not documented in the xml's, but is found in the pcaps. The functionality seems the same with or without it.
        }

        public GameEventTell(Session session, string messageText, string senderName, uint senderID, uint targetID, ChatMessageType chatMessageType)
            : base(GameEventType.Tell, GameMessageGroup.UIQueue, session)
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
