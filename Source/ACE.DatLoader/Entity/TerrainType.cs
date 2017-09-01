using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TerrainType
    {
        public string TerrainName { get; set; }
        public uint TerrainColor { get; set; }
        public List<uint> SceneTypes { get; set; } = new List<uint>();

        public static TerrainType Read(DatReader datReader)
        {
            TerrainType obj = new TerrainType();

            obj.TerrainName = datReader.ReadPString();
            datReader.AlignBoundary();

            obj.TerrainColor = datReader.ReadUInt32();

            uint num_stypes = datReader.ReadUInt32();
            for (uint i = 0; i < num_stypes; i++)
                obj.SceneTypes.Add(datReader.ReadUInt32());

            return obj;
        }
    }
}
