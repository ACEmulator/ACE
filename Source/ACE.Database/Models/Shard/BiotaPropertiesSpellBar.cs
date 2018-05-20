using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesSpellBar
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint SpellBarNumber { get; set; }
        public uint SpellBarIndex { get; set; }
        public uint SpellId { get; set; }

        public Biota Object { get; set; }
    }
}
