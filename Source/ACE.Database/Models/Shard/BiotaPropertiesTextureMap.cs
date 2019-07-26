using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesTextureMap
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public byte Index { get; set; }
        public uint OldId { get; set; }
        public uint NewId { get; set; }
        public byte? Order { get; set; }

        public virtual Biota Object { get; set; }
    }
}
