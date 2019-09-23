using ACE.DatLoader.FileTypes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            //var last = SpellSetTiers.LastOrDefault();
            //var first = SpellSetTiers.FirstOrDefault();

            SpellSetTiers lastSpellSetTier = null;

            for (uint i = 0; i < SpellSetTiers.Keys.LastOrDefault(); i++)
            {                
                if (SpellSetTiers.ContainsKey(i))
                    lastSpellSetTier = SpellSetTiers[i];
                else
                {
                    if (lastSpellSetTier != null)
                        SpellSetTiers.Add(i, lastSpellSetTier);
                }

            }
        }
    }
}
