﻿using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{

    /// <summary>
    /// Segment to Control AC Model / Pallets and Textures
    /// </summary>
    public class ModelData
    {
        
        private List<ModelPallete> modelPalletes = new List<ModelPallete>();
        private List<ModelTexture> modelTextures = new List<ModelTexture>();
        private List<Model> models = new List<Model>();

        public void AddPallet (uint palette, byte offset, byte length)
        {
            ModelPallete newpallet = new ModelPallete(palette, offset,length);
            modelPalletes.Add(newpallet);
        }

        public void AddTexture(byte index, byte oldresourceid, byte newresourceid)
        {
            ModelTexture nextexture = new ModelTexture(index, oldresourceid, newresourceid);
            modelTextures.Add(nextexture);
        }

        public void AddModel(byte index, byte modelresourceid)
        {
            Model newmodel = new Model(index, modelresourceid);
            models.Add(newmodel);
        }

        //todo: render object network code
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0x11);
            writer.Write((byte)modelPalletes.Count);
            writer.Write((byte)modelTextures.Count);
            writer.Write((byte)models.Count);

            foreach (ModelPallete pallet in modelPalletes)
            {
                writer.Write((uint)pallet.Guid);
                writer.Write((byte)pallet.Offset);
                writer.Write((byte)pallet.Length);
            }

            foreach (ModelTexture texture in modelTextures)
            {
                writer.Write((byte)texture.Index);
                writer.Write((uint)texture.OldGuid);
                writer.Write((uint)texture.NewGuid);
            }

            foreach (Model model in models)
            {
                writer.Write((byte)model.Index);
                writer.Write((uint)model.Guid);
            }

            writer.Align();

        }

    }
}