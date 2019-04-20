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
    public class ClothingTable : FileType
    {
        /// <summary>
        /// Key is the setup model id
        /// </summary>
        public Dictionary<uint, ClothingBaseEffect> ClothingBaseEffects { get; } = new Dictionary<uint, ClothingBaseEffect>();
        /// <summary>
        /// Key is PaletteTemplate
        /// </summary>
        public Dictionary<uint, CloSubPalEffect> ClothingSubPalEffects { get; } = new Dictionary<uint, CloSubPalEffect>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ClothingBaseEffects.UnpackPackedHashTable(reader);

            ClothingSubPalEffects.UnpackPackedHashTable(reader);
        }

        public uint GetIcon(uint palEffectIdx)
        {
            if (ClothingSubPalEffects.TryGetValue(palEffectIdx, out CloSubPalEffect result))
                return result.Icon;

            return 0;
        }
    }
}
