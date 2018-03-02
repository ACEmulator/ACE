using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.BSP
{
    public class BSPLeaf: BSPNode
    {
        public int LeafIdx;
        public bool Solid;

        public BSPLeaf(): base()
        {
            LeafIdx = -1;
        }

        public BSPLeaf(BSPTreeType type): base(type)
        {
        }

        public override void find_walkable(SpherePath path, Sphere validPos, ref Polygon hitPoly, Vector3 movement, Vector3 up, ref bool changed)
        {
            if (NumPolys == 0 || !Sphere.Intersects(validPos))
                return;

            foreach (var polygon in Polygons)
            {
                if (polygon.walkable_hits_sphere(path, validPos, up) && polygon.adjust_sphere_to_plane(path, validPos, movement))
                {
                    changed = true;
                    hitPoly = polygon;
                }
            }
        }

        public override bool hits_walkable(SpherePath path, Sphere validPos, Vector3 up)
        {
            if (NumPolys == 0 || !Sphere.Intersects(validPos))
                return false;

            foreach (var polygon in Polygons)
            {
                if (polygon.walkable_hits_sphere(path, validPos, up) && polygon.check_small_walkable(validPos, up))
                    return true;
            }
            return false;
        }

        public override bool point_intersects_solid(Vector3 point)
        {
            return NumPolys != 0;
        }

        public override bool sphere_intersects_poly(Sphere checkPos, Vector3 movement, ref Polygon hitPoly, Vector3 contactPoint)
        {
            if (NumPolys == 0 || !Sphere.Intersects(checkPos))
                return false;

            foreach (var polygon in Polygons)
            {
                if (polygon.pos_hits_sphere(checkPos, movement, contactPoint, hitPoly))
                    return true;
            }
            return false;
        }

        public override bool sphere_intersects_solid(Sphere checkPos, bool checkCenter)
        {
            if (NumPolys == 0) return false;
            if (checkCenter && Solid) return true;
            if (Sphere.Intersects(checkPos)) return false;

            foreach (var polygon in Polygons)
                if (polygon.hits_sphere(checkPos))
                    return true;

            return false;
        }

        public override bool sphere_intersects_solid_poly(Sphere checkPos, float radius, ref bool centerSolid, ref Polygon hitPoly, bool checkCenter)
        {
            if (NumPolys == 0) return false;

            if (checkCenter && Solid)
                centerSolid = true;

            if (!Sphere.Intersects(checkPos))
                return centerSolid;

            foreach (var polygon in Polygons)
            {
                if (polygon.hits_sphere(checkPos))
                {
                    hitPoly = polygon;
                    return true;
                }
            }
            return centerSolid;
        }
    }
}
