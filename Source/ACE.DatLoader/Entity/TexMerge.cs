using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class TexMerge : IUnpackable
    {
        public uint BaseTexSize { get; private set; }
        public List<TerrainAlphaMap> CornerTerrainMaps { get; } = new List<TerrainAlphaMap>();
        public List<TerrainAlphaMap> SideTerrainMaps { get; } = new List<TerrainAlphaMap>();
        public List<RoadAlphaMap> RoadMaps { get; } = new List<RoadAlphaMap>();
        public List<TMTerrainDesc> TerrainDesc { get; } = new List<TMTerrainDesc>();

        public void Unpack(BinaryReader reader)
        {
            BaseTexSize = reader.ReadUInt32();

            CornerTerrainMaps.Unpack(reader);
            SideTerrainMaps.Unpack(reader);
            RoadMaps.Unpack(reader);
            TerrainDesc.Unpack(reader);
        }
    }
}
