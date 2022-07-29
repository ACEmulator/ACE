using System;

namespace ACE.Common.Cryptography
{
    public class ISAAC
    {
        private uint offset;

        private uint a, b, c;
        private uint[] mm;
        private uint[] randRsl;

        public ISAAC(byte[] seed)
        {
            mm      = new uint[256];
            randRsl = new uint[256];
            offset  = 255u;

            Initialize(seed);
        }

        private bool isReleased;

        public void ReleaseResources()
        {
            isReleased = true;
            mm = null;
            randRsl = null;
        }

        public uint Next()
        {
            if (isReleased)
            {
                return 0;
            }

            var issacValue = randRsl[offset];
            if (offset > 0)
                offset--;
            else
            {
                IsaacScramble();
                offset = 255u;
            }

            return issacValue;
        }

        private void Initialize(byte[] keyBytes)
        {
            int i;
            for (i = 0; i < 256; i++)
                mm[i] = randRsl[i] = 0;

            uint[] abcdefgh = new uint[8];
            for (i = 0; i < 8; i++)
                abcdefgh[i] = 0x9E3779B9;

            for (i = 0; i < 4; i++)
                Shuffle(abcdefgh);

            for (i = 0; i < 2; i++)
            {
                int j;
                for (j = 0; j < 256; j += 8)
                {
                    int k;
                    for (k = 0; k < 8; k++)
                        abcdefgh[k] += (i < 1) ? randRsl[j + k] : mm[j + k];

                    Shuffle(abcdefgh);

                    for (k = 0; k < 8; k++)
                        mm[j + k] = abcdefgh[k];
                }
            }

            a = BitConverter.ToUInt32(keyBytes, 0);
            c = b = a;

            IsaacScramble();
        }

        private void IsaacScramble()
        {
            b += ++c;
            for (int i = 0; i < 256; i++)
            {
                var x = mm[i];
                switch (i & 3)
                {
                    case 0: a ^= (a << 0x0D);
                        break;
                    case 1: a ^= (a >> 0x06);
                        break;
                    case 2: a ^= (a << 0x02);
                        break;
                    case 3: a ^= (a >> 0x10);
                        break;
                    default:
                        break;
                }

                a += mm[(i + 128) & 0xFF];

                uint y;
                mm[i]      = y = mm[(int)(x >> 2) & 0xFF] + a + b;
                randRsl[i] = b = mm[(int)(y >> 10) & 0xFF] + x;
            }
        }

        private void Shuffle(uint[] x)
        {
            x[0] ^= x[1] << 0x0B; x[3] += x[0]; x[1] += x[2];
            x[1] ^= x[2] >> 0x02; x[4] += x[1]; x[2] += x[3];
            x[2] ^= x[3] << 0x08; x[5] += x[2]; x[3] += x[4];
            x[3] ^= x[4] >> 0x10; x[6] += x[3]; x[4] += x[5];
            x[4] ^= x[5] << 0x0A; x[7] += x[4]; x[5] += x[6];
            x[5] ^= x[6] >> 0x04; x[0] += x[5]; x[6] += x[7];
            x[6] ^= x[7] << 0x08; x[1] += x[6]; x[7] += x[0];
            x[7] ^= x[0] >> 0x09; x[2] += x[7]; x[0] += x[1];
        }
    }
}
