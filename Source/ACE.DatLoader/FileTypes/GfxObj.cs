using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x01. 
    /// These are used both on their own for some pre-populated structures in the world (trees, buildings, etc) or make up SetupModel (0x02) objects.
    /// </summary>
    [DatFileType(DatFileType.GraphicsObject)]
    public class GfxObj : IUnpackable
    {
        public uint Id { get; private set; }

        public List<uint> Surfaces { get; } = new List<uint>(); // also referred to as m_rgSurfaces in the client
        public CVertexArray VertexArray { get; } = new CVertexArray();

        public Dictionary<ushort, Polygon> PhysicsPolygons { get; } = new Dictionary<ushort, Polygon>();
        public BSPTree PhysicsBSP { get; } = new BSPTree();

        public Position SortCenter { get; } = new Position();

        public Dictionary<ushort, Polygon> Polygons { get; } = new Dictionary<ushort, Polygon>();
        public BSPTree DrawingBSP { get; } = new BSPTree();

        public uint DIDDegrade { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            var fields   = reader.ReadUInt32();

            Surfaces.UnpackSmartArray(reader);

            VertexArray.Unpack(reader);

            // Has Physics 
            if ((fields & 1) != 0)
            {
                PhysicsPolygons.UnpackSmartArray(reader);

                PhysicsBSP.Unpack(reader, BSPType.Physics);
            }

            SortCenter.ReadOrigin(reader);

            // Has Drawing 
            if ((fields & 2) != 0)
            {
                Polygons.UnpackSmartArray(reader);

                DrawingBSP.Unpack(reader, BSPType.Drawing);
            }

            if ((fields & 8) != 0)
                DIDDegrade = reader.ReadUInt32();
        }

        public static GfxObj ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (GfxObj)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new GfxObj();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
