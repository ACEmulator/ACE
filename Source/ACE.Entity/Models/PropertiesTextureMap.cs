using System;

namespace ACE.Entity.Models
{
    public class PropertiesTextureMap
    {
        public byte PartIndex { get; set; }
        public uint OldTexture { get; set; }
        public uint NewTexture { get; set; }

        public PropertiesTextureMap Clone()
        {
            var result = new PropertiesTextureMap
            {
                PartIndex = PartIndex,
                OldTexture = OldTexture,
                NewTexture = NewTexture,
            };

            return result;
        }
    }
}
