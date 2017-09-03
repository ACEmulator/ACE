using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class GfxObjInfo
    {
        public uint Id { get; set; }
        public uint DegradeMode { get; set; }
        public float MinDist { get; set; }
        public float IdealDist { get; set; }
        public float MaxDist { get; set; }

        public static GfxObjInfo Read(DatReader datReader)
        {
            GfxObjInfo obj = new GfxObjInfo();

            obj.Id = datReader.ReadUInt32();
            obj.DegradeMode = datReader.ReadUInt32();
            obj.MinDist = datReader.ReadSingle();
            obj.IdealDist = datReader.ReadSingle();
            obj.MaxDist = datReader.ReadSingle();

            return obj;
        }
    }
}
