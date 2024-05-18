using System;

namespace ACE.Common.Performance
{
    /// <summary>
    /// This class represents a circular collection of NumberOfEventsBeingTracked elements.
    /// It can be used to track total elapsed time over a set number of events.
    /// </summary>
    public class RollingTimeSpanTracker
    {
        public readonly int NumberOfEventsBeingTracked;

        private readonly TimeSpan[] trackedEvents;

        private int nextIndex = 0;

        public TimeSpan Elapsed { get; private set; }

        public RollingTimeSpanTracker(int numberOfEventsToTrack)
        {
            NumberOfEventsBeingTracked = numberOfEventsToTrack;

            trackedEvents = new TimeSpan[numberOfEventsToTrack];
        }

        public void Add(TimeSpan timeSpan)
        {
            if (nextIndex == NumberOfEventsBeingTracked)
                nextIndex = 0;

            Elapsed -= trackedEvents[nextIndex];
            trackedEvents[nextIndex] = timeSpan;
            Elapsed += trackedEvents[nextIndex];

            nextIndex++;
        }
    }
}
