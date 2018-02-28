using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public static class Extensions
    {
        public static Vector3 Normalize(this Vector3 v)
        {
            return v / v.Length();
        }

        public static Vector3 SnapToPlane(this Plane p, Vector3 v)
        {
            return v;
        }
    }
}
