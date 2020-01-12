using ACE.Database;
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
            // @version command is native to client. If an admin, will respond with the following:

            // Using Turbine Chat
            // Client version 00.00.11.6096.r Portal: compiled Fri Jun 12 04:16:27 2015 : RETAIL
            // ^^^^^^ embedded response in client

            var version = DatabaseManager.World.GetVersion();

            var msg = $"Server database version Base: {version.BaseVersion} Patch: {version.PatchVersion} - compiled {version.LastModified.ToString("ddd MMM d HH:mm:ss yyyy")}\n";

#if DEBUG
            msg += "Server is compiled in DEBUG mode";
#else
            msg += "Server is compiled in RELEASE mode";
#endif

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }
    }
}
