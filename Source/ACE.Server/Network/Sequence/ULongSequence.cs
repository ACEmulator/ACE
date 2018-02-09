using System;

namespace ACE.Server.Network.Sequence
{
    public class ULongSequence : ISequence
    {
        private ulong maxValue = UInt64.MaxValue;

        public ULongSequence(ulong startingValue, ulong maxValue = UInt64.MaxValue)
        {
            CurrentValue = startingValue;
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
                CurrentValue = 0;
            else
                CurrentValue = maxValue;
        }

        public ulong CurrentValue { get; private set; }

        public ulong NextValue
        {
            get
            {
                if (CurrentValue == maxValue)
                {
                    CurrentValue = 0;
                    return CurrentValue;
                }
                return ++CurrentValue;
            }
        }

        public byte[] CurrentBytes => BitConverter.GetBytes(CurrentValue);

        public byte[] NextBytes => BitConverter.GetBytes(NextValue);
    }
}
