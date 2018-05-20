using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesContract
    {
        public uint ObjectId { get; set; }
        public uint ContractId { get; set; }
        public uint Version { get; set; }
        public uint Stage { get; set; }
        public ulong TimeWhenDone { get; set; }
        public ulong TimeWhenRepeats { get; set; }
        public bool DeleteContract { get; set; }
        public bool SetAsDisplayContract { get; set; }

        public Biota Object { get; set; }
    }
}
