using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesSpellBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int Spell { get; set; }
        public float Probability { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
