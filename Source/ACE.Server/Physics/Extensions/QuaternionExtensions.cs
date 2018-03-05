using System;
using System.Numerics;

namespace ACE.Server.Physics.Extensions
{
    public static class QuaternionExtensions
    {
        public static bool IsValid(this Quaternion q)
        {
            if (q.X == float.NaN || q.Y == float.NaN || q.Z == float.NaN || q.W == float.NaN)
                return false;

            var length = q.Length();
            if (length == float.NaN)
                return false;

            if (Math.Abs(length - 1.0f) > PhysicsGlobals.EPSILON * 5.0f)
                return false;

            return true;
        }
    }
}
