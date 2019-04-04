using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class TreasureMaterialColor
    {
        public uint Id { get; set; }
        public uint MaterialId { get; set; }
        public uint TsysMutationColor { get; set; }
        public uint PaletteTemplate { get; set; }
        public float Chance { get; set; }
    }
}
