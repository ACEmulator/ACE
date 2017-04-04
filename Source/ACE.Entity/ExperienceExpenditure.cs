using Newtonsoft.Json;

namespace ACE.Entity
{
    public class ExperienceExpenditure
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("totalXp")]
        public uint TotalXp { get; set; }

        [JsonProperty("rankXp")]
        public uint XpFromPreviousRank { get; set; }
    }
}