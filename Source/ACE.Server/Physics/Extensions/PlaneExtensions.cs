using System;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.Extensions
{
    public static class PlaneExtensions
    {
        public static Plane LocalToGlobal(this Plane plane, Position to, Position from, Plane localPlane)
        {
            var normal = from.Frame.LocalToGlobalVec(localPlane.Normal);
            var dist = to.LocalToGlobal(from, localPlane.Normal * -localPlane.D);

            return new Plane(normal, -Vector3.Dot(normal, dist));
        }

        public static Side GetSide(this Plane plane, Vector3 p, float bias = 0.0f)
        {
            var dist = Vector3.Dot(plane.Normal, p) + plane.D + bias;

            Side side = Side.Front;
            if (dist <= PhysicsGlobals.EPSILON)
                side = dist < -PhysicsGlobals.EPSILON ? Side.Behind : Side.Close;

            return side;
        }

        public static void SnapToPlane(this Plane p, ref Vector3 offset)
        {
            if (Math.Abs(p.Normal.Z) <= PhysicsGlobals.EPSILON)
                return;

            offset.Z = -(offset.Dot2D(p.Normal) + p.D) * (1.0f / p.Normal.Z) - 1.0f / p.Normal.Z * -p.D;
        }

        public static bool compute_time_of_intersection(this Plane p, Ray ray, ref float time)
        {
            var angle = Vector3.Dot(ray.Dir, p.Normal);
            if (Math.Abs(angle) < PhysicsGlobals.EPSILON)
                return false;

            time = (Vector3.Dot(ray.Point, p.Normal) + p.D) * (-1.0f / angle);
            return time >= 0.0f;
        }

        public static Sidedness intersect_box(this Plane p, BBox box)
        {
            Sidedness result;

            var dist = Vector3.Dot(box.Min, p.Normal) + p.D;
            if (dist <= PhysicsGlobals.EPSILON)
            {
                if (dist >= -PhysicsGlobals.EPSILON)
                    return Sidedness.Crossing;
                else
                    result = Sidedness.Negative;
            }
            else
                result = Sidedness.Positive;

            var corners = box.GetCorners();

            foreach (var corner in corners)
            {
                if (result != (Sidedness)GetSide(p, corner, PhysicsGlobals.EPSILON))
                    return Sidedness.Crossing;
            }
            return result;
        }

        public static bool set_height(this Plane p, ref Vector3 v)
        {
            if (Math.Abs(p.Normal.Z) <= PhysicsGlobals.EPSILON)
                return false;

            v.Z = -((v.Y * p.Normal.Y + v.X * p.Normal.X + p.D) / p.Normal.Z);
            return true;
        }

        public static bool is_equal(this Plane p1, Plane p2)
        {
            return p1.Normal.X == p2.Normal.X && p1.Normal.Y == p2.Normal.Y && p1.Normal.Z == p2.Normal.Z && p1.D == p2.D;
        }

        public static int get_hash_code(this Plane p)
        {
            int hash = 0;

            hash = (hash * 397) ^ p.Normal.GetHashCode();
            hash = (hash * 397) ^ p.D.GetHashCode();

            return hash;
        }
    }
}
