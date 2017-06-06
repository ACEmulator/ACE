using System.Collections.Generic;
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
    public class ClothingTable
    {
        public uint Id { get; private set; }
        public Dictionary<uint, ClothingBaseEffect> ClothingBaseEffects { get; set; } = new Dictionary<uint, ClothingBaseEffect>(); // uint is the setup model id
        public Dictionary<uint, CloSubPalEffect> ClothingSubPalEffects { get; set; } = new Dictionary<uint, CloSubPalEffect>(); // uint is a non-zero index

        public static ClothingTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (ClothingTable)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                ClothingTable ct = new ClothingTable();
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

                ct.Id = datReader.ReadUInt32();

                uint numClothingEffects = datReader.ReadUInt16();
                datReader.Offset += 2;
                for (uint i = 0; i < numClothingEffects; i++)
                {
                    ClothingBaseEffect cb = new ClothingBaseEffect();
                    cb.SetupModel = datReader.ReadUInt32();
                    int numObjectEffects = datReader.ReadInt32();
                    for (int j = 0; j < numObjectEffects; j++)
                    {
                        CloObjectEffect cloObjEffect = new CloObjectEffect();
                        cloObjEffect.Index = datReader.ReadUInt32();
                        cloObjEffect.ModelId = datReader.ReadUInt32();
                        uint numTextureEffects = datReader.ReadUInt32();

                        for (uint k = 0; k < numTextureEffects; k++)
                        {
                            CloTextureEffect cloTexEffect = new CloTextureEffect();
                            cloTexEffect.OldTexture = datReader.ReadUInt32();
                            cloTexEffect.NewTexture = datReader.ReadUInt32();
                            cloObjEffect.CloTextureEffects.Add(cloTexEffect);
                        }

                        cb.CloObjectEffects.Add(cloObjEffect);
                    }
                    ct.ClothingBaseEffects.Add(cb.SetupModel, cb);
                }

                ushort numSubPalEffects = datReader.ReadUInt16();
                for (uint i = 0; i < numSubPalEffects; i++)
                {
                    datReader.AlignBoundary();
                    CloSubPalEffect cloSubPalEffect = new CloSubPalEffect();
                    uint subPalIdx = datReader.ReadUInt32();
                    cloSubPalEffect.Icon = datReader.ReadUInt32();
                    uint numPalettes = datReader.ReadUInt32();
                    for (uint j = 0; j < numPalettes; j++)
                    {
                        CloSubPalette cloSubPalette = new CloSubPalette();
                        uint length = datReader.ReadUInt32();
                        for (uint k = 0; k < length; k++)
                        {
                            CloSubPalleteRange range = new CloSubPalleteRange();
                            range.Offset = datReader.ReadUInt32();
                            range.NumColors = datReader.ReadUInt32();
                            cloSubPalette.Ranges.Add(range);
                        }
                        cloSubPalette.PaletteSet = datReader.ReadUInt32();
                        cloSubPalEffect.CloSubPalettes.Add(cloSubPalette);
                    }
                    ct.ClothingSubPalEffects.Add(subPalIdx, cloSubPalEffect);
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = ct;
                return ct;
            }
        }

        public uint GetIcon(uint palEffectIdx)
        {
            if (ClothingSubPalEffects.ContainsKey(palEffectIdx))
                return (ClothingSubPalEffects[palEffectIdx].Icon);
            else
                return 0;
        }
    }
}
