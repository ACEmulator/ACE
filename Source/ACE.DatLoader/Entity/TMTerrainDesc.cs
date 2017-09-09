using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TMTerrainDesc
    {
        public uint TerrainType { get; set; }
        public TerrainTex TerrainTex { get; set; }

        public static TMTerrainDesc Read(DatReader datReader)
        {
            TMTerrainDesc obj = new TMTerrainDesc();

            obj.TerrainType = datReader.ReadUInt32();
            obj.TerrainTex = TerrainTex.Read(datReader);

            return obj;
        }
    }
}
