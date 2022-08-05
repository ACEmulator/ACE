using System;
using System.Collections.Generic;

#nullable disable

namespace ACE.Database.Models.World
{
    public partial class TreasureMaterialBase
    {
        public uint Id { get; set; }
        public uint MaterialCode { get; set; }
        public uint Tier { get; set; }
        public float Probability { get; set; }
        public uint MaterialId { get; set; }
    }
}
