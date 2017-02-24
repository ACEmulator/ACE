using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCreatureMessage : GameMessage
    {
        public GameMessageCreatureMessage(string messageText, string senderName, uint senderID, ChatMessageType chatMessageType) 
            : base(GameMessageOpcode.CreatureMessage, 0x9)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(senderName);
            Writer.Write(senderID);
            Writer.Write((uint)chatMessageType);
        }
    }
}
