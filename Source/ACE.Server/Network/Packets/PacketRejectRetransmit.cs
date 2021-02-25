using System.Collections.Generic;

namespace ACE.Server.Network.Packets
{
    public class PacketRejectRetransmit : ServerPacket
    {
        public PacketRejectRetransmit(List<uint> sequences)
        {
            Header.Flags = PacketHeaderFlags.RejectRetransmit;

            InitializeDataWriter();

            DataWriter.Write(sequences.Count);

            foreach (var sequence in sequences)
                DataWriter.Write(sequence);
        }
    }
}
