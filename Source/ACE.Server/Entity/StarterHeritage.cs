using System.Collections.Generic;
using Newtonsoft.Json;

namespace ACE.Server.Entity
{
    public class StarterHeritage
    {
        [JsonProperty("id")]
        public ushort HeritageId { get; set; }

        /// <summary>
        /// not used, but the file has it for readability
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gear")]
        public List<StarterItem> Gear { get; set; } = new List<StarterItem>();

        /// <summary>
        /// Only needed to give an Olthoi Spitter starter spells.
        /// </summary>
        [JsonProperty("spells")]
        public List<StarterSpell> Spells { get; set; } = new List<StarterSpell>();
    }
}
