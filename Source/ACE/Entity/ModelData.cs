using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{

    /// <summary>
    /// Segment to Control AC Model / Palettes and Textures
    /// </summary>
    public class ModelData
    {
        public uint PaletteGuid { get; set; } = 0;

        private List<ModelPalette> modelPalettes = new List<ModelPalette>();

        private List<ModelTexture> modelTextures = new List<ModelTexture>();

        private List<Model> models = new List<Model>();

        public void AddPalette(uint paletteID, ushort offset, ushort length)
        {
            ModelPalette newpalette = new ModelPalette(paletteID, offset, length);
            modelPalettes.Add(newpalette);
        }

        public void AddTexture(byte index, ushort oldtexture, ushort newtexture)
        {
            ModelTexture nextTexture = new ModelTexture(index, oldtexture, newtexture);
            modelTextures.Add(nextTexture);
        }

        public void AddModel(byte index, ushort modelresourceid)
        {
            Model newmodel = new Model(index, modelresourceid);
            models.Add(newmodel);
        }

        // todo: render object network code
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0x11);
            writer.Write((byte)modelPalettes.Count);
            writer.Write((byte)modelTextures.Count);
            writer.Write((byte)models.Count);

            if (modelPalettes.Count > 0)
                writer.Write((ushort)PaletteGuid);
            foreach (ModelPalette palette in modelPalettes)
            {
                writer.Write((ushort)palette.PaletteId);
                writer.Write((byte)palette.Offset);
                writer.Write((byte)palette.Length);

            }

            foreach (ModelTexture texture in modelTextures)
            {
                writer.Write((byte)texture.Index);
                writer.Write((ushort)texture.OldTexture);
                writer.Write((ushort)texture.NewTexture);
            }

            foreach (Model model in models)
            {
                writer.Write((byte)model.Index);
                writer.Write((ushort)model.ModelID);
            }

            writer.Align();

        }

    }
}