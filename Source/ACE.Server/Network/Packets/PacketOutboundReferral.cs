namespace ACE.Server.Network.Packets
{
    public class PacketOutboundReferral : ServerPacket
    {
        public PacketOutboundReferral(ulong worldConnectionKey, string[] sessionIPAddress, byte[] host, ushort port, bool sendInternalHostOnLocalNetwork, byte[] internalHost)
        {
            Header.Flags = PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral;

            InitializeBodyWriter();

            BodyWriter.Write(worldConnectionKey);
            BodyWriter.Write((ushort)2);
            BodyWriter.WriteUInt16BE(port);

            if (sendInternalHostOnLocalNetwork &&
                (sessionIPAddress[0] == "10"
                || (sessionIPAddress[0] == "172" && System.Convert.ToInt16(sessionIPAddress[1]) >= 16 && System.Convert.ToInt16(sessionIPAddress[1]) <= 31)
                || (sessionIPAddress[0] == "192" && sessionIPAddress[1] == "168")))
                BodyWriter.Write(internalHost);
            else
                BodyWriter.Write(host);

            BodyWriter.Write(0ul);
            BodyWriter.Write((ushort)0x18); // This value is currently the hard coded Server ID. It can be something different...
            BodyWriter.Write((ushort)0);
            BodyWriter.Write(0u);
        }
    }
}
