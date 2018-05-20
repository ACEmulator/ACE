using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesFillCompBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int SpellComponentId { get; set; }
        public int QuantityToRebuy { get; set; }

        public Biota Object { get; set; }
    }
}
