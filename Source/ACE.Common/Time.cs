using System;

namespace ACE.Common
{
    public class Time
    {
        public static ulong GetUnixTime() { return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }
    }
}
