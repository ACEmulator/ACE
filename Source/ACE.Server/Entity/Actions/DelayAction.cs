using System;
using System.Threading;

namespace ACE.Server.Entity.Actions
{
    /// <summary>
    /// Action that will not return until Timer.PortalYearTicks >= EndTime
    /// must only be inserted into DelayManager actor
    /// </summary>
    public class DelayAction : ActionEventBase, IComparable<DelayAction>
    {
        public double WaitTime { get; }
        public double EndTime { get; private set; }

        // For breaking ties on compareto, two actions cannot be equal
        private readonly long sequence;
        private static long glblSequence;

        public DelayAction(double waitTimePortalYearTicks)
        {
            WaitTime = waitTimePortalYearTicks;
            sequence = Interlocked.Increment(ref glblSequence);
        }

        public void Start()
        {
            EndTime = Timers.PortalYearTicks + WaitTime;
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
