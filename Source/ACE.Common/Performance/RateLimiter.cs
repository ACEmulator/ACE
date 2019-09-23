using System;
using System.Diagnostics;

namespace ACE.Common.Performance
{
    public class RateLimiter
    {
        private readonly int maxNumberOfEvents;
        private readonly double overPeriodInSeconds;
        private readonly double targetEventSpacingInSeconds;

        private readonly Stopwatch stopwatch = Stopwatch.StartNew();

        public RateLimiter(int maxNumberOfEvents, TimeSpan overPeriod)
        {
            if (maxNumberOfEvents <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxNumberOfEvents), $"{nameof(maxNumberOfEvents)} must be greater than 0");

            if (overPeriod <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(overPeriod), $"{nameof(overPeriod)} must be greater than TimeSpan.Zero");

            this.maxNumberOfEvents = maxNumberOfEvents;
            overPeriodInSeconds = overPeriod.TotalSeconds;

            targetEventSpacingInSeconds = overPeriodInSeconds / maxNumberOfEvents;
        }


        private int numberOfEventsRegistered;

        /// <summary>
        /// Result > 0 : We're able to meet our target rate and must pause between events.<para />
        /// Result = 0 : We're running right no time. A new event should be registered without delay.<para />
        /// Result &lt; 0 : We're running behind. A new event should be registered without delay. We're failing to meet our target rate.
        /// </summary>
        /// <returns></returns>
        public double GetSecondsToWaitBeforeNextEvent()
        {
            var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

            return ((targetEventSpacingInSeconds * numberOfEventsRegistered) - elapsedSeconds);
        }

        public void RegisterEvent()
        {
            numberOfEventsRegistered++;

            var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

            if (numberOfEventsRegistered > maxNumberOfEvents || elapsedSeconds > overPeriodInSeconds)
            {
                numberOfEventsRegistered = 1;

                stopwatch.Restart();
            }
        }
    }
}
