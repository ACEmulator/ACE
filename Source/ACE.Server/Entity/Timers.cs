using System;

using ACE.Common;

namespace ACE.Server.Entity
{
    public static class Timers
    {
        /// <summary>
        /// DateTime.UtcNow at the time the server started.
        /// </summary>
        public static DateTime WorldStartTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// DateTime.UtcNow
        /// </summary>
        public static DateTime CurrentTime => DateTime.UtcNow;

        /// <summary>
        /// (CurrentTime - WorldStartTime).TotalSeconds<para />
        /// This value has 1ms precision.
        /// </summary>
        public static double RunningTime => (CurrentTime - WorldStartTime).TotalSeconds;


        /// <summary>
        /// DerethDateTime.UTCNowToLoreTime at the time the server started.
        /// </summary>
        public static DerethDateTime WorldStartLoreTime { get; } = DerethDateTime.UTCNowToLoreTime;

        /// <summary>
        /// Returns DerethDateTime.UTCNowToLoreTime
        /// </summary>
        public static DerethDateTime CurrentLoreTime => DerethDateTime.UTCNowToLoreTime;


        /// <summary>
        /// This is the current Portal Year time, in seconds.<para />
        /// It is updated once per WorldManager.UpdateWorld() loop.<para />
        /// This can also be used as the "frame time" value. This value will be unchanged for any calculations done inside of a single Tick of WorldManager.UpdateWorld().
        /// </summary>
        public static double PortalYearTicks { get; internal set; } = Timers.WorldStartLoreTime.Ticks;
    }
}
