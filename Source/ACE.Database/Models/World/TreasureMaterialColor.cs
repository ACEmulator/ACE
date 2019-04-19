using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class TreasureMaterialColor
    {
        public uint Id { get; set; }
        public int MaterialId { get; set; }
        public int ColorCode { get; set; }
        public int PaletteTemplate { get; set; }
        public float Probability { get; set; }
    }
}
