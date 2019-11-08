using System;
using System.Threading;

namespace ACE.Server.Entity.Actions
{
    /// <summary>
    /// Action that will not return until Time.GetUnixTime() >= EndTime
    /// must only be inserted into DelayManager actor
    /// </summary>
    public class DelayAction : ActionEventBase, IComparable<DelayAction>
    {
        public enum DelayType
        {
            FixedDelay,
            DelayUntil,
        }

        private readonly DelayType delayType;

        public double WaitTime { get; }
        public double EndTime { get; private set; }

        // For breaking ties on compareto, two actions cannot be equal
        private readonly long sequence;
        private static long glblSequence;

        public DelayAction(DelayType delayType, double timeInSeconds)
        {
            this.delayType = delayType;

            if (delayType == DelayType.FixedDelay)
                WaitTime = timeInSeconds;
            else if (delayType == DelayType.DelayUntil)
                EndTime = timeInSeconds;

            sequence = Interlocked.Increment(ref glblSequence);
        }

        public void Start(double startTime)
        {
            if (delayType == DelayType.FixedDelay)
                EndTime = startTime + WaitTime;
        }

        public int CompareTo(DelayAction rhs)
        {
            int ret = EndTime.CompareTo(rhs.EndTime);

            if (ret == 0)
                return sequence.CompareTo(rhs.sequence);

            return ret;
        }
    }
}
