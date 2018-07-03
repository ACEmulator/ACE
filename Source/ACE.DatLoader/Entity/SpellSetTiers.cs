using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    public class SpellSetTiers : IUnpackable
    {
        /// <summary>
        /// A list of spell ids that are active in the spell set tier
        /// </summary>
        public List<uint> Spells = new List<uint>();

        public void Unpack(BinaryReader reader)
        {
            Spells.Unpack(reader);
        }
    }
}
