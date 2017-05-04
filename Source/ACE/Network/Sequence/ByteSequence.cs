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

        public ByteSequence(byte startingValue)
        {
            value = startingValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public ByteSequence(bool clientPrimed = true)
        {
            if (clientPrimed)
                value = 0;
            else
                value = Byte.MaxValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                return new byte[] { value };
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == Byte.MaxValue)
                {
                    value = 0;
                    return new byte[] { value };
                }
                return new byte[] { ++value };
            }
        }
    }
}
