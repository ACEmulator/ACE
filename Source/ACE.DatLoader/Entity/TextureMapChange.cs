using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class TextureMapChange
    {
        public byte PartIndex { get; set; }
        public ushort OldTexture { get; set; }
        public ushort NewTexture { get; set; }
    }
}
