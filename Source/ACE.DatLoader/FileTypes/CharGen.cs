using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.CharacterGenerator)]
    public class CharGen : IUnpackable
    {
        private const uint FILE_ID = 0x0E000002;

        public int Id { get; private set; }
        public List<StarterArea> StarterAreas { get; } = new List<StarterArea>();
        public Dictionary<uint, HeritageGroupCG> HeritageGroups { get; } = new Dictionary<uint, HeritageGroupCG>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            reader.BaseStream.Position += 4;

            StarterAreas.UnpackSmartArray(reader);

            // HERITAGE GROUPS -- 11 standard player races and 2 Olthoi.
            reader.BaseStream.Position++; // Not sure what this byte 0x01 is indicating, but we'll skip it because we can.

            HeritageGroups.UnpackSmartArray(reader);
        }

        public static CharGen ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(FILE_ID, out var result))
                return (CharGen)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(FILE_ID);

            var obj = new CharGen();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[FILE_ID] = obj;

            return obj;
        }
    }
}
