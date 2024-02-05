using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellCastingStats
    {
        [JsonPropertyName("casting_likelihood")]
        public double? CastingChance { get; set; }
    }
}
