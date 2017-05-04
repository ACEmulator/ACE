using System;

namespace ACE.Common.Cryptography
{
    public static class Hash32
    {
        public static uint Calculate(byte[] data, int length)
        {
            uint checksum = (uint)length << 16;
            for (int i = 0; i < length && i + 4 <= length; i += 4)
                checksum += BitConverter.ToUInt32(data, i);

            int shift = 3;
            int j = (length / 4) * 4;
            while (j < length)
            {
                checksum += (uint)(data[j++] << (8 * shift--));
            }

            return checksum;
        }
    }
}
