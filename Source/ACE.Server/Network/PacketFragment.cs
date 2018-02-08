namespace ACE.Server.Network
{
    public abstract class PacketFragment
    {
        public static uint MaxFragementSize { get; } = 464u; // Packet.MaxPacketSize - PacketHeader.HeaderSize
        public static uint MaxFragmentDataSize { get; } = 448u; // Packet.MaxPacketSize - PacketHeader.HeaderSize - PacketFragmentHeader.HeaderSize

        public PacketFragmentHeader Header { get; protected set; }
        public byte[] Data { get; protected set; }

        public int Length
        {
            get
            {
                return Data.Length + (int)PacketFragmentHeader.HeaderSize;
            }
        }
    }
}