using System;
using System.Collections.Generic;

namespace ACE.Common.Performance
{
    public class RollingAmountOverTimeTracker
    {
        public readonly TimeSpan TimeBeingTracked;

        private readonly LinkedList<Tuple<DateTime, double>> amounts = new LinkedList<Tuple<DateTime, double>>();


        public double LastAmount { get; private set; }

        public long TotalAmounts => amounts.Count;

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

                foreach (var amount in amounts)
                {
                    if (amount.Item2 > largest)
                        largest = amount.Item2;
                }

                return largest;
            }
        }

        public RollingAmountOverTimeTracker(TimeSpan timeToTrack)
        {
            TimeBeingTracked = timeToTrack;
        }


        public void RegisterAmount(double amount)
        {
            LastAmount = amount;

            var now = DateTime.UtcNow;

            var pruneBefore = now - TimeBeingTracked;

            while (amounts.Count > 0 && amounts.First.Value.Item1 < pruneBefore)
            {
                Sum -= amounts.First.Value.Item2;

                amounts.RemoveFirst();
            }

            Sum += amount;

            amounts.AddLast(new Tuple<DateTime, double>(now, amount));
        }
    }
}
