using System;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public static class Vec
    {
        public static bool NormalizeCheckSmall(ref Vector3 v)
        {
            var dist = v.Length();
            if (dist < PhysicsGlobals.EPSILON)
                return true;

            v *= 1.0f / dist;
            return false;
        }

        public static bool IsZero(Vector3 v)
        {
            return Math.Abs(v.X) < PhysicsGlobals.EPSILON &&
                   Math.Abs(v.Y) < PhysicsGlobals.EPSILON &&
                   Math.Abs(v.Z) < PhysicsGlobals.EPSILON;
        }
    }
}
