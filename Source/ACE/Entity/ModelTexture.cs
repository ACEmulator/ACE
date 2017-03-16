using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// Used to replace default Textures / not required
    /// </summary>
    public class ModelTexture
    {
        /// <summary>
        /// Index of model to replace texture.
        /// </summary>
        public byte Index { get; }
        /// <summary>
        /// Texture portal.dat entry minus 0x05000000
        /// </summary>
        public ushort OldTexture { get; }
        /// <summary>
        /// Texture portal.dat entry minus 0x05000000
        /// </summary>
        public ushort NewTexture { get; } 

        public ModelTexture(byte index, ushort oldtexture, ushort newtexture)
        {
            Index = index;
            OldTexture = oldtexture;
            NewTexture = newtexture;
        }
    }
}
