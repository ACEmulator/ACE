using System;

namespace ACE
{
    public class Time
    {
        public static ulong GetUnixTime() { return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }
    }
}
