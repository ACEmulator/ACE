using System;
using System.Collections.Generic;

#nullable disable

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesFloat
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public double Value { get; set; }

        public virtual Biota Object { get; set; }
    }
}
