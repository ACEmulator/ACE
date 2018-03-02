using System.Numerics;

namespace ACE.Server.Physics.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Normalize(this Vector3 v)
        {
            return v / v.Length();
        }
    }
}
