using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class CharacterPropertiesQuestRegistry
    {
        public uint CharacterId { get; set; }
        public string QuestName { get; set; }
        public uint LastTimeCompleted { get; set; }
        public int NumTimesCompleted { get; set; }

        public virtual Character Character { get; set; }
    }
}
