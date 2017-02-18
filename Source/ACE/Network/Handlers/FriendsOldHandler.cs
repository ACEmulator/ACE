using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;

namespace ACE.Network.Handlers
{
    public static class FriendsOldHandler
    {
        [GameMessageAttribute(GameMessageOpcode.FriendsOld, SessionState.WorldConnected)]
        public static void FriendsOld(ClientPacketFragment fragment, Session session)
        {
            ChatPacket.SendSystemMessage(session, "That command is not used in the emulator.");            
        }
    }
}
