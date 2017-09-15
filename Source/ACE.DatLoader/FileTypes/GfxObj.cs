using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x01. 
    /// These are used both on their own for some pre-populated structures in the world (trees, buildings, etc) or make up SetupModel (0x02) objects.
    /// </summary>
    public class GfxObj
    {
        public uint Id { get; set; }
        public uint Flags { get; set; }
        public List<uint> Surfaces { get; set; } = new List<uint>(); // also referred to as m_rgSurfaces in the client
        public CVertexArray VertexArray { get; set; }
        public Dictionary<ushort, Polygon> PhysicsPolygons { get; set; } = new Dictionary<ushort, Polygon>();
        public BSPTree PhysicsBSP { get; set; }
        public Position SortCenter { get; set; }
        public Dictionary<ushort, Polygon> Polygons { get; set; } = new Dictionary<ushort, Polygon>();
        public BSPTree DrawingBSP { get; set; }
        public uint DIDDegrade { get; set; }

        public static GfxObj ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (GfxObj)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                GfxObj obj = new GfxObj();

                obj.Id = datReader.ReadUInt32();
                obj.Flags = datReader.ReadUInt32();

                short num_surfaces = datReader.ReadPackedByte();
                for (short i = 0; i < num_surfaces; i++)
                    obj.Surfaces.Add(datReader.ReadUInt32());

                obj.VertexArray = CVertexArray.Read(datReader);

                // Has Physics 
                if ((obj.Flags & 1) > 0)
                {
                    short num_physics_polygons = datReader.ReadPackedByte();
                    for (ushort i = 0; i < num_physics_polygons; i++)
                    {
                        ushort poly_id = datReader.ReadUInt16();
                        obj.PhysicsPolygons.Add(poly_id, Polygon.Read(datReader));
                    }

                    obj.PhysicsBSP = BSPTree.Read(datReader, BSPType.Physics);
                }

                obj.SortCenter = PositionExtensions.ReadPositionFrame(datReader);

                // Has Drawing 
                if ((obj.Flags & 2) > 0)
                {
                    short num_polygons = datReader.ReadPackedByte();
                    for (ushort i = 0; i < num_polygons; i++)
                    {
                        ushort poly_id = datReader.ReadUInt16();
                        obj.Polygons.Add(poly_id, Polygon.Read(datReader));
                    }

                    obj.DrawingBSP = BSPTree.Read(datReader, BSPType.Drawing);
                }

                if ((obj.Flags & 8) > 0)
                    obj.DIDDegrade = datReader.ReadUInt32();

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
