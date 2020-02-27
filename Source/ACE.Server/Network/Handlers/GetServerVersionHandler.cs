using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

using System.Diagnostics;
using System.Reflection;

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

            var databaseVersion = DatabaseManager.World.GetVersion();

            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var serverVersion = fileVersionInfo.ProductVersion;

            var msg = $"Server binaries version {serverVersion} - compiled {VersionConstant.CompilationTimestampUtc.ToString("ddd MMM d HH:mm:ss yyyy")} : ACEmulator\n";

            msg += $"Server database version Base: {databaseVersion.BaseVersion} Patch: {databaseVersion.PatchVersion} - compiled {databaseVersion.LastModified.ToString("ddd MMM d HH:mm:ss yyyy")}\n";

#if DEBUG
            msg += "Server is compiled in DEBUG mode";
#else
            msg += "Server is compiled in RELEASE mode";
#endif

            session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
        }
    }
}
