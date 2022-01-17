using System;
using System.Collections.Generic;

#nullable disable

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesSpellBook
    {
        public uint ObjectId { get; set; }
        public int Spell { get; set; }
        public float Probability { get; set; }

        public virtual Biota Object { get; set; }
    }
}
