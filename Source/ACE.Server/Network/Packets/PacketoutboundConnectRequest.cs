namespace ACE.Server.Network.Packets
{
    public class PacketOutboundConnectRequest : ServerPacket
    {
        public PacketOutboundConnectRequest(double serverTime, ulong cookie, uint clientId, byte[] isaacServerSeed, byte[] isaacClientSeed)
        {
            Header.Flags = PacketHeaderFlags.ConnectRequest;

            InitializeBodyWriter();

            BodyWriter.Write(serverTime); // CConnectHeader.ServerTime
            BodyWriter.Write(cookie); // CConnectHeader.Cookie
            BodyWriter.Write(clientId); // CConnectHeader.NetID
            BodyWriter.Write(isaacServerSeed); // CConnectHeader.OutgoingSeed
            BodyWriter.Write(isaacClientSeed); // CConnectHeader.IncomingSeed
            BodyWriter.Write(0u); // Padding for alignment?
        }
    }
}
