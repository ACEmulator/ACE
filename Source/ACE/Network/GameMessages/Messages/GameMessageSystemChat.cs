using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSystemChat : GameMessage
    {
        public GameMessageSystemChat(string message, ChatMessageType chatMessageType) 
            : base(GameMessageOpcode.ServerMessage, 0x9)
        {
            Writer.WriteString16L(message);
            Writer.Write((int)chatMessageType);
        }
    }
}