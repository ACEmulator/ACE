using Newtonsoft.Json;

namespace ACE.Server.Entity
{
    public class StarterSpell
    {
        [JsonProperty("spellId")]
        public uint SpellId { get; set; }

        /// <summary>
        /// not used, but in the json file for readability
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("specializedOnly")]
        public bool SpecializedOnly { get; set; }
    }
}
