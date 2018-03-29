using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SpellComponentTable)]
    public class SpellComponentsTable : FileType
    {
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

            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == 2)
                    firstSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == 3)
                    secondSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents[formula[i]].Type == 4)
                    thirdSpellWord = comps.SpellComponents[formula[i]].Text;
            }

            string result = $"{firstSpellWord} {secondSpellWord}{thirdSpellWord.ToLower()}";
            return result;
        }
    }
}
