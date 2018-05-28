using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class TreasureWielded
    {
        public uint Id { get; set; }
        public uint TreasureType { get; set; }
        public uint WeenieClassId { get; set; }
        public uint PaletteId { get; set; }
        public uint Unknown1 { get; set; }
        public float Shade { get; set; }
        public int StackSize { get; set; }
        public float Unknown2 { get; set; }
        public float Probability { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public bool Unknown6 { get; set; }
        public bool Unknown7 { get; set; }
        public bool Unknown8 { get; set; }
        public uint Unknown9 { get; set; }
        public uint Unknown10 { get; set; }
        public uint Unknown11 { get; set; }
        public uint Unknown12 { get; set; }
    }
}
