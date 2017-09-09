using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TerrainDesc
    {
        public List<TerrainType> TerrainTypes { get; set; } = new List<TerrainType>();
        public LandSurf LandSurfaces { get; set; }

        public static TerrainDesc Read(DatReader datReader)
        {
            TerrainDesc obj = new TerrainDesc();

            uint num_terrain_types = datReader.ReadUInt32();
            for (uint i = 0; i < num_terrain_types; i++)
                obj.TerrainTypes.Add(TerrainType.Read(datReader));

            obj.LandSurfaces = LandSurf.Read(datReader);

            return obj;
        }
    }
}
