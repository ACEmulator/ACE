using System;
using System.Collections.Generic;
using System.Numerics;

using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Entity;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.BSP
{
    public enum BoundingType
    {
        Outside = 0x0,
        PartiallyInside = 0x1,
        EntirelyInside = 0x2
    };

    public enum Side
    {
        Front = 0x0,
        Behind = 0x1,
        Close = 0x02
    };

    public class BSPNode: IEquatable<BSPNode>
    {
        public Sphere Sphere;
        public Plane SplittingPlane;
        public BSPTreeType Type;
        public string Typename;
        public int NumPolys;
        public List<ushort> PolyIDs;
        public List<Polygon> Polygons;
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

        public BSPNode(DatLoader.Entity.BSPNode node, Dictionary<ushort, DatLoader.Entity.Polygon> polys, DatLoader.Entity.CVertexArray vertexArray)
        {
            if (node.Sphere != null)
                Sphere = new Sphere(node.Sphere);
            if (node.SplittingPlane != null)
                SplittingPlane = node.SplittingPlane.ToNumerics();
            Typename = node.Type;
            //Typename
            if (node.InPolys != null)
            {
                NumPolys = node.InPolys.Count;
                PolyIDs = node.InPolys;
                Polygons = new List<Polygon>(node.InPolys.Count);
                foreach (var poly in node.InPolys)
                    Polygons.Add(PolygonCache.Get(polys[poly], vertexArray));
            }
            if (node.PosNode != null)
            {
                if (!(node.PosNode is DatLoader.Entity.BSPLeaf))
                    PosNode = new BSPNode(node.PosNode, polys, vertexArray);
                else // portal?
                    PosNode = new BSPLeaf((DatLoader.Entity.BSPLeaf)node.PosNode, polys, vertexArray);
            }
            if (node.NegNode != null)
            {
                if (!(node.NegNode is DatLoader.Entity.BSPLeaf))
                    NegNode = new BSPNode(node.NegNode, polys, vertexArray);
                else // portal?
                    NegNode = new BSPLeaf((DatLoader.Entity.BSPLeaf)node.NegNode, polys, vertexArray);
            }
        }

        public void LinkPortals(List<BSPPortal> portals)
        {
            var current = this;

            for (var i = portals.Count - 1; i >= 0; --i)
            {
                var nextPortal = portals[i];
                current.PosNode = nextPortal;
                if (i > 0)
                    nextPortal.PosNode = portals[i - 1];

                current = nextPortal;
            }
        }

        public List<BSPPortal> PurgePortals()
        {
            var portals = new List<BSPPortal>();

            if (PosNode != null)
            {
                portals.AddRange(PosNode.PurgePortals());
                PosNode = null;
            }

            if (NegNode != null)
            {
                portals.AddRange(NegNode.PurgePortals());
                NegNode = null;
            }
            return portals;
        }

        public int TraceRay(Ray ray, float delta, Vector3 collisionNormal)
        {
            return -1;  // unused?
        }

        public bool box_intersects_cell_bsp(BBox box)
        {
            var corners = box.GetCorners();
            for (var node = this; node != null; node = node.PosNode)
            {
                var dist = Vector3.Dot(box.Min, node.SplittingPlane.Normal) + node.SplittingPlane.D;
                if (dist >= -PhysicsGlobals.EPSILON) continue;

                if (box_intersects_cell_bsp_inner(node, corners))
                    return false;
            }
            return true;
        }

        public bool box_intersects_cell_bsp_inner(BSPNode node, List<Vector3> corners)
        {
            foreach (var corner in corners)
                if (node.SplittingPlane.GetSide(corner) != Side.Behind)
                    return false;

            return true;
        }

        public void build_draw_portals_only(int portalPolyOrPortalContents)
        {
            // for rendering
        }

        public virtual void find_walkable(SpherePath path, Sphere validPos, ref Polygon polygon, Vector3 movement, Vector3 up, ref bool changed)
        {
            if (!Sphere.Intersects(validPos)) return;

            var dist = Vector3.Dot(SplittingPlane.Normal, validPos.Center) + SplittingPlane.D;
            var reach = validPos.Radius - PhysicsGlobals.EPSILON;

            if (dist >= reach)
            {
                PosNode.find_walkable(path, validPos, ref polygon, movement, up, ref changed);
                return;
            }
            if (dist <= -reach)
            {
                NegNode.find_walkable(path, validPos, ref polygon, movement, up, ref changed);
                return;
            }
            PosNode.find_walkable(path, validPos, ref polygon, movement, up, ref changed);

            NegNode.find_walkable(path, validPos, ref polygon, movement, up, ref changed);
        }

        public virtual bool hits_walkable(SpherePath path, Sphere validPos, Vector3 up)
        {
            if (!Sphere.Intersects(validPos))
                return false;

            var dist = Vector3.Dot(SplittingPlane.Normal, validPos.Center) + SplittingPlane.D;
            var reach = validPos.Radius - PhysicsGlobals.EPSILON;

            if (dist >= reach)
                return PosNode.hits_walkable(path, validPos, up);

            if (dist <= -reach)
                return NegNode.hits_walkable(path, validPos, up);

            if (PosNode.hits_walkable(path, validPos, up))
                return true;

            if (NegNode.hits_walkable(path, validPos, up))
                return true;

            return false;
        }

        public bool point_inside_cell_bsp(Vector3 point)
        {
            var side = SplittingPlane.GetSide(point);

            switch (side)
            {
                case Side.Front:
                case Side.Close:

                    if (PosNode != null)
                        return PosNode.point_inside_cell_bsp(point);
                    else
                        return true;

                case Side.Behind:
                default:
                    return false;
            }
        }

        public virtual bool point_intersects_solid(Vector3 point)
        {
            if (Vector3.Dot(point, SplittingPlane.Normal) + SplittingPlane.D > 0.0f)
                return PosNode.point_intersects_solid(point);
            else
                return NegNode.point_intersects_solid(point);
        }

        public BoundingType sphere_intersects_cell_bsp(Sphere curSphere)
        {
            var dist = Vector3.Dot(SplittingPlane.Normal, curSphere.Center) + SplittingPlane.D;
            var checkRad = curSphere.Radius + 0.01f;   // 0.0099999998;

            if (dist <= -checkRad)
                return BoundingType.Outside;

            if (dist >= checkRad)
            {
                if (PosNode != null)
                    return PosNode.sphere_intersects_cell_bsp(curSphere);
                else
                    return BoundingType.EntirelyInside;
            }

            if (PosNode != null)
                return PosNode.sphere_intersects_cell_bsp(curSphere) != BoundingType.Outside ?
                    BoundingType.PartiallyInside : BoundingType.Outside;

            return BoundingType.PartiallyInside;
        }

        public virtual bool sphere_intersects_poly(Sphere checkPos, Vector3 movement, ref Polygon polygon, ref Vector3 contactPoint)
        {
            if (!Sphere.Intersects(checkPos))
                return false;

            var dist = Vector3.Dot(SplittingPlane.Normal, checkPos.Center) + SplittingPlane.D;
            var reach = checkPos.Radius - PhysicsGlobals.EPSILON;

            if (dist >= reach)
                return PosNode.sphere_intersects_poly(checkPos, movement, ref polygon, ref contactPoint);

            if (dist <= -reach)
                return NegNode.sphere_intersects_poly(checkPos, movement, ref polygon, ref contactPoint);

            if (PosNode != null && PosNode.sphere_intersects_poly(checkPos, movement, ref polygon, ref contactPoint))
                return true;

            if (NegNode != null && NegNode.sphere_intersects_poly(checkPos, movement, ref polygon, ref contactPoint))
                return true;

            return false;
        }

        public virtual bool sphere_intersects_solid(Sphere checkPos, bool centerCheck)
        {
            if (!Sphere.Intersects(checkPos))
                return false;

            var dist = Vector3.Dot(SplittingPlane.Normal, checkPos.Center) + SplittingPlane.D;
            var reach = checkPos.Radius - PhysicsGlobals.EPSILON;

            if (dist >= reach)
                return PosNode.sphere_intersects_solid(checkPos, centerCheck);

            if (dist <= -reach)
                return NegNode.sphere_intersects_solid(checkPos, centerCheck);

            if (dist < 0.0f)
            {
                if (PosNode.sphere_intersects_solid(checkPos, false))
                    return true;

                return NegNode.sphere_intersects_solid(checkPos, centerCheck);
            }
            else
            {
                if (PosNode.sphere_intersects_solid(checkPos, centerCheck))
                    return true;

                return NegNode.sphere_intersects_solid(checkPos, false);
            }
        }

        public virtual bool sphere_intersects_solid_poly(Sphere checkPos, float radius, ref bool centerSolid, ref Polygon hitPoly, bool centerCheck)
        {
            if (!Sphere.Intersects(checkPos))
                return false;

            var dist = Vector3.Dot(SplittingPlane.Normal, checkPos.Center) + SplittingPlane.D;
            var reach = radius - PhysicsGlobals.EPSILON;

            if (dist >= reach)
                return PosNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, centerCheck);

            if (dist <= -reach)
                return NegNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, centerCheck);

            if (dist <= 0.0f)
            {
                NegNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, centerCheck);

                if (hitPoly != null) return centerSolid;

                return PosNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, false);
            }
            else
            {
                PosNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, centerCheck);

                if (hitPoly != null) return centerSolid;

                return NegNode.sphere_intersects_solid_poly(checkPos, radius, ref centerSolid, ref hitPoly, false);
            }
        }

        public bool Equals(BSPNode node)
        {
            if (Sphere != null && !Sphere.Equals(node.Sphere) || !SplittingPlane.is_equal(node.SplittingPlane) || Type != node.Type || Typename != null && !Typename.Equals(node.Typename) || NumPolys != node.NumPolys)
                return false;

            for (var i = 0; i < NumPolys; i++)
            {
                if (PolyIDs[i] != node.PolyIDs[i] || !Polygons[i].Equals(node.Polygons[i]))
                    return false;
            }

            if (PosNode != null && !PosNode.Equals(node.PosNode))
                return false;

            if (NegNode != null && !NegNode.Equals(node.NegNode))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            if (Sphere != null)
                hash = (hash * 397) ^ Sphere.GetHashCode();

            hash = (hash * 397) ^ SplittingPlane.get_hash_code();
            hash = (hash * 397) ^ Type.GetHashCode();

            if (Typename != null)
                hash = (hash * 397) ^ Typename.GetHashCode();

            hash = (hash * 397) ^ NumPolys.GetHashCode();

            for (var i = 0; i < NumPolys; i++)
            {
                hash = (hash * 397) ^ PolyIDs[i].GetHashCode();
                hash = (hash * 397) ^ Polygons[i].GetHashCode();
            }
            if (PosNode != null)
                hash = (hash * 397) ^ PosNode.GetHashCode();

            if (NegNode != null)
                hash = (hash * 397) ^ NegNode.GetHashCode();

            return hash;
        }
    }
}
