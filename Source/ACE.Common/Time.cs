using System;

namespace ACE.Common
{
    public class Time
    {
        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static double GetUnixTime()
        {
            return GetUnixTime(DateTime.UtcNow);
        }

        public static double GetUnixTime(DateTime dateTime)
        {
            TimeSpan span = (dateTime - unixEpoch);

            return span.TotalSeconds;
        }

        public static double GetFutureUnixTime(double seconds)
        {
            TimeSpan span = (DateTime.UtcNow.AddSeconds(seconds) - unixEpoch);

            return span.TotalSeconds;
        }

        public static DateTime GetDateTimeFromTimestamp(double timestamp)
        {
            return unixEpoch.AddSeconds(timestamp);
        }
    }
}
