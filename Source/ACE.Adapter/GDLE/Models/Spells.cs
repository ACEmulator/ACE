
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Spells
    {
        [JsonPropertyName("table")]
        public SpellTable Table { get; set; }
    }
}
