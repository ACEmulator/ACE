using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SkyTimeOfDay
    {
        public float Begin { get; set; }
        public float DirBright { get; set; }
        public float DirHeading { get; set; }
        public float DirPitch { get; set; }
        public uint DirColor { get; set; }

        public float AmbBright { get; set; }
        public uint AmbColor { get; set; }

        public float MinWorldFog { get; set; }
        public float MaxWorldFog { get; set; }
        public uint WorldFogColor { get; set; }
        public uint WorldFog { get; set; }

        public List<SkyObjectReplace> SkyObjReplace { get; set; } = new List<SkyObjectReplace>();

        public static SkyTimeOfDay Read(DatReader datReader)
        {
            SkyTimeOfDay obj = new SkyTimeOfDay();
            obj.Begin = datReader.ReadSingle();
            obj.DirBright = datReader.ReadSingle();
            obj.DirHeading = datReader.ReadSingle();
            obj.DirPitch = datReader.ReadSingle();
            obj.DirColor = datReader.ReadUInt32();

            obj.AmbBright = datReader.ReadSingle();
            obj.AmbColor = datReader.ReadUInt32();

            obj.MinWorldFog = datReader.ReadSingle();
            obj.MaxWorldFog = datReader.ReadSingle();
            obj.WorldFogColor = datReader.ReadUInt32();
            obj.WorldFog = datReader.ReadUInt32();

            uint num_sky_obj_replace = datReader.ReadUInt32();
            for (uint i = 0; i < num_sky_obj_replace; i++)
                obj.SkyObjReplace.Add(SkyObjectReplace.Read(datReader));

            return obj;
        }
    }
}
