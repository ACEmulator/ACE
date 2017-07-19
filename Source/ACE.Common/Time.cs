using System;

namespace ACE.Common
{
    public class Time
    {
        public static ulong GetUnixTime() { return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }

        public static double GetTimestamp()
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            double timestamp = span.TotalSeconds;

            return timestamp;
        }
    }
}
