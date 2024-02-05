using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class AttributeSet
    {
        [JsonPropertyName("strength")]
        public Attribute Strength { get; set; }

        [JsonPropertyName("endurance")]
        public Attribute Endurance { get; set; }

        [JsonPropertyName("coordination")]
        public Attribute Coordination { get; set; }

        [JsonPropertyName("quickness")]
        public Attribute Quickness { get; set; }

        [JsonPropertyName("focus")]
        public Attribute Focus { get; set; }

        [JsonPropertyName("self")]
        public Attribute Self { get; set; }

        [JsonPropertyName("health")]
        public Vital Health { get; set; } = new Vital();


        [JsonPropertyName("stamina")]
        public Vital Stamina { get; set; } = new Vital();


        [JsonPropertyName("mana")]
        public Vital Mana { get; set; } = new Vital();

    }
}
