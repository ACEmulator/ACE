using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

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

        /// <summary>
        /// Calculates the ClothingPriority of an item based on the actual coverage. So while an Over-Robe may just be "Chest", we want to know it covers everything but head & arms.
        /// </summary>
        /// <param name="setupId">Defaults to HUMAN_MALE if not set, which is good enough</param>
        /// <returns></returns>
        public CoverageMask? GetVisualPriority(uint setupId = 0x02000001)
        {
            if (ClothingBaseEffects.ContainsKey(setupId))
            {
                CoverageMask visualPriority = 0;
                foreach (CloObjectEffect t in ClothingBaseEffects[setupId].CloObjectEffects)
                    switch (t.Index)
                    {
                        case 0: // HUMAN_ABDOMEN;
                            visualPriority |= CoverageMask.OuterwearAbdomen;
                            break;
                        case 1: // HUMAN_LEFT_UPPER_LEG;
                        case 5: // HUMAN_RIGHT_UPPER_LEG;
                            visualPriority |= CoverageMask.OuterwearUpperLegs;
                            break;
                        case 2: // HUMAN_LEFT_LOWER_LEG;
                        case 6: // HUMAN_RIGHT_LOWER_LEG;
                            visualPriority |= CoverageMask.OuterwearLowerLegs;
                            break;
                        case 3: // HUMAN_LEFT_FOOT;
                        case 4: // HUMAN_LEFT_TOE;
                        case 7: // HUMAN_RIGHT_FOOT;
                        case 8: // HUMAN_RIGHT_TOE;
                            visualPriority |= CoverageMask.Feet;
                            break;
                        case 9: // HUMAN_CHEST;
                            visualPriority |= CoverageMask.OuterwearChest;
                            break;
                        case 10: // HUMAN_LEFT_UPPER_ARM;
                        case 13: // HUMAN_RIGHT_UPPER_ARM;
                            visualPriority |= CoverageMask.OuterwearUpperArms;
                            break;
                        case 11: // HUMAN_LEFT_LOWER_ARM;
                        case 14: // HUMAN_RIGHT_LOWER_ARM;
                            visualPriority |= CoverageMask.OuterwearLowerArms;
                            break;
                        case 12: // HUMAN_LEFT_HAND;
                        case 15: // HUMAN_RIGHT_HAND;
                            visualPriority |= CoverageMask.Hands;
                            break;
                        case 16: // HUMAN_HEAD;
                            visualPriority |= CoverageMask.Head;
                            break;
                        default: // Lots of things we don't care about
                            break;
                    }
                return visualPriority;
            }
            else
                return null;            
        }
    }
}
