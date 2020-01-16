using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesEventFilter
    {
        public uint ObjectId { get; set; }
        public int Event { get; set; }

        public virtual Biota Object { get; set; }
    }
}
