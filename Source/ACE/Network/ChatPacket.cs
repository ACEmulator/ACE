namespace ACE.Network
{
    public static class ChatPacket
    {
        public static void SendSystemMessage(Session session, string message)
        {
            var textboxString         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var textboxStringFragment = new ServerPacketFragment(0x09, FragmentOpcode.TextboxString);
            textboxStringFragment.Payload.WriteString16L(message);
            textboxStringFragment.Payload.Write(0x00);
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
