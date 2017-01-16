using System;

namespace ACE.Cryptography
{
    public class Hash32
    {
        public static uint Calculate(byte[] data, int length)
        {
            uint checksum = (uint)length << 16;
            for (int i = 0; i < length && i + 4 <= length; i += 4)
                checksum += BitConverter.ToUInt32(data, i);    

            int shift = 24;
            for (int i = (length / 4) * 4; i < length; i++)
            {
                checksum += (byte)(data[i] << shift);
                shift -= 8;
            }

            return checksum;
        }
    }
}
