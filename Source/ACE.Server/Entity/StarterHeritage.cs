using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Server.Entity
{
    public class StarterHeritage
    {
        [JsonPropertyName("id")]
        public ushort HeritageId { get; set; }

        /// <summary>
        /// not used, but the file has it for readability
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gear")]
        public List<StarterItem> Gear { get; set; } = new List<StarterItem>();

        /// <summary>
        /// Only needed to give an Olthoi Spitter starter spells.
        /// </summary>
        [JsonPropertyName("spells")]
        public List<StarterSpell> Spells { get; set; } = new List<StarterSpell>();
    }
}
