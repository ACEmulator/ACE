using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesFriendList
    {
        public uint Id { get; set; }
        public uint CharacterId { get; set; }
        public uint FriendId { get; set; }
        public uint AccountId { get; set; }

        public Character Character { get; set; }
    }
}
