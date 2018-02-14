using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesFriendList
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint FriendId { get; set; }
        public uint AccountId { get; set; }

        public Biota Friend { get; set; }
        public Biota Object { get; set; }
    }
}
