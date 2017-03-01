using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.WorldPackages
{
    /// <summary>
    /// Used to replace default textures / not needed unless you want too.
    /// </summary>
    public class WorldObjectModelDataTexture
    {
        public byte Index { get; } //index of model to replace texture.
        public uint OldGuid { get; }
        public uint NewGuid { get; }

        public WorldObjectModelDataTexture(byte index, uint oldguid, uint newguid)
        {
            Index = index;
            OldGuid = oldguid; // - 0x05000000
            NewGuid = newguid; // - 0x05000000
        }
    }
}
