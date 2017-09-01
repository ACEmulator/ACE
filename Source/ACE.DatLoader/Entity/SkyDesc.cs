using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SkyDesc
    {
        public UInt64 TickSize { get; set; }
        public UInt64 LightTickSize { get; set; }
        public List<DayGroup> DayGroups { get; set; } = new List<DayGroup>();

        public static SkyDesc Read(DatReader datReader)
        {
            SkyDesc obj = new SkyDesc();
            obj.TickSize = datReader.ReadUInt64();
            obj.LightTickSize = datReader.ReadUInt64();

            uint numDayGroups = datReader.ReadUInt32();
            for (uint i = 0; i < numDayGroups; i++)
                obj.DayGroups.Add(DayGroup.Read(datReader));

            return obj;
        }

    }
}
