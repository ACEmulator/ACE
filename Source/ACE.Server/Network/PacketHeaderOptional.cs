using System.Collections.Generic;
using System.IO;
using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class PacketHeaderOptional
    {
        public uint Size { get; private set; }

        public uint Sequence { get; private set; }
        public double TimeSynch { get; private set; }
        public float EchoRequestClientTime { get; private set; }
        public List<uint> RetransmitData { get; } = new List<uint>();

        private MemoryStream headerBytes = new MemoryStream();

        public byte[] Bytes => headerBytes.ToArray();

        public PacketHeaderOptional(BinaryReader payload, PacketHeader header)
        {
            Size = (uint)payload.BaseStream.Position;
            BinaryWriter writer = new BinaryWriter(headerBytes);

            if (header.HasFlag(PacketHeaderFlags.ServerSwitch)) // 0x100
                writer.Write(payload.ReadBytes(8));

            if (header.HasFlag(PacketHeaderFlags.RequestRetransmit)) // 0x1000
            {
                uint retransmitCount = payload.ReadUInt32();
                writer.Write(retransmitCount);
                for (uint i = 0u; i < retransmitCount; i++)
                {
                    uint sequence = payload.ReadUInt32();
                    writer.Write(sequence);
                    RetransmitData.Add(sequence);
                }
            }

            if (header.HasFlag(PacketHeaderFlags.RejectRetransmit)) // 0x2000
            {
                uint count = payload.ReadUInt32();
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    writer.Write(payload.ReadBytes(4));
                }
            }

            if (header.HasFlag(PacketHeaderFlags.AckSequence)) // 0x4000
            {
                Sequence = payload.ReadUInt32();
                writer.Write(Sequence);
            }

            if (header.HasFlag(PacketHeaderFlags.LoginRequest)) // 0x10000
            {
                var position = payload.BaseStream.Position;
                var length = payload.BaseStream.Length - position;
                byte[] loginBytes = new byte[length];
                payload.BaseStream.Read(loginBytes, (int)position, (int)length);
                writer.Write(loginBytes);
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.WorldLoginRequest)) // 0x20000
            {
                var position = payload.BaseStream.Position;
                writer.Write(payload.ReadBytes(8));
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.ConnectResponse)) // 0x80000
            {
                var position = payload.BaseStream.Position;
                writer.Write(payload.ReadBytes(8));
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.CICMDCommand)) // 0x400000
            {
                writer.Write(payload.ReadBytes(8));
            }

            if (header.HasFlag(PacketHeaderFlags.TimeSync)) // 0x1000000
            {
                TimeSynch = payload.ReadDouble();
                writer.Write(TimeSynch);
            }

            if (header.HasFlag(PacketHeaderFlags.EchoRequest)) // 0x2000000
            {
                EchoRequestClientTime = payload.ReadSingle();
                writer.Write(EchoRequestClientTime);
            }

            if (header.HasFlag(PacketHeaderFlags.Flow)) // 0x8000000
            {
                writer.Write(payload.ReadBytes(6));
            }

            Size = (uint)payload.BaseStream.Position - Size;
        }

        public uint CalculateHash32()
        {
            return Hash32.Calculate(Bytes, Bytes.Length);
        }
    }
}
