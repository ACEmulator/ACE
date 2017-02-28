using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity.WorldPackages
{

    /// <summary>
    /// Segment to Control AC Model / Pallets and Textures
    /// </summary>
    public class WorldObjectSegmentModelData
    {

        private byte paletteCount = 0; // number of pallets associated with model
        private byte textureCount = 0; //number of textures associate with model
        private byte modelCount = 0; // number of models

        //PackedDWORD
        private uint packedPaletteGuid;  // ?.

        private List<WorldObjectModelDataPallete> ModelDataPalletes = new List<WorldObjectModelDataPallete>();
        private List<WorldObjectModelDataTexture> ModelDataTextures = new List<WorldObjectModelDataTexture>();
        private List<WorldObjectModel> Models = new List<WorldObjectModel>();

        public void AddPallet (uint palette, byte offset, byte length)
        {
            paletteCount++;
            WorldObjectModelDataPallete newpallet = new WorldObjectModelDataPallete(palette, offset,length);
            ModelDataPalletes.Add(newpallet);
        }

        public void AddTexture(byte index, byte oldresourceid, byte newresourceid)
        {
            textureCount++;
            WorldObjectModelDataTexture nextexture = new WorldObjectModelDataTexture(index, oldresourceid, newresourceid);
            ModelDataTextures.Add(nextexture);
        }

        public void AddModel(byte index, byte modelresourceid)
        {
            modelCount++;
            WorldObjectModel newmodel = new WorldObjectModel(index, modelresourceid);
            Models.Add(newmodel);
        }

        //todo: render object network code
        public void Render(BinaryWriter writer)
        {
            writer.Write((byte)0x11);
            writer.Write((byte)paletteCount);
            writer.Write((byte)textureCount);
            writer.Write((byte)modelCount);

            writer.Write((uint)packedPaletteGuid);
            foreach (WorldObjectModelDataPallete pallet in ModelDataPalletes)
            {
                writer.Write((uint)pallet.Guid);
                writer.Write((byte)pallet.Offset);
                writer.Write((byte)pallet.Length);
            }

            foreach (WorldObjectModelDataTexture texture in ModelDataTextures)
            {
                writer.Write((byte)texture.Index);
                writer.Write((uint)texture.OldGuid);
                writer.Write((uint)texture.NewGuid);
            }

            foreach (WorldObjectModel model in Models)
            {
                writer.Write((byte)model.Index);
                writer.Write((uint)model.Guid);
            }

            writer.Align();

        }

    }

    //todo: move these into their own files..

    class WorldObjectModelDataPallete
    {
        public uint Guid { get; }
        public byte Offset { get; }
        public byte Length { get; }

        public WorldObjectModelDataPallete(uint guid, byte offset, byte length)
        {
            Guid = guid;
            Offset = offset;
            Length = length;
        }
    }

    /// <summary>
    /// Used to replace default textures // prob not needed unless you want too.
    /// </summary>
    class WorldObjectModelDataTexture
    {
        public byte Index { get; } //index of model to replace texture.
        public uint OldGuid { get; }
        public uint NewGuid { get; }

        public WorldObjectModelDataTexture(byte index, byte oldguid, byte newguid)
        {
            Index = index;
            OldGuid = oldguid; // - 0x05000000
            NewGuid = newguid; // - 0x05000000
        }
    }

    class WorldObjectModel
    {
        public byte Index { get; } //index of model
        public uint Guid { get; }  //- 0x01000000

        public WorldObjectModel(byte index, uint guid)
        {
            Index = index;
            Guid = guid;
        }
    }

}