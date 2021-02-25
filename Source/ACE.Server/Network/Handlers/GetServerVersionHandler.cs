using ACE.Entity.Enum;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.Handlers
{
    public static class GetServerVersionHandler
    {
        [GameMessage(GameMessageOpcode.GetServerVersion, SessionState.WorldConnected)]
        public static void GetServerVersion(ClientMessage message, Session session)
        {
            // @version command is native to client. Client responds with the following (if using end of retail client/data):

            // Using Turbine Chat
            // Client version 00.00.11.6096.r Portal: compiled Fri Jun 12 04:16:27 2015 : RETAIL
            
            // If client connects with admin account, it will forward version request to server and will respond with the following:

            var msg = ServerBuildInfo.GetVersionInfo();

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }
    }
}
