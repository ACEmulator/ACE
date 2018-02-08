using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.CharacterGenerator)]
    public class CharGen : FileType
    {
        internal const uint FILE_ID = 0x0E000002;

        public List<StarterArea> StarterAreas { get; } = new List<StarterArea>();
        public Dictionary<uint, HeritageGroupCG> HeritageGroups { get; } = new Dictionary<uint, HeritageGroupCG>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            reader.BaseStream.Position += 4;

            StarterAreas.UnpackSmartArray(reader);

            // HERITAGE GROUPS -- 11 standard player races and 2 Olthoi.
            reader.BaseStream.Position++; // Not sure what this byte 0x01 is indicating, but we'll skip it because we can.

            HeritageGroups.UnpackSmartArray(reader);
        }
    }
}
