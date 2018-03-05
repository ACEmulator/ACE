using System.Numerics;

namespace ACE.Server.Physics.Extensions
{
    public static class Vector3Extensions
    {
        public static bool IsValid(this Vector3 v)
        {
            return v.X != float.NaN && v.Y != float.NaN && v.Z != float.NaN;
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
    }
}
