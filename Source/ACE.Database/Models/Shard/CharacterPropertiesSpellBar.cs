using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesSpellBar
    {
        public uint CharacterId { get; set; }
        public uint SpellBarNumber { get; set; }
        public uint SpellBarIndex { get; set; }
        public uint SpellId { get; set; }

        public virtual Character Character { get; set; }
    }
}
