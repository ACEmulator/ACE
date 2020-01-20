using System;

namespace ACE.Entity.Models
{
    public class PropertiesTextureMap
    {
        public byte Index { get; set; }
        public uint OldId { get; set; }
        public uint NewId { get; set; }

        public PropertiesTextureMap Clone()
        {
            var result = new PropertiesTextureMap
            {
                Index = Index,
                OldId = OldId,
                NewId = NewId,
            };

            return result;
        }
    }
}
