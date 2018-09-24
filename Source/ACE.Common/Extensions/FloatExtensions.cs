using System;

namespace ACE.Common.Extensions
{
    public static class FloatExtensions
    {
        public static int Round(this float num, int decimalPlaces = 0)
        {
            return (int)Math.Round(num, decimalPlaces, MidpointRounding.AwayFromZero);
        }
    }
}
