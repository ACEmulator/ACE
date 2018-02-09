using System;

namespace ACE.Server.Network.Sequence
{
    public class ByteSequence : ISequence
    {
        private byte maxValue = Byte.MaxValue;

        public ByteSequence(byte startingValue, byte maxValue = Byte.MaxValue)
        {
            CurrentValue = startingValue;
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
                CurrentValue = 0;
            else
                CurrentValue = this.maxValue;
        }

        public byte CurrentValue { get; private set; }

        public byte NextValue
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

        public byte[] CurrentBytes => new byte[] { CurrentValue };

        public byte[] NextBytes => new byte[] { NextValue };
    }
}
