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
        public uint PaletteId { get; }

        public ushort Offset { get; }

        public ushort Length { get; }

        public ModelPalette(uint paletteId, ushort offset, ushort length)
        {
            PaletteId = paletteId;
            Offset = offset;
            Length = length;
        }
    }
}
