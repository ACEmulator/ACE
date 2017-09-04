using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TexMerge
    {
        public uint BaseTexSize { get; set; }
        public List<TerrainAlphaMap> CornerTerrainMaps { get; set; } = new List<TerrainAlphaMap>();
        public List<TerrainAlphaMap> SideTerrainMaps { get; set; } = new List<TerrainAlphaMap>();
        public List<RoadAlphaMap> RoadMaps { get; set; } = new List<RoadAlphaMap>();
        public List<TMTerrainDesc> TerrainDesc { get; set; } = new List<TMTerrainDesc>();

        public static TexMerge Read(DatReader datReader)
        {
            TexMerge obj = new TexMerge();

            obj.BaseTexSize = datReader.ReadUInt32();

            uint num_corner_terrain_maps = datReader.ReadUInt32();
            for (uint i = 0; i < num_corner_terrain_maps; i++)
                obj.CornerTerrainMaps.Add(TerrainAlphaMap.Read(datReader));

            uint num_side_terrain_maps = datReader.ReadUInt32();
            for (uint i = 0; i < num_side_terrain_maps; i++)
                obj.SideTerrainMaps.Add(TerrainAlphaMap.Read(datReader));

            uint num_road_maps = datReader.ReadUInt32();
            for (uint i = 0; i < num_road_maps; i++)
                obj.RoadMaps.Add(RoadAlphaMap.Read(datReader));

            uint num_terrain_desc = datReader.ReadUInt32();
            for (uint i = 0; i < num_terrain_desc; i++)
                obj.TerrainDesc.Add(TMTerrainDesc.Read(datReader));

            return obj;
        }
    }
}
