using System;

namespace ACE.Common.Performance
{
    public class RollingAmountOverHitsTracker
    {
        public readonly long HitsBeingTracked;

        private readonly double[] amounts;

        private int nextIndex = 0;

        private bool rolledOver;


        public double LastAmount { get; private set; }

        public long TotalAmounts
        {
            get
            {
                if (rolledOver)
                    return HitsBeingTracked;

                return nextIndex;
            }
        }

        public double Sum { get; private set; }

        public double AverageAmount => TotalAmounts == 0 ? 0 : Sum / TotalAmounts;

        /// <summary>
        /// Use this sparingly as it iterates over the entire collection
        /// </summary>
        public double LargestAmount
        {
            get
            {
                if (TotalAmounts == 0)
                    return 0;

                var largest = double.MinValue;

                for (int i = 0; i < TotalAmounts; i++)
                {
                    if (amounts[i] > largest)
                        largest = amounts[i];
                }

                return largest;
            }
        }

        public RollingAmountOverHitsTracker(long hitsToTrack)
        {
            HitsBeingTracked = hitsToTrack;

            amounts = new double[hitsToTrack];
        }


        public void RegisterAmount(double amount)
        {
            LastAmount = amount;

            Sum -= amounts[nextIndex];
            amounts[nextIndex] = amount;
            Sum += amounts[nextIndex];

            nextIndex++;

            if (nextIndex == HitsBeingTracked)
            {
                nextIndex = 0;
                rolledOver = true;
            }
        }
    }
}
