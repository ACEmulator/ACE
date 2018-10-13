using System.Diagnostics;

namespace ACE.Common
{
    public class RateMonitor
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Last event duration in seconds
        /// </summary>
        public double LastEvent { get; private set; }

        public long TotalEvents { get; private set; }

        public double TotalSeconds { get; private set; }

        /// <summary>
        /// Longest event duration in seconds
        /// </summary>
        public double LongestEvent { get; private set; }

        /// <summary>
        /// Shortest event duration in seconds
        /// </summary>
        public double ShortestEvent { get; private set; }

        /// <summary>
        /// Average event duration in seconds
        /// </summary>
        public double AverageEventDuration => TotalSeconds / TotalEvents;

        public void RegisterEventStart()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        /// <summary>
        /// returns the elapsed seconds for this event
        /// </summary>
        public double RegisterEventEnd()
        {
            stopwatch.Stop();

            LastEvent = stopwatch.Elapsed.TotalSeconds;

            TotalEvents++;
            TotalSeconds += LastEvent;

            if (LastEvent > LongestEvent)
                LongestEvent = LastEvent;

            if (LastEvent < ShortestEvent)
                ShortestEvent = LastEvent;

            return LastEvent;
        }

        public void ClearEventHistory()
        {
            LastEvent = 0;
            TotalEvents = 0;
            TotalSeconds = 0;
            LongestEvent = 0;
            ShortestEvent = 0;
        }

        public override string ToString()
        {
            return $"Total Events: {TotalEvents:N0}, Average: {AverageEventDuration:N4} s, Longest: {LongestEvent:N4} s, Shortest: {ShortestEvent:N4} s, Last: {LastEvent:N4} s";
        }
    }
}
