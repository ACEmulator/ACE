using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.BSP
{
    public class BSPNode
    {
        public Sphere Sphere;
        public Plane SplittingPlane;
        public BSPTreeType Type;
        public string Typename;
        public int NumPolys;
        public List<Polygon> Polys;
        public BSPNode PosNode;
        public BSPNode NegNode;

        public BSPNode()
        {
            Typename = "####";
        }

        public BSPNode(BSPTreeType type)
        {
            Type = type;
        }

        public void LinkPortals(List<BSPPortal> portals)
        {

        }

        public List<BSPPortal> PurgePortals()
        {
            return null;
        }

        public int TraceRay(Ray ray, float delta, Vector3 collisionNormal)
        {
            return -1;
        }

        public bool box_intersects_cell_bsp(BBox box)
        {
            return false;
        }

        public void build_draw_portals_only(int portalPolyOrPortalContents)
        {

        }

        public virtual void find_walkable(SpherePath path, Sphere validPos, List<Polygon> polygons, Vector3 movement, Vector3 up, bool changed)
        {

        }

        public virtual bool hits_walkable(SpherePath path, Sphere validPos, Vector3 up)
        {
            return false;
        }

        public bool point_inside_cell_bsp(Vector3 origin)
        {
            return false;
        }

        public virtual bool point_intersects_solid(Vector3 point)
        {
            return false;
        }

        public bool sphere_intersects_cell_bsp(Sphere sphere)
        {
            return false;
        }

        public virtual bool sphere_intersects_poly(Sphere checkPos, Vector3 movement, List<Polygon> polygons, Vector3 contactPoint)
        {
            return false;
        }

        public virtual bool sphere_intersects_solid(Sphere checkPos, bool centerCheck)
        {
            return false;
        }

        public virtual bool sphere_intersects_solid_poly(Sphere checkPos, float radius, bool centerSolid, List<Polygon> hitPoly, bool centerCheck)
        {
            return false;
        }
    }
}
