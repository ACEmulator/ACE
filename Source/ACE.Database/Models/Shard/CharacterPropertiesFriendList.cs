using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesFriendList
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint FriendId { get; set; }
        public uint AccountId { get; set; }

        public Biota Object { get; set; }
    }
}
