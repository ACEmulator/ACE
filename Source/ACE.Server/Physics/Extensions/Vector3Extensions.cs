using System;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Extensions
{
    public static class Vector3Extensions
    {
        public static float Dot2D(this Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static bool IsValid(this Vector3 v)
        {
            return !float.IsNaN(v.X) && !float.IsNaN(v.Y) && !float.IsNaN(v.Z);
        }

        public static double Length2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y).Length();
        }

        public static double LengthSquared2D(this Vector3 v)
        {
            return v.X * v.X + v.Y * v.Y;
        }

        public static float get_heading(this Vector3 v)
        {
            var normal = new Vector3(v.X, v.Y, 0);
            if (Vec.NormalizeCheckSmall(ref normal))
                return 0.0f;

            var heading = (450.0f - ((float)Math.Atan2(normal.Y, normal.X)).ToDegrees()) % 360.0f;
            return heading;
        }
    }
}
