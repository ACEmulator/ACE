using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Server.Entity
{
    public class StarterGearConfiguration
    {
        [JsonPropertyName("skills")]
        public List<StarterGearSkill> Skills { get; set; }
    }
}
