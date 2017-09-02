using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TerrainAlphaMap
    {
        public uint TCode { get; set; }
        public uint TexGID { get; set; }

        public static TerrainAlphaMap Read(DatReader datReader)
        {
            TerrainAlphaMap obj = new TerrainAlphaMap();
            obj.TCode = datReader.ReadUInt32();
            obj.TexGID = datReader.ReadUInt32();
            return obj;
        }
    }
}
