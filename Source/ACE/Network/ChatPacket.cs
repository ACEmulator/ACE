
using ACE.Entity.Enum;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network
{
    public static class ChatPacket
    {
        public static void SendServerMessage(Session session, string message, ChatMessageType chatMessageType)
        {
            var textboxString         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var textboxStringFragment = new ServerPacketFragment(0x09, GameMessageOpcode.ServerMessage);
            textboxStringFragment.Payload.WriteString16L(message);
            textboxStringFragment.Payload.Write((int)chatMessageType);
            textboxString.Fragments.Add(textboxStringFragment);

            if (session == null)
            {
                // TODO: broadcast
            }
            else
                NetworkManager.SendPacket(ConnectionType.World, textboxString, session);
        }
    }
}
