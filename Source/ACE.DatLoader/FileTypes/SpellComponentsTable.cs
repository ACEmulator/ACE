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

        // TODO - Complete this function.
        public static string GetSpellWords(List<uint> comps)
        {
            string result = "";
            return result;
        }
    }
}
