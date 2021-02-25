using System;

namespace ACE.Entity.Models
{
    public class PropertiesAnimPart
    {
        public byte Index { get; set; }
        public uint AnimationId { get; set; }

        public PropertiesAnimPart Clone()
        {
            var result = new PropertiesAnimPart
            {
                Index = Index,
                AnimationId = AnimationId,
            };

            return result;
        }
    }
}
