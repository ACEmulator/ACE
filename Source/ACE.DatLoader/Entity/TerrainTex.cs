using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TerrainTex
    {
        public uint TexGID { get; set; }
        public uint TexTiling { get; set; }
        public uint MaxVertBright { get; set; }
        public uint MinVertBright { get; set; }
        public uint MaxVertSaturate { get; set; }
        public uint MinVertSaturate { get; set; }
        public uint MaxVertHue { get; set; }
        public uint MinVertHue { get; set; }
        public uint DetailTexTiling { get; set; }
        public uint DetailTexGID { get; set; }

        public static TerrainTex Read(DatReader datReader)
        {
            TerrainTex obj = new TerrainTex();

            obj.TexGID = datReader.ReadUInt32();
            obj.TexTiling = datReader.ReadUInt32();
            obj.MaxVertBright = datReader.ReadUInt32();
            obj.MinVertBright = datReader.ReadUInt32();
            obj.MaxVertSaturate = datReader.ReadUInt32();
            obj.MinVertSaturate = datReader.ReadUInt32();
            obj.MaxVertHue = datReader.ReadUInt32();
            obj.MinVertHue = datReader.ReadUInt32();
            obj.DetailTexTiling = datReader.ReadUInt32();
            obj.DetailTexGID = datReader.ReadUInt32();

            return obj;
        }
    }
}
