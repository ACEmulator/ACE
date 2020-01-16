using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesBool
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public bool Value { get; set; }

        public virtual Biota Object { get; set; }
    }
}
