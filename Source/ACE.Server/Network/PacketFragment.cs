
namespace ACE.Server.Network
{
    public abstract class PacketFragment
    {
        public static int MaxFragementSize { get; } = 464; // Packet.MaxPacketSize - PacketHeader.HeaderSize
        public static int MaxFragmentDataSize { get; } = 448; // Packet.MaxPacketSize - PacketHeader.HeaderSize - PacketFragmentHeader.HeaderSize

        public PacketFragmentHeader Header { get; } = new PacketFragmentHeader();
        public byte[] Data { get; protected set; }

        public int Length => PacketFragmentHeader.HeaderSize + Data.Length;
    }
}
