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
    }
}
