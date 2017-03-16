using System.Collections.Generic;
using System.IO;

using ACE.Common.Extensions;

namespace ACE.Network
{
    public class PacketHeaderOptional
    {
        public uint Size { get; private set; }

        public uint Sequence { get; private set; }
        public double TimeSynch { get; private set; }
        public float ClientTime { get; private set; }

        public List<uint> RetransmitData { get; } = new List<uint>();

        public PacketHeaderOptional(BinaryReader payload, PacketHeader header)
        {
            Size = (uint)payload.BaseStream.Position;

            if (header.HasFlag(PacketHeaderFlags.ServerSwitch))
                payload.Skip(8);

            if (header.HasFlag(PacketHeaderFlags.RequestRetransmit))
            {
                uint retransmitCount = payload.ReadUInt32();
                for (uint i = 0u; i < retransmitCount; i++)
                    RetransmitData.Add(payload.ReadUInt32());
            }

            if (header.HasFlag(PacketHeaderFlags.RejectRetransmit))
                payload.Skip(payload.ReadUInt32() * sizeof(uint));

            if (header.HasFlag(PacketHeaderFlags.AckSequence))
                Sequence = payload.ReadUInt32();

            if (header.HasFlag(PacketHeaderFlags.CICMDCommand))
                payload.Skip(8);

            if (header.HasFlag(PacketHeaderFlags.TimeSynch))
                TimeSynch = payload.ReadDouble();

            if (header.HasFlag(PacketHeaderFlags.EchoRequest))
                ClientTime = payload.ReadSingle();

            if (header.HasFlag(PacketHeaderFlags.Flow))
                payload.Skip(6);

            Size = (uint)payload.BaseStream.Position - Size;
        }
    }

}
