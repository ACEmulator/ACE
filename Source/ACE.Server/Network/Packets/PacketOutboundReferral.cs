namespace ACE.Server.Network.Packets
{
    public class PacketOutboundReferral : ServerPacket
    {
        public PacketOutboundReferral(ulong worldConnectionKey, string[] sessionIPAddress, byte[] host, ushort port, bool sendInternalHostOnLocalNetwork, byte[] internalHost)
        {
            Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral;

            InitializeDataWriter();

            DataWriter.Write(worldConnectionKey);
            DataWriter.Write((ushort)2);
            DataWriter.WriteUInt16BE(port);

            if (sendInternalHostOnLocalNetwork &&
                (sessionIPAddress[0] == "10"
                || (sessionIPAddress[0] == "172" && System.Convert.ToInt16(sessionIPAddress[1]) >= 16 && System.Convert.ToInt16(sessionIPAddress[1]) <= 31)
                || (sessionIPAddress[0] == "192" && sessionIPAddress[1] == "168")))
                DataWriter.Write(internalHost);
            else
                DataWriter.Write(host);

            DataWriter.Write(0ul);
            DataWriter.Write((ushort)0x18); // This value is currently the hard coded Server ID. It can be something different...
            DataWriter.Write((ushort)0);
            DataWriter.Write(0u);
        }
    }
}
