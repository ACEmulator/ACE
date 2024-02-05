using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Server.Entity
{
    public class StarterGearSkill
    {
        [JsonPropertyName("id")]
        public ushort SkillId { get; set; }

        /// <summary>
        /// not used, but the file has it for readability
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gear")]
        public List<StarterItem> Gear { get; set; } = new List<StarterItem>();

        [JsonPropertyName("heritage")]
        public List<StarterHeritage> Heritage { get; set; } = new List<StarterHeritage>();

        [JsonPropertyName("spells")]
        public List<StarterSpell> Spells { get; set; } = new List<StarterSpell>();
    }
}
