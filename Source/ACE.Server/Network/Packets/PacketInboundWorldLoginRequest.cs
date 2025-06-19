// UTF-8 BOM removed to ensure consistent encoding
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
