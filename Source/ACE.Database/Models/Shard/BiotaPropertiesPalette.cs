using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesPalette
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint SubPaletteId { get; set; }
        public ushort Offset { get; set; }
        public ushort Length { get; set; }
        public byte? Order { get; set; }

        public virtual Biota Object { get; set; }
    }
}
