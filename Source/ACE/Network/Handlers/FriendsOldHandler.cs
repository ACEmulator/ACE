using System.Threading.Tasks;

using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages;

namespace ACE.Network.Handlers
{
    public static class FriendsOldHandler
    {
        [GameMessageAttribute(GameMessageOpcode.FriendsOld, SessionState.WorldConnected)]
        #pragma warning disable 1998
        public static async Task FriendsOld(ClientMessage message, Session session)
        {
            ChatPacket.SendServerMessage(session, "That command is not used in the emulator.", ChatMessageType.Broadcast);
        }
        #pragma warning restore 1998
    }
}
