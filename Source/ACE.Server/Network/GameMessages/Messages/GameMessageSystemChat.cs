using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSystemChat : GameMessage
    {
        public GameMessageSystemChat(string message, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.ServerMessage, GameMessageGroup.UIQueue)
        {
            Writer.WriteString16L(message);
            Writer.Write((int)chatMessageType);
        }
    }
}
