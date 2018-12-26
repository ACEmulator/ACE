using System;

namespace ACE.Common.Extensions
{
    public static class FloatExtensions
    {
        public static int Round(this float num, int decimalPlaces = 0)
        {
            return (int)Math.Round(num, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        public static float Truncate(this float num, int decimalPlaces)
        {
            var multiplier = Math.Pow(10, decimalPlaces);

            return (float)(Math.Truncate(num * multiplier) / multiplier);
        }
    }
}
