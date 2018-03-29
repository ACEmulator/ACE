using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SpellComponentTable)]
    public class SpellComponentsTable : FileType
    {
        public enum Type
        {
            Scarab      = 1,
            Herb        = 2,
            Powder      = 3,
            Potion      = 4,
            Talisman    = 5,
            Taper       = 6,
            PotionPea   = 7,
            TalismanPea = 5,
            TaperPea    = 7
        }

        internal const uint FILE_ID = 0x0E00000F;

        public Dictionary<uint, SpellComponentBase> SpellComponents { get; } = new Dictionary<uint, SpellComponentBase>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            uint numComps = reader.ReadUInt16(); // Should be 163 or 0xA3
            reader.AlignBoundary();

            SpellComponents.Unpack(reader, numComps);
        }

        public static string GetSpellWords(SpellComponentsTable comps, List<uint> formula)
        {
            string firstSpellWord = null;
            string secondSpellWord = null;
            string thirdSpellWord = null;

            // Locate the herb component in the Spell formula
            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == (uint)Type.Herb)
                    firstSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            // Locate the powder component in the Spell formula
            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == (uint)Type.Powder)
                    secondSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            // Locate the potion component in the Spell formula
            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == (uint)Type.Potion)
                    thirdSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            string result = $"{firstSpellWord} {secondSpellWord}{thirdSpellWord.ToLower()}";
            return result;
        }
    }
}
