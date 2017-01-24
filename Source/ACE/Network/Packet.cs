using ACE.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ACE.Network
{
    [Flags]
    public enum PacketHeaderFlags : uint
    {
        None              = 0x00000000,
        EncryptedChecksum = 0x00000002, // can't be paired with 0x00000001, see FlowQueue::DequeueAck
        BlobFragments     = 0x00000004,
        ServerSwitch      = 0x00000100,
        Referral          = 0x00000800,
        RequestRetransmit = 0x00001000,
        RejectRetransmit  = 0x00002000,
        AckSequence       = 0x00004000,
        Disconnect        = 0x00008000,
        LoginRequest      = 0x00010000,
        WorldLoginRequest = 0x00020000,
        ConnectRequest    = 0x00040000,
        ConnectResponse   = 0x00080000,
        CICMDCommand      = 0x00400000,
        TimeSynch         = 0x01000000,
        EchoRequest       = 0x02000000,
        EchoResponse      = 0x04000000,
        Flow              = 0x08000000
    }

    public enum PacketDirection
    {
        None,
        Client, // C->S
        Server  // S->C
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketFragementHeader
    {
        public static uint HeaderSize { get; } = 16u;

        public uint Sequence { get; set; }
        public uint Id { get; set; }
        public ushort Count { get; set; }
        public ushort Size { get; set; }
        public ushort Index { get; set; }
        public ushort Group { get; set; }

        public PacketFragementHeader() { }

        public PacketFragementHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Id       = payload.ReadUInt32();
            Count    = payload.ReadUInt16();
            Size     = payload.ReadUInt16();
            Index    = payload.ReadUInt16();
            Group    = payload.ReadUInt16();
        }

        public byte[] GetRaw()
        {
            var headerHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                byte[] bytes = new byte[Marshal.SizeOf(typeof(PacketFragementHeader))];
                Marshal.Copy(headerHandle.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                headerHandle.Free();
            }
        }
    }

    public abstract class PacketFragment
    {
        public static uint MaxFragementSize { get; } = 464u; // Packet.MaxPacketSize - PacketHeader.HeaderSize

        public PacketFragementHeader Header { get; protected set; }
        public MemoryStream Data { get; protected set; }
    }

    public class ClientPacketFragment : PacketFragment
    {
        public BinaryReader Payload { get; }

        public ClientPacketFragment(BinaryReader payload)
        {
            Header  = new PacketFragementHeader(payload);
            Data    = new MemoryStream(payload.ReadBytes((int)(Header.Size - PacketFragementHeader.HeaderSize)));
            Payload = new BinaryReader(Data);
        }
    }

    public class ServerPacketFragment : PacketFragment
    {
        public BinaryWriter Payload { get; }
        public FragmentOpcode Opcode { get; }

        public ServerPacketFragment(ushort group, FragmentOpcode opcode = FragmentOpcode.None)
        {
            Opcode  = opcode;

            Data    = new MemoryStream((int)MaxFragementSize);
            Payload = new BinaryWriter(Data);
            Header  = new PacketFragementHeader()
            {
                Group = group
            };

            if (opcode != FragmentOpcode.None)
                Payload.Write((uint)opcode);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketHeader
    {
        public static uint HeaderSize { get; } = 20u;

        public uint Sequence { get; set; }
        public PacketHeaderFlags Flags { get; set; }
        public uint Checksum { get; set; }
        public ushort Id { get; set; }
        public ushort Time { get; set; }
        public ushort Size { get; set; }
        public ushort Table { get; set; }

        public PacketHeader() { }

        public PacketHeader(BinaryReader payload)
        {
            Sequence = payload.ReadUInt32();
            Flags    = (PacketHeaderFlags)payload.ReadUInt32();
            Checksum = payload.ReadUInt32();
            Id       = payload.ReadUInt16();
            Time     = payload.ReadUInt16();
            Size     = payload.ReadUInt16();
            Table    = payload.ReadUInt16();
        }

        public byte[] GetRaw()
        {
            var headerHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                byte[] bytes = new byte[Marshal.SizeOf(typeof(PacketHeader))];
                Marshal.Copy(headerHandle.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                headerHandle.Free();
            }
        }

        public void CalculateHash32(out uint checksum)
        {
            uint original = Checksum;

            Checksum = 0x0BADD70DD;
            byte[] rawHeader = GetRaw();
            checksum = Hash32.Calculate(rawHeader, rawHeader.Length);
            Checksum = original;
        }

        public bool HasFlag(PacketHeaderFlags flags) { return (flags & Flags) != 0; }
    }

    public class PacketHeaderOptional
    {
        public uint Size { get; private set; }

        public uint Sequence { get; private set; }
        public double TimeSynch { get; private set; }
        public float ClientTime { get; private set; }

        public PacketHeaderOptional(BinaryReader payload, PacketHeader header)
        {
            Size = (uint)payload.BaseStream.Position;

            if (header.HasFlag(PacketHeaderFlags.ServerSwitch))
                payload.Skip(8);

            if (header.HasFlag(PacketHeaderFlags.RequestRetransmit))
                payload.Skip(payload.ReadUInt32() * sizeof(uint));

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

    public abstract class Packet
    {
        public static uint MaxPacketSize { get; } = 484u;
        public static uint MaxDataSize { get; } = 448u;

        public PacketHeader Header { get; protected set; }
        public PacketHeaderOptional HeaderOptional { get; protected set; }
        public MemoryStream Data { get; protected set; }
        public PacketDirection Direction { get; protected set; } = PacketDirection.None;
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();

        public uint CalculateChecksum(Session session, ConnectionType type)
        {
            uint headerChecksum = 0u;
            Header.CalculateHash32(out headerChecksum);

            uint xor = (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? session.GetIssacValue(Direction, type) : 0u);
            return headerChecksum + (CalculatePayloadChecksum() ^ xor);
        }

        private uint CalculatePayloadChecksum()
        {
            uint checksum = 0u;
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
            {
                if (HeaderOptional != null)
                {
                    // FIXME: packets with both optional headers and fragments fail to verify
                    byte[] optionalHeader = new byte[HeaderOptional.Size];
                    Buffer.BlockCopy(Data.ToArray(), 0, optionalHeader, 0, (int)HeaderOptional.Size);

                    checksum += Hash32.Calculate(optionalHeader, (int)HeaderOptional.Size);
                }

                foreach (var fragment in Fragments)
                    checksum += Hash32.Calculate(fragment.Header.GetRaw(), (int)PacketFragementHeader.HeaderSize) + Hash32.Calculate(fragment.Data.ToArray(), fragment.Header.Size - (int)PacketFragementHeader.HeaderSize);
            }
            else
                checksum += Hash32.Calculate(Data.ToArray(), (int)Data.Length);

            return checksum;
        }
    }

    public class ClientPacket : Packet
    {
        public BinaryReader Payload { get; }

        public ClientPacket(byte[] data, bool debug = false)
        {
            Direction = (debug ? PacketDirection.Server : PacketDirection.Client);

            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    Header         = new PacketHeader(reader);
                    Data           = new MemoryStream(reader.ReadBytes(Header.Size), 0, Header.Size, false, true);
                    Payload        = new BinaryReader(Data);
                    HeaderOptional = new PacketHeaderOptional(Payload, Header);
                }
            }

            ReadFragments();
        }

        private void ReadFragments()
        {
            if (Header.HasFlag(PacketHeaderFlags.BlobFragments))
                while (Payload.BaseStream.Position != Payload.BaseStream.Length)
                    Fragments.Add(new ClientPacketFragment(Payload));
        }
    }

    public class ServerPacket : Packet
    {
        public BinaryWriter Payload { get; }

        public ServerPacket(ushort id, PacketHeaderFlags flags = PacketHeaderFlags.None)
        {
            Direction = PacketDirection.Server;

            Data      = new MemoryStream((int)MaxPacketSize);
            Payload   = new BinaryWriter(Data);
            Header    = new PacketHeader()
            {
                Id    = id,
                Flags = flags
            };

        }
    }
}
