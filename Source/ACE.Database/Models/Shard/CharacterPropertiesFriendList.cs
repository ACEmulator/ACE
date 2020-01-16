using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesFriendList
    {
        public uint CharacterId { get; set; }
        public uint FriendId { get; set; }

        public virtual Character Character { get; set; }
    }
}
