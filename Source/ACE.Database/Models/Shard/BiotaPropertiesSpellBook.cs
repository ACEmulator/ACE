using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesSpellBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int Spell { get; set; }
        public float Probability { get; set; }

        public Biota Object { get; set; }
    }
}
