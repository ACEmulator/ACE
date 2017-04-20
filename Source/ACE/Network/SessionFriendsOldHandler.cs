using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages;

namespace ACE.Network
{
    public partial class Session
    {
        [GameMessageAttribute(GameMessageOpcode.FriendsOld, SessionState.WorldConnected)]
        private void FriendsOld(ClientMessage message)
        {
            ChatPacket.SendServerMessage(this, "That command is not used in the emulator.", ChatMessageType.Broadcast);
        }
    }
}