using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.BSP;

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
        public List<Polygon> Polygons;
        public BSPTree DrawingBSP;
        public int NumPhysicsPolygons;
        public List<Polygon> PhysicsPolygons;
        public BSPTree PhysicsBSP;
        public BSPTree CellBSP;

        public bool point_in_cell(Vector3 point)
        {
            return CellBSP.point_inside_cell_bsp(point);
        }
    }
}
