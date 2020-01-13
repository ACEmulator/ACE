using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class HousePermission
    {
        public uint HouseId { get; set; }
        public uint PlayerGuid { get; set; }
        public bool Storage { get; set; }

        public virtual Biota House { get; set; }
    }
}
