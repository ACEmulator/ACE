using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class LandDefs
    {
        public List<uint> LandHeightTable { get; set; } = new List<uint>();

        public static LandDefs Read(DatReader datReader)
        {
            LandDefs obj = new LandDefs();
            for (int i = 0; i < 256; i++)
            {
                obj.LandHeightTable.Add(datReader.ReadUInt32());
            }
            return obj;
        }
    }
}
