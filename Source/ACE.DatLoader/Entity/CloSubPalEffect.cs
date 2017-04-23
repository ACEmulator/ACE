using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class CloSubPalEffect
    {
        /// <summary>
        /// Icon portal.dat 0x06000000
        /// </summary>
        public uint Icon { get; set; }
        public List<CloSubPalette> CloSubPalettes { get; set; } = new List<CloSubPalette>();
    }
}
