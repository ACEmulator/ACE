using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class ByteSequence : ISequence
    {
        private byte value;

        public ByteSequence(byte startingValue = 0)
        {
            value = startingValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                if (value == 0)
                    return BitConverter.GetBytes(Byte.MaxValue);
                return BitConverter.GetBytes(value - 1);
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == Byte.MaxValue)
                {
                    value = 0;
                    return BitConverter.GetBytes(Byte.MaxValue);
                }
                return BitConverter.GetBytes(value++);
            }
        }
    }
}
