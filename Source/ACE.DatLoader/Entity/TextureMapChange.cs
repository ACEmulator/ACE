using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TextureMapChange
    {
        public int PartIndex { get; set; }
        public uint OldTexture { get; set; }
        public uint NewTexture { get; set; }
    }
}
