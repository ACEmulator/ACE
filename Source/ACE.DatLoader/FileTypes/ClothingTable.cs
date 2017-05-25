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
                var ct = new ClothingTable();
                var datReader = DatManager.PortalDat.GetReaderForFile(fileId);

                ct.Id = datReader.ReadUInt32();

                uint numClothingEffects = datReader.ReadUInt16();
                datReader.Offset += 2;
                for (uint i = 0; i < numClothingEffects; i++)
                {
                    var cb = new ClothingBaseEffect();
                    cb.SetupModel = datReader.ReadUInt32();
                    var numObjectEffects = datReader.ReadInt32();
                    for (var j = 0; j < numObjectEffects; j++)
                    {
                        var cloObjEffect = new CloObjectEffect();
                        cloObjEffect.Index = datReader.ReadUInt32();
                        cloObjEffect.ModelId = datReader.ReadUInt32();
                        var numTextureEffects = datReader.ReadUInt32();

                        for (uint k = 0; k < numTextureEffects; k++)
                        {
                            var cloTexEffect = new CloTextureEffect();
                            cloTexEffect.OldTexture = datReader.ReadUInt32();
                            cloTexEffect.NewTexture = datReader.ReadUInt32();
                            cloObjEffect.CloTextureEffects.Add(cloTexEffect);
                        }

                        cb.CloObjectEffects.Add(cloObjEffect);
                    }
                    ct.ClothingBaseEffects.Add(cb.SetupModel, cb);
                }

                var numSubPalEffects = datReader.ReadUInt16();
                for (uint i = 0; i < numSubPalEffects; i++)
                {
                    datReader.AlignBoundary();
                    var cloSubPalEffect = new CloSubPalEffect();
                    var subPalIdx = datReader.ReadUInt32();
                    cloSubPalEffect.Icon = datReader.ReadUInt32();
                    var numPalettes = datReader.ReadUInt32();
                    for (uint j = 0; j < numPalettes; j++)
                    {
                        var cloSubPalette = new CloSubPalette();
                        var length = datReader.ReadUInt32();
                        for (uint k = 0; k < length; k++)
                        {
                            var range = new CloSubPalleteRange();
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
    }
}
