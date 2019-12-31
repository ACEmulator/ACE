namespace ACE.Server.Network.Packets
{
    public class PacketInboundWorldLoginRequest
    {
        public ulong ConnectionKey { get; }

        public PacketInboundWorldLoginRequest(ClientPacket packet)
        {
            ConnectionKey = packet.DataReader.ReadUInt64();
        }
    }
}
