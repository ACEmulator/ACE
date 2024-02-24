using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Attribute
    {
        [JsonPropertyName("cp_spent")]
        public uint? XpSpent { get; set; } = 0u;


        [JsonPropertyName("level_from_cp")]
        public uint LevelFromCp { get; set; } = 0u;


        [JsonPropertyName("init_level")]
        public uint? Ranks { get; set; } = 0u;

    }
}
