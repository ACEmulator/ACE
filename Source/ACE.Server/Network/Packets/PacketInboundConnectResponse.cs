namespace ACE.Server.Network.Packets
{
    public class PacketInboundConnectResponse
    {
        public ulong Check { get; }

        public PacketInboundConnectResponse(ClientPacket packet)
        {
            Check = packet.DataReader.ReadUInt64();
        }
    }
}
