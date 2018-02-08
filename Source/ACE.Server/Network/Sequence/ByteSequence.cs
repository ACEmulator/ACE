using System;

namespace ACE.Server.Network.Sequence
{
    public class ByteSequence : ISequence
    {
        private byte value;
        private byte maxValue = Byte.MaxValue;

        public ByteSequence(byte startingValue, byte maxValue = Byte.MaxValue)
        {
            value = startingValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public ByteSequence(bool clientPrimed = true, byte maxValue = Byte.MaxValue)
        {
            this.maxValue = maxValue;
            if (clientPrimed)
                value = 0;
            else
                value = this.maxValue;
        }

        public byte CurrentValue
        {
            get
            {
                return value;
            }
        }

        public byte NextValue
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
                return new byte[] { CurrentValue };
            }
        }

        public byte[] NextBytes
        {
            get
            {
                return new byte[] { NextValue };
            }
        }
    }
}
