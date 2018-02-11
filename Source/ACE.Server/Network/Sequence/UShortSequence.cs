using System;

namespace ACE.Server.Network.Sequence
{
    public class UShortSequence : ISequence
    {
        private ushort maxValue = UInt16.MaxValue;

        public UShortSequence(ushort startingValue, ushort maxValue = UInt16.MaxValue)
        {
            CurrentValue = startingValue;
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
                CurrentValue = 0;
            else
                CurrentValue = maxValue;
        }

        public ushort CurrentValue { get; private set; }

        public ushort NextValue
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
