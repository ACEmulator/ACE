using System;
using System.Collections.Generic;

namespace ACE.Database.Models.EventLog
{
    public partial class PKKill
    {
        public uint Id { get; set; }
        public uint KillerId { get; set; }
        public uint VictimId { get; set; }

        public uint? KillerMonarchId { get; set; }

        public uint? VictimMonarchId { get; set; }

        public DateTime KillDateTime { get; set; }
    }
}
