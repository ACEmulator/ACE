using Lifestoned.DataModel.DerethForever;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Vital
    {
        [JsonPropertyName("cp_spent")]
        public uint? XpSpent { get; set; } = 0u;


        [JsonPropertyName("level_from_cp")]
        public uint? LevelFromCp { get; set; } = 0u;


        [JsonPropertyName("init_level")]
        public uint? Ranks { get; set; } = 0u;


        [JsonPropertyName("current")]
        public uint? Current { get; set; } = 0u;


        public static Vital Convert(Ability ability)
        {
            Vital vital = new Vital();
            vital.Ranks = ability.Ranks;
            vital.Current = ability.Base + ability.Ranks;
            vital.XpSpent = ability.ExperienceSpent;
            vital.LevelFromCp = 0u;
            return vital;
        }
    }
}
