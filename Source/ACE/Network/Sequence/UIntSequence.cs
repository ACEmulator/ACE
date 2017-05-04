using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Sequence
{
    public class UIntSequence : ISequence
    {
        private uint value;

        public UIntSequence(uint startingValue)
        {
            value = startingValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public UIntSequence(bool clientPrimed = true)
        {
            if (clientPrimed)
                value = 0;
            else
                value = UInt32.MaxValue;
        }

        public byte[] CurrentValue
        {
            get
            {
                return BitConverter.GetBytes(value);
            }
        }

        public byte[] NextValue
        {
            get
            {
                if (value == UInt32.MaxValue)
                {
                    value = 0;
                    return BitConverter.GetBytes(value);
                }
                return BitConverter.GetBytes(++value);
            }
        }
    }
}
