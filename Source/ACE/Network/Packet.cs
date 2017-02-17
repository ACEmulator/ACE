using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using ACE.Common.Cryptography;
using ACE.Common.Extensions;
using ACE.Network.Fragments;

namespace ACE.Network
{
    public abstract class Packet
    {
        public static uint MaxPacketSize { get; } = 484u;

        public PacketHeader Header { get; protected set; }
        public PacketHeaderOptional HeaderOptional { get; protected set; }
        public MemoryStream Data { get; protected set; }
        public PacketDirection Direction { get; protected set; } = PacketDirection.None;
        public List<PacketFragment> Fragments { get; } = new List<PacketFragment>();

        public uint CalculateChecksum(Session session, ConnectionType type, uint overrideXor, out uint issacXor)
        {
            uint headerChecksum = 0u;
            Header.CalculateHash32(out headerChecksum);

            issacXor = (Header.HasFlag(PacketHeaderFlags.EncryptedChecksum) ? (overrideXor != 0u ? overrideXor : session.GetIssacValue(Direction, type)) : 0u);
            return headerChecksum + (CalculatePayloadChecksum() ^ issacXor);
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
                    checksum += Hash32.Calculate(fragment.Header.GetRaw(), (int)PacketFragmentHeader.HeaderSize) + Hash32.Calculate(fragment.Data.ToArray(), fragment.Header.Size - (int)PacketFragmentHeader.HeaderSize);
            }
            else
                checksum += Hash32.Calculate(Data.ToArray(), (int)Data.Length);

            return checksum;
        }
    }
}
