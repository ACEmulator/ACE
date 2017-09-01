using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TimeOfDay
    {
        public uint Start { get; set; }
        public uint IsNight { get; set; }
        public string Name { get; set; }

        public static TimeOfDay Read(DatReader datReader)
        {
            TimeOfDay obj = new TimeOfDay();
            obj.Start = datReader.ReadUInt32();
            obj.IsNight = datReader.ReadUInt32();
            obj.Name = datReader.ReadPString();
            datReader.AlignBoundary();
            return obj;
        }
    }
}
