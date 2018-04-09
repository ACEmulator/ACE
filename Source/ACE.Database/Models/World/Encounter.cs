using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Encounter
    {
        public uint Id { get; set; }
        public int Index { get; set; }
        public uint WeenieClassId { get; set; }
        public string EncounterMap { get; set; }
    }
}
