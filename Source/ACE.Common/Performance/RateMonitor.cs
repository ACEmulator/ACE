using System.Diagnostics;

namespace ACE.Common.Performance
{
    public class RateMonitor
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        public readonly TimedEventHistory EventHistory = new TimedEventHistory();

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        public void Reset()
        {
            stopwatch.Reset();
        }

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        public void Restart()
        {
            stopwatch.Restart();
        }

        /// <summary>
        /// Stops time interval measurement.
        /// </summary>
        public void Pause()
        {
            stopwatch.Stop();
        }

        /// <summary>
        /// Starts time interval measurement.
        /// </summary>
        public void Resume()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Stops time interval measurement.
        /// </summary>
        public void RegisterEventEnd()
        {
            stopwatch.Stop();

            RegisterEvent(stopwatch.Elapsed.TotalSeconds);
        }

        public void RegisterEvent(double totalSeconds)
        {
            EventHistory.RegisterEvent(totalSeconds);
        }

        public void ClearEventHistory()
        {
            EventHistory.ClearHistory();
        }

        public override string ToString()
        {
            return EventHistory.ToString();
        }
    }
}
