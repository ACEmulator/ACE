using System.Diagnostics;

namespace ACE.Server.Physics.Common
{
    /// <summary>
    /// This should only be used by the ACE.Server.Physics namespace and physics related properties<para />
    /// For a equally precise timer outside of this namespace, you can use WorldManager.PortalYearTicks
    /// </summary>
    public class PhysicsTimer
    {
        private static readonly Stopwatch _timer;

        /// <summary>
        /// This should only be used by the ACE.Server.Physics namespace and physics related properties<para />
        /// For a equally precise timer outside of this namespace, you can use WorldManager.PortalYearTicks
        /// </summary>
        public static double CurrentTime => _timer.Elapsed.TotalSeconds;

        static PhysicsTimer()
        {
            _timer = Stopwatch.StartNew();
        }
    }
}
