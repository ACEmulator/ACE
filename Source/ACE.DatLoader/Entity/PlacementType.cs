using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class PlacementType
    {
        AnimationFrame AnimFrame { get; set; }

        public static PlacementType Read(int numParts, DatReader datReader)
        {
            PlacementType obj = new PlacementType();

            obj.AnimFrame = AnimationFrame.Read((uint)numParts, datReader);

            return obj;
        }
    }
}
