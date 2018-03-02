using System.Collections.Generic;
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

        public override void find_walkable(SpherePath path, Sphere validPos, List<Polygon> polys, Vector3 movement, Vector3 up, bool changed)
        {

        }

        public override bool hits_walkable(SpherePath path, Sphere validPos, Vector3 up)
        {
            return false;
        }

        public override bool point_intersects_solid(Vector3 point)
        {
            return NumPolys != 0;
        }

        public override bool sphere_intersects_poly(Sphere checkPos, Vector3 movement, List<Polygon> polys, Vector3 contactPoint)
        {
            return false;
        }

        public override bool sphere_intersects_solid(Sphere checkPos, bool centerCheck)
        {
            return false;
        }

        public override bool sphere_intersects_solid_poly(Sphere checkPos, float radius, bool centerSolid, List<Polygon> hitPolys, bool centerCheck)
        {
            return false;
        }
    }
}
