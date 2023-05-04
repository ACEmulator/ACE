
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellBaseHash
    {
        [JsonPropertyName("key")]
        public uint Key { get; set; }

        [JsonPropertyName("value")]
        public SpellValue Value { get; set; }
    }
}
