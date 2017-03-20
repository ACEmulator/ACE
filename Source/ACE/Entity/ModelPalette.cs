using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// Used to replace default Palette colors / not required
    /// </summary>
    public class ModelPalette
    {

        /// <summary>
        ///  Palette portal.dat entry minus 0x04000000
        /// </summary>
        public ushort PaletteId { get; }

        public byte Offset { get; }

        public byte Length { get; }

        public ModelPalette(ushort paletteId, byte offset, byte length)
        {
            PaletteId = paletteId;
            Offset = offset;
            Length = length;
        }
    }
}
