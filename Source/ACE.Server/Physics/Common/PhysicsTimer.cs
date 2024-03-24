using System;

using ACE.Server.Entity;

namespace ACE.Server.Physics.Common
{
    /// <summary>
    /// This should only be used by the ACE.Server.Physics namespace and physics related properties<para />
    /// For a equally precise timer outside of this namespace, you can use WorldManager.PortalYearTicks
    /// </summary>
    public class PhysicsTimer
    {
        // When using PhysicsTimer outside of a running ACE instance, you must uncomment these lines and use this version of the timer. Otherwise, your CurrentTime will not increment.
        // You can use this method when running in an ACE instance, it's just less efficient.
        /*private static readonly System.Diagnostics.Stopwatch _timer;

        public static double CurrentTime => _timer.Elapsed.TotalSeconds;

        static PhysicsTimer()
        {
            _timer = System.Diagnostics.Stopwatch.StartNew();
        }*/


        // When using PhysicsTimer in a running ACE instance, you should use this timer instead. It is more efficient. Timers.PortalYearTicks is incremented by the WorldManager.
        public static double CurrentTime => Timers.PortalYearTicks;
    }
}
