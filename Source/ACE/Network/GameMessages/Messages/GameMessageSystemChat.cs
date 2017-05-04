using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSystemChat : GameMessage
    {
        public GameMessageSystemChat(string message, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.ServerMessage, GameMessageGroup.Group09)
        {
            Writer.WriteString16L(message);
            Writer.Write((int)chatMessageType);
        }
    }
}