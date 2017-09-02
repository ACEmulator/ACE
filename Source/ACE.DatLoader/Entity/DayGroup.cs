using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class DayGroup
    {
        public float ChanceOfOccur { get; set; }
        public string DayName { get; set; }
        public List<SkyObject> SkyObjects { get; set; } = new List<SkyObject>();
        public List<SkyTimeOfDay> SkyTime { get; set; } = new List<SkyTimeOfDay>();

        public static DayGroup Read(DatReader datReader)
        {
            DayGroup obj = new DayGroup();
            obj.ChanceOfOccur = datReader.ReadSingle();
            obj.DayName = datReader.ReadPString();
            datReader.AlignBoundary();

            uint num_sky_objects = datReader.ReadUInt32();
            for (uint i = 0; i < num_sky_objects; i++)
                obj.SkyObjects.Add(SkyObject.Read(datReader));

            uint num_sky_times = datReader.ReadUInt32();
            for (uint i = 0; i < num_sky_times; i++)
                obj.SkyTime.Add(SkyTimeOfDay.Read(datReader));

            return obj;
        }
    }
}
