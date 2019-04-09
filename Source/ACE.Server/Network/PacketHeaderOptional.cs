using ACE.Common.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.Server.Network
{
    public class PacketHeaderOptional
    {
        public uint Size { get; private set; }

        public uint AckSequence { get; private set; }
        public double TimeSynch { get; private set; }
        public float EchoRequestClientTime { get; private set; }
        public List<uint> RetransmitData { get; } = null;
        public bool IsValid { get; private set; } = true;
        public uint FlowBytes { get; private set; }
        public ushort FlowInterval { get; private set; }

        private PacketHeader Header { get; set; }
        private MemoryStream headerBytes = new MemoryStream();

        public PacketHeaderOptional(BinaryReader payload, PacketHeader header)
        {
            Header = header;
            Size = (uint)payload.BaseStream.Position;
            BinaryWriter writer = new BinaryWriter(headerBytes);

            if (header.HasFlag(PacketHeaderFlags.ServerSwitch)) // 0x100
            {
                writer.Write(payload.ReadBytes(8));
            }

            if (header.HasFlag(PacketHeaderFlags.RequestRetransmit)) // 0x1000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                uint retransmitCount = payload.ReadUInt32();
                writer.Write(retransmitCount);
                RetransmitData = new List<uint>();
                for (uint i = 0u; i < retransmitCount; i++)
                {
                    if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                    uint sequence = payload.ReadUInt32();
                    writer.Write(sequence);
                    RetransmitData.Add(sequence);
                }
            }

            if (header.HasFlag(PacketHeaderFlags.RejectRetransmit)) // 0x2000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                uint count = payload.ReadUInt32();
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                    writer.Write(payload.ReadBytes(4));
                }
            }

            if (header.HasFlag(PacketHeaderFlags.AckSequence)) // 0x4000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                AckSequence = payload.ReadUInt32();
                writer.Write(AckSequence);
            }

            if (header.HasFlag(PacketHeaderFlags.LoginRequest)) // 0x10000
            {
                long position = payload.BaseStream.Position;
                long length = payload.BaseStream.Length - position;
                if (length < 1) { IsValid = false; return; }
                byte[] loginBytes = new byte[length];
                payload.BaseStream.Read(loginBytes, (int)position, (int)length);
                writer.Write(loginBytes);
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.WorldLoginRequest)) // 0x20000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 8) { IsValid = false; return; }
                long position = payload.BaseStream.Position;
                writer.Write(payload.ReadBytes(8));
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.ConnectResponse)) // 0x80000
            {
                long position = payload.BaseStream.Position;
                if (payload.BaseStream.Length < payload.BaseStream.Position + 8) { IsValid = false; return; }
                writer.Write(payload.ReadBytes(8));
                payload.BaseStream.Position = position;
            }

            if (header.HasFlag(PacketHeaderFlags.CICMDCommand)) // 0x400000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 8) { IsValid = false; return; }
                writer.Write(payload.ReadBytes(8));
            }

            if (header.HasFlag(PacketHeaderFlags.TimeSync)) // 0x1000000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 8) { IsValid = false; return; }
                TimeSynch = payload.ReadDouble();
                writer.Write(TimeSynch);
            }

            if (header.HasFlag(PacketHeaderFlags.EchoRequest)) // 0x2000000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 4) { IsValid = false; return; }
                EchoRequestClientTime = payload.ReadSingle();
                writer.Write(EchoRequestClientTime);
            }

            if (header.HasFlag(PacketHeaderFlags.Flow)) // 0x8000000
            {
                if (payload.BaseStream.Length < payload.BaseStream.Position + 6) { IsValid = false; return; }
                FlowBytes = payload.ReadUInt32();
                FlowInterval = payload.ReadUInt16();
                writer.Write(FlowBytes);
                writer.Write(FlowInterval);
            }

            Size = (uint)payload.BaseStream.Position - Size;
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
