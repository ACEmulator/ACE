using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesPalette
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint SubPaletteId { get; set; }
        public ushort Offset { get; set; }
        public ushort Length { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
