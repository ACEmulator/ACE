using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// Used to replace default Pallete colors / not required
    /// </summary>
    public class ModelPallete
    {

        /// <summary>
        ///  Palette portal.dat entry minus 0x04000000
        /// </summary>
        public ushort PaletteID { get; }
        public byte Offset { get; }
        public byte Length { get; }

        public ModelPallete(ushort paletteID, byte offset, byte length)
        {
            PaletteID = paletteID;
            Offset = offset;
            Length = length;
        }
    }
}
