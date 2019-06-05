using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class TreasureMaterialGroups
    {
        public uint Id { get; set; }
        public uint MaterialGroup { get; set; }
        public uint Tier { get; set; }
        public float Probability { get; set; }
        public uint MaterialId { get; set; }
    }
}
