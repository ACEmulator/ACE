using System;
using System.Collections.Generic;

namespace ACE.Database.Models.PKKills
{
    public partial class Kills
    {
        public uint Id { get; set; }
        public uint KillerId { get; set; }
        public uint VictimId { get; set; }
        public string KillType { get; set; }
    }
}
