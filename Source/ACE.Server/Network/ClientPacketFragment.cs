using System.Buffers;
using System.IO;

using ACE.Common.Cryptography;

namespace ACE.Server.Network
{
    public class ClientPacketFragment : PacketFragment
    {
        public void Unpack(BinaryReader payload)
        {
            Header.Unpack(payload);
            Data = payload.ReadBytes(Header.Size - PacketFragmentHeader.HeaderSize);
        }

        public uint CalculateHash32()
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(PacketFragmentHeader.HeaderSize);

            try
            {
                Header.Pack(buffer);

                uint fragmentChecksum = Hash32.Calculate(buffer, buffer.Length) + Hash32.Calculate(Data, Data.Length);

                return fragmentChecksum;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
