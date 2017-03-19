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

        public byte[] NextValue
        {
            get
            {
                return BitConverter.GetBytes(++value);
            }
        }
    }
}
