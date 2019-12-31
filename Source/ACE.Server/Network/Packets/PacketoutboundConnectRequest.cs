namespace ACE.Server.Network.Packets
{
    public class PacketOutboundConnectRequest : ServerPacket
    {
        public PacketOutboundConnectRequest(double serverTime, ulong cookie, uint clientId, byte[] isaacServerSeed, byte[] isaacClientSeed)
        {
            Header.Flags = PacketHeaderFlags.ConnectRequest;

            InitializeDataWriter();

            DataWriter.Write(serverTime); // CConnectHeader.ServerTime
            DataWriter.Write(cookie); // CConnectHeader.Cookie
            DataWriter.Write(clientId); // CConnectHeader.NetID
            DataWriter.Write(isaacServerSeed); // CConnectHeader.OutgoingSeed
            DataWriter.Write(isaacClientSeed); // CConnectHeader.IncomingSeed
            DataWriter.Write(0u); // Padding for alignment?
        }
    }
}
