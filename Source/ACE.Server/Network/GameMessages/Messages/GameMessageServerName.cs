namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageServerName : GameMessage
    {
        public GameMessageServerName(string serverName, int currentConnections = 0, int maxConnections = -1)
            : base(GameMessageOpcode.ServerName, GameMessageGroup.UIQueue, 32) // 28 is the max seen in retail pcaps
        {
            Writer.Write(currentConnections);
            Writer.Write(maxConnections);
            Writer.WriteString16L(serverName);
        }
    }
}
