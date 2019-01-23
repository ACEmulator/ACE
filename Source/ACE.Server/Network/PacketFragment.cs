namespace ACE.Server.Network
{
    public abstract class PacketFragment
    {
        public static int MaxFragementSize { get; } = 464; // Packet.MaxPacketSize - PacketHeader.HeaderSize
        public static int MaxFragmentDataSize { get; } = 448; // Packet.MaxPacketSize - PacketHeader.HeaderSize - PacketFragmentHeader.HeaderSize

        public PacketFragmentHeader Header { get; protected set; }
        public byte[] Data { get; protected set; }

        public int Length => Data.Length + PacketFragmentHeader.HeaderSize;
    }
}
