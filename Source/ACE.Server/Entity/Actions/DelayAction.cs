using System;
using ACE.Server.Managers;

namespace ACE.Server.Entity.Actions
{
    /// <summary>
    /// Action that will not return until WorldManager.PortalTickYears >= EndTime
    /// must only be inserted into DelayManager actor
    /// </summary>
    public class DelayAction : ActionEventBase, IComparable<DelayAction>
    {
        public double WaitTime { get; private set; }
        public double EndTime { get; private set; }

        // For breaking ties on compareto, two actions cannot be equal
        private long sequence;
        private static volatile uint glblSequence;

        public DelayAction(double waitTimePortalTickYears)
        {
            WaitTime = waitTimePortalTickYears;
            sequence = glblSequence++;
        }

        public void Start()
        {
            EndTime = WorldManager.PortalYearTicks + WaitTime;
        }

        public int CompareTo(DelayAction rhs)
        {
            int ret = EndTime.CompareTo(rhs.EndTime);
            if (ret == 0)
            {
                return sequence.CompareTo(rhs.sequence);
            }
            return ret;
        }
    }
}
