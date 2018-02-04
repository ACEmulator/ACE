using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SpellComponentTable)]
    public class SpellComponentsTable : IUnpackable
    {
        private const uint FILE_ID = 0x0E00000F;

        public uint Id { get; private set; } // This should match FILE_ID
        public Dictionary<uint, SpellComponentBase> SpellComponents { get; } = new Dictionary<uint, SpellComponentBase>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            uint numComps = reader.ReadUInt16(); // Should be 163 or 0xA3
            reader.AlignBoundary();

            SpellComponents.Unpack(reader, numComps);
        }

        public static SpellComponentsTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(FILE_ID))
                return (SpellComponentsTable)DatManager.PortalDat.FileCache[FILE_ID];

            // Create the datReader for the proper file
            DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E00000F);

            var obj = new SpellComponentsTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[FILE_ID] = obj;

            return obj;
        }

        // TODO - Complete this function.
        public static string GetSpellWords(List<uint> comps)
        {
            string result = "";
            return result;
        }
    }
}
