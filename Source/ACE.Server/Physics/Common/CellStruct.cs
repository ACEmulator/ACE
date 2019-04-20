using System.Collections.Generic;
using System.Numerics;

using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Entity;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.Common
{
    public class CellStruct
    {
        public int CellStructID;
        public List<Vector3> VertexArray;
        public Dictionary<ushort, Polygon> Polygons;
        public Dictionary<ushort, Polygon> PhysicsPolygons;
        public List<Polygon> Portals;
        //public int NumSurfaceStrips;
        //public List<SurfaceTriStrips> SurfaceStrips;
        public BSPTree DrawingBSP;
        public BSPTree PhysicsBSP;
        public BSPTree CellBSP;

        public CellStruct() { }

        public CellStruct(DatLoader.Entity.CellStruct cellStruct)
        {
            Polygons = new Dictionary<ushort, Polygon>();
            foreach (var poly in cellStruct.Polygons)
                Polygons.Add(poly.Key, PolygonCache.Get(poly.Value, cellStruct.VertexArray));

            Portals = new List<Polygon>();
            foreach (var portal in cellStruct.Portals)
                Portals.Add(Polygons[portal]);

            PhysicsPolygons = new Dictionary<ushort, Polygon>();
            foreach (var poly in cellStruct.PhysicsPolygons)
                PhysicsPolygons.Add(poly.Key, PolygonCache.Get(poly.Value, cellStruct.VertexArray));

            if (cellStruct.CellBSP != null)
                CellBSP = BSPCache.Get(cellStruct.CellBSP, cellStruct.PhysicsPolygons, cellStruct.VertexArray);

            if (cellStruct.DrawingBSP != null)

                DrawingBSP = BSPCache.Get(cellStruct.DrawingBSP, cellStruct.Polygons, cellStruct.VertexArray);
            if (cellStruct.PhysicsBSP != null)
                PhysicsBSP = BSPCache.Get(cellStruct.PhysicsBSP, cellStruct.PhysicsPolygons, cellStruct.VertexArray);
        }

        public bool box_intersects_cell(BBox bbox)
        {
            return CellBSP.box_intersects_cell_bsp(bbox);
        }

        public Polygon get_portal(uint polyID)
        {
            Polygon portal = null;
            foreach (var p in Portals)
            {
                if (polyID == portal.PolyID) continue;
                portal = p;
            }
            return portal;
        }

        public BoundingType sphere_intersects_cell(Sphere sphere)
        {
            return CellBSP.sphere_intersects_cell_bsp(sphere);
        }

        public bool point_in_cell(Vector3 point)
        {
            return CellBSP.point_inside_cell_bsp(point);
        }
    }
}
