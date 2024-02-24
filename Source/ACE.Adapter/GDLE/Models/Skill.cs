using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Skill
    {
        [JsonPropertyName("level_from_pp")]
        public uint? LevelFromPp { get; set; } = 0u;


        [JsonPropertyName("last_used_time")]
        public float? LastUsed { get; set; } = 0f;


        [JsonPropertyName("init_level")]
        public uint? Ranks { get; set; } = 0u;


        [JsonPropertyName("pp")]
        public uint? XpInvested { get; set; } = 0u;


        [JsonPropertyName("resistance_of_last_check")]
        public uint? ResistanceOfLastCheck { get; set; } = 0u;


        [JsonPropertyName("sac")]
        public int? TrainedLevel { get; set; } = 0;


        [JsonIgnore]
        public SkillStatus? Status_Binder
        {
            get
            {
                return (SkillStatus?)TrainedLevel;
            }
            set
            {
                TrainedLevel = (int?)value;
            }
        }
    }
}
