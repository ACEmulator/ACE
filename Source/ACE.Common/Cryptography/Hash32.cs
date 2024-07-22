using System;
using System.Buffers.Binary;

namespace ACE.Common.Cryptography
{
    public static class Hash32
    {
        public static uint Calculate(Span<byte> data, int length)
        {
            uint checksum = (uint)length << 16;

            for (int i = 0; i < length && i + 4 <= length; i += 4)
                checksum += BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(i));

            int shift = 3;
            int j = (length / 4) * 4;

            while (j < length)
                checksum += (uint)(data[j++] << (8 * shift--));

            return checksum;
        }

        public static uint Calculate(byte[] data, int offset, int length)
        {
            uint checksum = (uint)length << 16;

            for (int i = 0; i < length && i + 4 <= length; i += 4)
                checksum += BitConverter.ToUInt32(data, offset + i);

            int shift = 3;
            int j = (length / 4) * 4;

            while (j < length)
                checksum += (uint)(data[offset + j++] << (8 * shift--));

            return checksum;
        }
    }
}
