using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesQuestRegistry
    {
        public uint ObjectId { get; set; }
        public string QuestName { get; set; }
        public uint LastTimeCompleted { get; set; }
        public int NumTimesCompleted { get; set; }

        public Biota Object { get; set; }
    }
}
