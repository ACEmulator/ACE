using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class CloSubPalette
    {
        /// <summary>
        /// Contains a list of valid offsets & color values
        /// </summary>
        public List<CloSubPalleteRange> Ranges { get; set; } = new List<CloSubPalleteRange>();
        /// <summary>
        /// Icon portal.dat 0x0F000000
        /// </summary>
        public uint PaletteSet { get; set; }
    }
}
