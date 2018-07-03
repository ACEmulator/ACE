using ACE.DatLoader.FileTypes;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{

    public class SpellSet : IUnpackable
    {
        // uint key is the m_PieceCount, the level/number of items active in the set.
        public Dictionary<uint, SpellSetTiers> SpellSetTiers = new Dictionary<uint, SpellSetTiers>();

        public void Unpack(BinaryReader reader)
        {
            SpellSetTiers.UnpackPackedHashTable(reader);
        }
    }
}
