using System;

namespace ACE.Server.Network.Sequence
{
    public class ULongSequence : ISequence
    {
        private ulong value;
        private ulong maxValue = UInt64.MaxValue;

        public ULongSequence(ulong startingValue, ulong maxValue = UInt64.MaxValue)
        {
            value = startingValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Creates an instance without a starting value
        /// </summary>
        /// <param name="clientPrimed">Whether the value gets sent to client before first increment</param>
        public ULongSequence(bool clientPrimed = true, ulong maxValue = UInt64.MaxValue)
        {
            this.maxValue = maxValue;
            if (clientPrimed)
                value = 0;
            else
                value = maxValue;
        }

        public ulong CurrentValue
        {
            get
            {
                return value;
            }
        }

        public ulong NextValue
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
