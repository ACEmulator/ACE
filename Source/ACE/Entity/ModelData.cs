﻿using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public List<ModelPalette> GetPalettes
        {
            get { return modelPalettes.ToList(); }
        }

        public List<ModelTexture> GetTextures
        {
            get { return modelTextures.ToList(); }
        }

        public List<Model> GetModels
        {
            get { return models.ToList(); }
        }

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
                writer.WritePackedDwordOfKnownType(PaletteGuid, 0x4000000);
            foreach (ModelPalette palette in modelPalettes)
            {
                writer.WritePackedDwordOfKnownType(palette.PaletteId, 0x4000000);
                writer.Write((byte)palette.Offset);
                writer.Write((byte)palette.Length);
            }

            foreach (ModelTexture texture in modelTextures)
            {
                writer.Write((byte)texture.Index);
                writer.WritePackedDwordOfKnownType(texture.OldTexture, 0x5000000);
                writer.WritePackedDwordOfKnownType(texture.NewTexture, 0x5000000);
            }

            foreach (Model model in models)
            {
                writer.Write((byte)model.Index);
                writer.WritePackedDwordOfKnownType(model.ModelID, 0x1000000);
            }

            writer.Align();
        }
    }
}