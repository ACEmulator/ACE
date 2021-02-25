using System;

namespace ACE.Entity.Models
{
    public class PropertiesPalette
    {
        public uint SubPaletteId { get; set; }
        public ushort Offset { get; set; }
        public ushort Length { get; set; }

        public PropertiesPalette Clone()
        {
            var result = new PropertiesPalette
            {
                SubPaletteId = SubPaletteId,
                Offset = Offset,
                Length = Length,
            };

            return result;
        }
    }
}
