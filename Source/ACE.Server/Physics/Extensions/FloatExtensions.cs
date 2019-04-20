using System;

namespace ACE.Server.Physics.Extensions
{
    public static class FloatExtensions
    {
        public static float ToRadians(this float angle)
        {
            return (float)(Math.PI / 180.0f * angle);
        }

        public static float ToDegrees(this float rads)
        {
            return (float)(180.0f / Math.PI * rads);
        }

        public static float Clamp(this float f, float min, float max)
        {
            if (f < min)
                f = min;
            if (f > max)
                f = max;
            return f;
        }
    }
}
