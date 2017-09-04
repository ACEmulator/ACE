using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class LandSurf
    {
        public uint HasPalShift { get; set; }
        public TexMerge TexMerge { get; set; }

        public static LandSurf Read(DatReader datReader)
        {
            LandSurf obj = new LandSurf();

            obj.HasPalShift = datReader.ReadUInt32(); // This is always 0

            if (obj.HasPalShift == 1)
            {
                // PalShift.Read would go here, if it ever actually existed...which it doesn't.
            }
            else
                obj.TexMerge = TexMerge.Read(datReader);

            return obj;
        }
    }
}
