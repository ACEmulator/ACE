using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class PacketHeaderOptional
    {
        public uint Size { get; private set; }

        public uint AckSequence { get; private set; }
        public double TimeSynch { get; private set; }
        public float EchoRequestClientTime { get; private set; }
        public List<uint> RetransmitData { get; private set; }
        public bool IsValid { get; private set; } = true;
        public uint FlowBytes { get; private set; }
        public ushort FlowInterval { get; private set; }

        private PacketHeader Header { get; set; }
        private MemoryStream headerBytes = new MemoryStream();

        public void Unpack(BinaryReader reader, PacketHeader header)
        {
            Header = header;
            Size = (uint)reader.BaseStream.Position;
            BinaryWriter writer = new BinaryWriter(headerBytes);

            if (header.HasFlag(PacketHeaderFlags.ServerSwitch)) // 0x100
            {
                writer.Write(reader.ReadBytes(8));
            }

            if (header.HasFlag(PacketHeaderFlags.RequestRetransmit)) // 0x1000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                uint retransmitCount = reader.ReadUInt32();
                writer.Write(retransmitCount);
                RetransmitData = new List<uint>();
                for (uint i = 0u; i < retransmitCount; i++)
                {
                    if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                    uint sequence = reader.ReadUInt32();
                    writer.Write(sequence);
                    RetransmitData.Add(sequence);
                }
            }

            if (header.HasFlag(PacketHeaderFlags.RejectRetransmit)) // 0x2000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                uint count = reader.ReadUInt32();
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                    writer.Write(reader.ReadBytes(4));
                }
            }

            if (header.HasFlag(PacketHeaderFlags.AckSequence)) // 0x4000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                AckSequence = reader.ReadUInt32();
                writer.Write(AckSequence);
            }

            if (header.HasFlag(PacketHeaderFlags.LoginRequest)) // 0x10000
            {
                long position = reader.BaseStream.Position;
                long length = reader.BaseStream.Length - position;
                if (length < 1) { IsValid = false; return; }
                byte[] loginBytes = new byte[length];
                reader.BaseStream.Read(loginBytes, (int)position, (int)length);
                writer.Write(loginBytes);
                reader.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.WorldLoginRequest)) // 0x20000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 8) { IsValid = false; return; }
                long position = reader.BaseStream.Position;
                writer.Write(reader.ReadBytes(8));
                reader.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.ConnectResponse)) // 0x80000
            {
                long position = reader.BaseStream.Position;
                if (reader.BaseStream.Length < reader.BaseStream.Position + 8) { IsValid = false; return; }
                writer.Write(reader.ReadBytes(8));
                reader.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.CICMDCommand)) // 0x400000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 8) { IsValid = false; return; }
                writer.Write(reader.ReadBytes(8));
            }

            if (header.HasFlag(PacketHeaderFlags.TimeSync)) // 0x1000000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 8) { IsValid = false; return; }
                TimeSynch = reader.ReadDouble();
                writer.Write(TimeSynch);
            }

            if (header.HasFlag(PacketHeaderFlags.EchoRequest)) // 0x2000000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 4) { IsValid = false; return; }
                EchoRequestClientTime = reader.ReadSingle();
                writer.Write(EchoRequestClientTime);
            }

            if (header.HasFlag(PacketHeaderFlags.Flow)) // 0x8000000
            {
                if (reader.BaseStream.Length < reader.BaseStream.Position + 6) { IsValid = false; return; }
                FlowBytes = reader.ReadUInt32();
                FlowInterval = reader.ReadUInt16();
                writer.Write(FlowBytes);
                writer.Write(FlowInterval);
            }

            Size = (uint)reader.BaseStream.Position - Size;
        }

        public uint CalculateHash32()
        {
            var bytes = headerBytes.GetBuffer();

            return Hash32.Calculate(bytes, (int)headerBytes.Length);
        }

        public override string ToString()
        {
            if (Size == 0 || !IsValid)
            {
                return string.Empty;
            }

            string nice = "";
            if (Header.HasFlag(PacketHeaderFlags.Flow))
            {
                nice = $" {FlowBytes} Interval: {FlowInterval}";
            }
            if (RetransmitData != null)
            {
                nice += $" requesting {RetransmitData.DefaultIfEmpty().Select(t => t.ToString()).Aggregate((a, b) => a + "," + b)}";
            }
            if (Header.HasFlag(PacketHeaderFlags.AckSequence))
            {
                nice += $" AckSeq: {AckSequence}";
            }
            return nice.Trim();
        }
    }
}
