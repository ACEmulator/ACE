
namespace ACE.Server.Network
{
    public abstract class PacketFragment
    {
        public const int MaxFragementSize = 464; // Packet.MaxPacketSize - PacketHeader.HeaderSize
        public const int MaxFragmentDataSize = 448; // Packet.MaxPacketSize - PacketHeader.HeaderSize - PacketFragmentHeader.HeaderSize

        public PacketFragmentHeader Header { get; } = new PacketFragmentHeader();
        public byte[] Data { get; protected set; }

        public int Length => PacketFragmentHeader.HeaderSize + Data.Length;
    }
}
