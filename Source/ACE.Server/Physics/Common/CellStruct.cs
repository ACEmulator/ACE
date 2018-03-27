using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.Common
{
    public class CellStruct
    {
        public int CellStructID;
        public List<Vector3> VertexArray;
        public int NumPortals;
        public List<Polygon> Portals;
        public int NumSurfaceStrips;
        //public List<SurfaceTriStrips> SurfaceStrips;
        public int NumPolygons;
        public Dictionary<ushort, Polygon> Polygons;
        public BSPTree DrawingBSP;
        public int NumPhysicsPolygons;
        public Dictionary<ushort, Polygon> PhysicsPolygons;
        public BSPTree PhysicsBSP;
        public BSPTree CellBSP;

        public CellStruct() { }

        public CellStruct(DatLoader.Entity.CellStruct cellStruct)
        {
            NumPortals = cellStruct.Portals.Count;
            NumPolygons = cellStruct.Polygons.Count;
            NumPhysicsPolygons = cellStruct.PhysicsPolygons.Count;
            Polygons = new Dictionary<ushort, Polygon>();
            foreach (var poly in cellStruct.Polygons)
                Polygons.Add(poly.Key, new Polygon(poly.Value, cellStruct.VertexArray));
            Portals = new List<Polygon>();
            foreach (var portal in cellStruct.Portals)
                Portals.Add(Polygons[portal]);
            PhysicsPolygons = new Dictionary<ushort, Polygon>();
            foreach (var poly in cellStruct.PhysicsPolygons)
                PhysicsPolygons.Add(poly.Key, new Polygon(poly.Value, cellStruct.VertexArray));
            if (cellStruct.CellBSP != null)
                CellBSP = new BSPTree(cellStruct.CellBSP, cellStruct.PhysicsPolygons, cellStruct.VertexArray); // physics or drawing?
            if (cellStruct.DrawingBSP != null)
                DrawingBSP = new BSPTree(cellStruct.DrawingBSP, cellStruct.Polygons, cellStruct.VertexArray);
            if (cellStruct.PhysicsBSP != null)
                PhysicsBSP = new BSPTree(cellStruct.PhysicsBSP, cellStruct.PhysicsPolygons, cellStruct.VertexArray);
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
