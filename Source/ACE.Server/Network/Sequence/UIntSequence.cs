using System;

namespace ACE.Server.Network.Sequence
{
    public class UIntSequence : ISequence
    {
        private uint maxValue = UInt32.MaxValue;

        public UIntSequence(uint startingValue, uint maxValue = UInt32.MaxValue)
        {
            CurrentValue = startingValue;
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
                CurrentValue = 0;
            else
                CurrentValue = this.maxValue;
        }

        public uint CurrentValue { get; private set; }

        public uint NextValue
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
