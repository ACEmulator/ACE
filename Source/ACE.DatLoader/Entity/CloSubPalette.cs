using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CloSubPalette : IUnpackable
    {
        /// <summary>
        /// Contains a list of valid offsets & color values
        /// </summary>
        public List<CloSubPalleteRange> Ranges { get; } = new List<CloSubPalleteRange>();
        /// <summary>
        /// Icon portal.dat 0x0F000000
        /// </summary>
        public uint PaletteSet { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Ranges.Unpack(reader);

            PaletteSet = reader.ReadUInt32();
        }
    }
}
