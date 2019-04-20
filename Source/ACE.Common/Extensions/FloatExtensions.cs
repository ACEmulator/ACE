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

        public static bool EpsilonEquals(this float num, float val/*, int decimalPlaces = 4*/)
        {
            //var epsilon = 1.0f / Math.Pow(10, decimalPlaces);
            var epsilon = 0.0001f;

            return Math.Abs(num - val) < epsilon;
        }
    }
}
