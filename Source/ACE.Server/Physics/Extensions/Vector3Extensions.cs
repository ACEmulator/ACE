using System.Numerics;

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
            return v.X != float.NaN && v.Y != float.NaN && v.Z != float.NaN;
        }

        public static double Length2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y).Length();
        }

        public static double LengthSquared2D(this Vector3 v)
        {
            return new Vector2(v.X, v.Y).LengthSquared();
        }

        public static Vector3 Normalize(this Vector3 v)
        {
            return v / v.Length();
        }

        public static bool NormalizeCheckSmall(this Vector3 v)
        {
            var dist = v.Length();
            if (dist >= PhysicsGlobals.EPSILON)
            {
                v *= 1.0f / dist;
                return true;
            }
            return false;
        }

        public static bool IsMoved(this Vector3 a, Vector3 b)
        {
            return (a.X != b.X || a.Y != b.Y || a.Z != b.Z);
        }
    }
}
