using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class TreasureGemCount
    {
        public uint Id { get; set; }
        public byte GemCode { get; set; }
        public int Tier { get; set; }
        public int Count { get; set; }
        public float Chance { get; set; }
    }
}
