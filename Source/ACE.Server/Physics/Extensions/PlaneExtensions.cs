using System.Numerics;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Extensions
{
    public static class PlaneExtensions
    {
        public static Plane LocalToGlobal(this Plane plane, Position to, Position from)
        {
            return plane;
        }

        public static Side GetSide(this Plane plane, Vector3 p)
        {
            var dist = Vector3.Dot(plane.Normal, p) + plane.D;

            Side side = Side.Front;
            if (dist <= PhysicsGlobals.EPSILON)
                side = dist < -PhysicsGlobals.EPSILON ? Side.Behind : Side.Close;

            return side;
        }

        public static Vector3 SnapToPlane(this Plane p, Vector3 v)
        {
            return v;
        }

        public static bool compute_time_of_intersection(this Plane p, Ray ray, ref float time)
        {
            return false;
        }
    }
}
