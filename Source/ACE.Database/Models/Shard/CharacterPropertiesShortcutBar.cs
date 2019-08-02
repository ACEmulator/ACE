using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesShortcutBar
    {
        public uint CharacterId { get; set; }
        public uint ShortcutBarIndex { get; set; }
        public uint ShortcutObjectId { get; set; }

        public virtual Character Character { get; set; }
    }
}
