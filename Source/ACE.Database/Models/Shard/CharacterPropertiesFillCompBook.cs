using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesFillCompBook
    {
        public uint CharacterId { get; set; }
        public int SpellComponentId { get; set; }
        public int QuantityToRebuy { get; set; }

        public virtual Character Character { get; set; }
    }
}
