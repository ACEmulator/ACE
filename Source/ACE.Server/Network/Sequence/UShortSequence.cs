using System;

namespace ACE.Server.Network.Sequence
{
    public class UShortSequence : ISequence
    {
        private ushort value;
        private ushort maxValue = UInt16.MaxValue;

        public UShortSequence(ushort startingValue, ushort maxValue = UInt16.MaxValue)
        {
            value = startingValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public UShortSequence(bool clientPrimed = true, ushort maxValue = UInt16.MaxValue)
        {
            this.maxValue = maxValue;
            if (clientPrimed)
                value = 0;
            else
                value = maxValue;
        }

        public ushort CurrentValue
        {
            get
            {
                return value;
            }
        }
        public ushort NextValue
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
