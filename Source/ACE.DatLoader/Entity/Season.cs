using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class Season
    {
        public uint StartDate { get; set; }
        public string Name { get; set; }

        public static Season Read(DatReader datReader)
        {
            Season obj = new Season();
            obj.StartDate = datReader.ReadUInt32();
            obj.Name = datReader.ReadPString();
            datReader.AlignBoundary();
            return obj;
        }
    }
}
