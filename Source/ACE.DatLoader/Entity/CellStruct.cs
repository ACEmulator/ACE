using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class CellStruct
    {
        public CVertexArray VertexArray { get; set; }
        public Dictionary<ushort, Polygon> Polygons { get; set; } = new Dictionary<ushort, Polygon>();
        public List<ushort> Portals { get; set; } = new List<ushort>();
        public BSPTree CellBSP { get; set; }
        public Dictionary<ushort, Polygon> PhysicsPolygons { get; set; } = new Dictionary<ushort, Polygon>();
        public BSPTree PhysicsBSP { get; set; }
        public BSPTree DrawingBSP { get; set; }

        public static CellStruct Read(DatReader datReader) { 
            CellStruct obj = new CellStruct();

            uint numPolygons = datReader.ReadUInt32();
            uint numPhysicsPolygons = datReader.ReadUInt32();
            uint numPortals = datReader.ReadUInt32();

            obj.VertexArray = CVertexArray.Read(datReader);

            for (uint i = 0; i < numPolygons; i++)
            {
                ushort poly_id = datReader.ReadUInt16();
                obj.Polygons.Add(poly_id, Polygon.Read(datReader));
            }

            for (uint i = 0; i < numPortals; i++)
                obj.Portals.Add(datReader.ReadUInt16());

            datReader.AlignBoundary();

            obj.CellBSP = BSPTree.Read(datReader, BSPType.Cell);

            for (uint i = 0; i < numPhysicsPolygons; i++)
            {
                ushort poly_id = datReader.ReadUInt16();
                obj.PhysicsPolygons.Add(poly_id, Polygon.Read(datReader));
            }
            obj.PhysicsBSP = BSPTree.Read(datReader, BSPType.Physics);

            uint hasDrawingBSP = datReader.ReadUInt32();
            if(hasDrawingBSP != 0)
                obj.DrawingBSP = BSPTree.Read(datReader, BSPType.Drawing);

            datReader.AlignBoundary();

            return obj;
        }
    }
}
