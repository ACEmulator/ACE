using System;
using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ServerPacketFragment : PacketFragment
    {
        private readonly ReadOnlyMemory<byte> payload;

        public override int Length =>
            PacketFragmentHeader.HeaderSize + payload.Length;
        
        public ServerPacketFragment(ReadOnlyMemory<byte> payload)
        {
            this.payload = payload;
        }

        /// <summary>
        /// Returns the Hash32 of the payload added to buffer
        /// </summary>
        public uint PackAndReturnHash32(byte[] buffer, ref int offset)
        {
            Header.Size = 
                (ushort)(PacketFragmentHeader.HeaderSize + payload.Length);

            var headerHash32 = 
                Header.PackAndReturnHash32(buffer, ref offset);

            // Copies directly from our read-only view into the final packet.
            payload.Span.CopyTo(
                buffer.AsSpan(offset, payload.Length));

            offset += payload.Length;

            return headerHash32 + 
                Hash32.Calculate(payload.Span, payload.Length);
        }
    }
}
