using ACE.DatLoader.FileTypes;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{

    public class SpellSet : IUnpackable
    {
        // uint key is the total combined item level of all the equipped pieces in the set
        // client calls this m_PieceCount
        public SortedDictionary<uint, SpellSetTiers> SpellSetTiers = new SortedDictionary<uint, SpellSetTiers>();

        public void Unpack(BinaryReader reader)
        {
            SpellSetTiers.UnpackPackedHashTable(reader);
        }
    }
}
