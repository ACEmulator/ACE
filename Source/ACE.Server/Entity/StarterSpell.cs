using System.Text.Json.Serialization;
using static ACE.Server.Factories.StarterGearFactory;

namespace ACE.Server.Entity
{
    public class StarterSpell
    {
        [JsonPropertyName("spellId")]
        public uint SpellId { get; set; }

        /// <summary>
        /// not used, but in the json file for readability
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("specializedOnly"), JsonConverter(typeof(StringToBoolConverter))]
        public bool SpecializedOnly { get; set; }
    }
}
