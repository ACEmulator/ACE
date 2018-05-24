using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesQuestRegistry
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public string QuestName { get; set; }
        public uint LastTimeCompleted { get; set; }
        public int NumTimesCompleted { get; set; }

        public Biota Object { get; set; }
    }
}
