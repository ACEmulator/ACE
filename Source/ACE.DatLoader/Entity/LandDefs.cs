using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class LandDefs
    {
        public List<float> LandHeightTable { get; set; } = new List<float>();

        public static LandDefs Read(DatReader datReader)
        {
            LandDefs obj = new LandDefs();
            for (int i = 0; i < 256; i++)
            {
                obj.LandHeightTable.Add(datReader.ReadSingle());
            }
            return obj;
        }
    }
}
