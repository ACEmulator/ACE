using ACE.Network.Enum;

namespace ACE.Network
{
    public class GameMessageSystemChat : GameMessage
    {
        private string message;

        public GameMessageSystemChat(Session target, string message) : base(GameMessageOpcode.TextboxString)
        {
            this.message = message;
            writer.WriteString16L(message);
            writer.Write(0x00);
        }
    }
}