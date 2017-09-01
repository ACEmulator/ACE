using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class RoadAlphaMap
    {
        public uint RCode { get; set; }
        public uint RoadTexGID { get; set; }

        public static RoadAlphaMap Read(DatReader datReader)
        {
            RoadAlphaMap obj = new RoadAlphaMap();
            obj.RCode = datReader.ReadUInt32();
            obj.RoadTexGID = datReader.ReadUInt32();
            return obj;
        }
    }
}
