using Newtonsoft.Json;

namespace ACE.Entity
{
    public class CharacterLevel
    {
        [JsonProperty("level")]
        public uint Level { get; set; }

        [JsonProperty("totalXp")]
        public ulong TotalXp { get; set; }

        [JsonProperty("fromLastLevelXp")]
        public ulong FromPreviousLevel { get; set; }

        [JsonProperty("skillPoint")]
        public bool GrantsSkillPoint { get; set; }

        [JsonProperty("totalSkillPoints")]
        public int CumulativeSkillPoints { get; set; }
    }
}
