using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x10. 
    /// It contains information on an items model, texture changes, available palette(s) and icons for that item.
    /// </summary>
    /// <remarks>
    /// Thanks to Steven Nygard and his work on the Mac program ACDataTools that were used to help debug & verify some of this data.
    /// </remarks>
    [DatFileType(DatFileType.Clothing)]
    public class ClothingTable : IUnpackable
    {
        public uint Id { get; private set; }
        /// <summary>
        /// Key is the setup model id
        /// </summary>
        public Dictionary<uint, ClothingBaseEffect> ClothingBaseEffects { get; } = new Dictionary<uint, ClothingBaseEffect>();
        /// <summary>
        /// Key is ?
        /// </summary>
        public Dictionary<uint, CloSubPalEffect> ClothingSubPalEffects { get; } = new Dictionary<uint, CloSubPalEffect>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ClothingBaseEffects.UnpackPackedHashTable(reader);

            ClothingSubPalEffects.UnpackPackedHashTable(reader);
        }

        public static ClothingTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (ClothingTable)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new ClothingTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }

        public uint GetIcon(uint palEffectIdx)
        {
            if (ClothingSubPalEffects.ContainsKey(palEffectIdx))
                return (ClothingSubPalEffects[palEffectIdx].Icon);

            return 0;
        }
    }
}
