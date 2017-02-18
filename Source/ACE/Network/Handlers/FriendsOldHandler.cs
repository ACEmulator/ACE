using ACE.Network.Enum;
using ACE.Network.Fragments;

namespace ACE.Network.Handlers
{
    public static class FriendsOldHandler
    {
        [Fragment(FragmentOpcode.FriendsOld, SessionState.WorldConnected)]
        public static void FriendsOld(ClientPacketFragment fragment, Session session)
        {
            ChatPacket.SendSystemMessage(session, "That command is not used in the emulator.");            
        }
    }
}
