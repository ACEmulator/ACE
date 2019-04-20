using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Encounter
    {
        public uint Id { get; set; }
        public int Landblock { get; set; }
        public uint WeenieClassId { get; set; }
        public int CellX { get; set; }
        public int CellY { get; set; }
        public DateTime LastModified { get; set; }
    }
}
