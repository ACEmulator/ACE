
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageServerName : GameMessage
    {
        public GameMessageServerName(string serverName) : base(GameMessageOpcode.ServerName, 0x9)
        {
            Writer.Write(0u);
            Writer.Write(0u);
            Writer.WriteString16L(serverName);
        }
    }
}