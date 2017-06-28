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
        private uint maxValue = UInt32.MaxValue;

        public UIntSequence(uint startingValue, uint maxValue = UInt32.MaxValue)
        {
            value = startingValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public UIntSequence(bool clientPrimed = true, uint maxValue = UInt32.MaxValue)
        {
            this.maxValue = maxValue;
            if (clientPrimed)
                value = 0;
            else
                value = this.maxValue;
        }

        public uint CurrentValue
        {
            get
            {
                return value;
            }
        }
        public uint NextValue
        {
            get
            {
                if (value == maxValue)
                {
                    value = 0;
                    return value;
                }
                return ++value;
            }
        }

        public byte[] CurrentBytes
        {
            get
            {
                return BitConverter.GetBytes(CurrentValue);
            }
        }

        public byte[] NextBytes
        {
            get
            {
                return BitConverter.GetBytes(NextValue);
            }
        }
    }
}
