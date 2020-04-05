using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesSkill
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public ushort LevelFromPP { get; set; }
        public uint SAC { get; set; }
        public uint PP { get; set; }
        public uint InitLevel { get; set; }
        public uint ResistanceAtLastCheck { get; set; }
        public double LastUsedTime { get; set; }

        public virtual Biota Object { get; set; }
    }
}
