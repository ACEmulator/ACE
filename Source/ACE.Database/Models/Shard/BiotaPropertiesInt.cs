using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesInt
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public int Value { get; set; }

        public virtual Biota Object { get; set; }
    }
}
