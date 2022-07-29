using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CloSubPalette : IUnpackable
    {
        /// <summary>
        /// Contains a list of valid offsets & color values
        /// </summary>
        public List<CloSubPaletteRange> Ranges { get; } = new List<CloSubPaletteRange>();
        /// <summary>
        /// Icon portal.dat 0x0F000000
        /// </summary>
        public uint PaletteSet { get; set; }

        public void Unpack(BinaryReader reader)
        {
            Ranges.Unpack(reader);

            PaletteSet = reader.ReadUInt32();
        }
    }
}
