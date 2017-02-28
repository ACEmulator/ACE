using System.Collections.Generic;

namespace ACE.Entity.WorldPackages
{

    /// <summary>
    /// Segment to Control AC Model / Pallets and Textures
    /// </summary>
    public class WorldObjectSegmentModelData
    {
        private byte init = 0x11; // thisis always 11 ?
        private byte paletteCount = 0; // number of pallets associated with model
        private byte textureCount = 0; //number of textures associate with model
        private byte modelCount = 0; // number of models

        //PackedDWORD
        private uint packedPaletteResourceId = 0;  // ?.

        private List<WorldObjectModelDataPallete> ModelDataPalletes = new List<WorldObjectModelDataPallete>();
        private List<WorldObjectModelDataTexture> ModelDataTextures = new List<WorldObjectModelDataTexture>();
        private List<WorldObjectModel> Models = new List<WorldObjectModel>();

        public void AddPallet (uint palette, byte offset, byte length)
        {
            WorldObjectModelDataPallete newpallet = new WorldObjectModelDataPallete(palette, offset,length);
            ModelDataPalletes.Add(newpallet);
        }

        public void AddTexture(byte index, byte oldresourceid, byte newresourceid)
        {
            WorldObjectModelDataTexture nextexture = new WorldObjectModelDataTexture(index, oldresourceid, newresourceid);
            ModelDataTextures.Add(nextexture);
        }

        public void AddModel(byte index, byte modelresourceid)
        {
            WorldObjectModel newmodel = new WorldObjectModel(index, modelresourceid);
            Models.Add(newmodel);
        }

        //todo: render object network code
        public void Render()
        {

        }

    }

    //todo: move these into their own files..
    class WorldObjectModelDataPallete
    {
        public uint Palette { get; }
        public byte Offset { get; }
        public byte Length { get; }

        public WorldObjectModelDataPallete(uint palette, byte offset, byte length)
        {
            Palette = palette;
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
        public uint OldResourceId { get; }
        public uint NewResourceId { get; }

        public WorldObjectModelDataTexture(byte index, byte oldresourceid, byte newresourceid)
        {
            Index = index;
            OldResourceId = oldresourceid; // - 0x05000000
            NewResourceId = newresourceid; // - 0x05000000
        }
    }

    class WorldObjectModel
    {
        public byte Index { get; } //index of model
        public uint ModelResourceId { get; }  //- 0x01000000

        public WorldObjectModel(byte index, uint modelresourceid)
        {
            Index = index;
            ModelResourceId = modelresourceid;
        }
    }

}
