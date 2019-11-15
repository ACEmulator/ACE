using System;
using System.Numerics;

namespace ACE.Server.Physics.Extensions
{
    public static class QuaternionExtensions
    {
        public static bool IsValid(this Quaternion q)
        {
            if (float.IsNaN(q.X) || float.IsNaN(q.Y) || float.IsNaN(q.Z) || float.IsNaN(q.W))
                return false;

            var length = q.Length();
            if (float.IsNaN(length))
                return false;

            if (Math.Abs(length - 1.0f) > PhysicsGlobals.EPSILON * 5.0f)
                return false;

            return true;
        }
    }
}
