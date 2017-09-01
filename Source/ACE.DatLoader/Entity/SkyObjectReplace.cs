using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SkyObjectReplace
    {
        public uint ObjectIndex { get; set; }
        public uint GFXObjId { get; set; }
        public float Rotate { get; set; }
        public float Transparent { get; set; }
        public float Luminosity { get; set; }
        public float MaxBright { get; set; }
    
        public static SkyObjectReplace Read(DatReader datReader)
        {
            SkyObjectReplace obj = new SkyObjectReplace();
            obj.ObjectIndex = datReader.ReadUInt32();
            obj.GFXObjId = datReader.ReadUInt32();
            obj.Rotate = datReader.ReadSingle();
            obj.Transparent = datReader.ReadSingle();
            obj.Luminosity = datReader.ReadSingle();
            obj.MaxBright = datReader.ReadSingle();
            return obj;
        }
    }
}
