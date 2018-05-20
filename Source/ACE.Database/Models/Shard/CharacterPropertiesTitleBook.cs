using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesTitleBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint TitleId { get; set; }

        public Biota Object { get; set; }
    }
}
