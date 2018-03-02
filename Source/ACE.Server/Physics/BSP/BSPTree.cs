using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.BSP
{
    public enum BSPTreeType
    {
        Drawing = 0x0,
        Physics = 0x1,
        Cell = 0x2
    };

    public class BSPTree
    {
        public BSPNode RootNode;

        public BSPTree()
        {

        }

        public BSPTree(BSPNode rootNode)
        {
            RootNode = rootNode;
        }

        public Sphere GetSphere()
        {
            return RootNode.Sphere;
        }

        public void RemoveNonPortalNodes()
        {
            RootNode.LinkPortals(RootNode.PurgePortals());
        }

        public bool adjust_to_plane(Sphere checkPos, Vector3 curPos, Polygon hitPoly, Vector3 contactPoint)
        {
            return false;
        }

        public bool box_intersects_cell_bsp(BBox box)
        {
            return RootNode.box_intersects_cell_bsp(box);
        }

        public void build_draw_portals_only(int portalPolyOrPortalContents)
        {

        }

        public bool check_walkable(SpherePath path, Sphere checkPos, float scale)
        {
            return false;
        }

        public TransitionState collide_with_pt(Transition transition, Sphere checkPos, Vector3 curPos, Polygon hitPoly, Vector3 contactPoint, float scale)
        {
            return TransitionState.Collided;
        }

        public bool find_collisions(Transition transition, float scale)
        {
            return false;
        }

        public bool placement_insert(Transition transition)
        {
            return false;
        }

        public bool point_inside_cell_bsp(Vector3 origin)
        {
            return RootNode.point_inside_cell_bsp(origin);
        }

        public bool slide_sphere(Transition transition, Vector3 collisionNormal)
        {
            return false;
        }

        public bool sphere_intersects_cell_bsp(Sphere sphere)
        {
            return RootNode.sphere_intersects_cell_bsp(sphere);
        }

        public bool step_sphere_down(Transition transition, Sphere checkPos, float scale)
        {
            return false;
        }

        public bool step_sphere_up(Transition transition, Vector3 collisionNormal)
        {
            return false;
        }
    }
}
