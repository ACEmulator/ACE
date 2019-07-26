using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesSquelch
    {
        public uint CharacterId { get; set; }
        public uint SquelchCharacterId { get; set; }
        public uint SquelchAccountId { get; set; }
        public uint Type { get; set; }

        public virtual Character Character { get; set; }
    }
}
