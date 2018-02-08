using ACE.Entity.Enum;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.Handlers
{
    public static class FriendsOldHandler
    {
        [GameMessage(GameMessageOpcode.FriendsOld, SessionState.WorldConnected)]
        public static void FriendsOld(ClientMessage message, Session session)
        {
            ChatPacket.SendServerMessage(session, "That command is not used in the emulator.", ChatMessageType.Broadcast);
        }
    }
}