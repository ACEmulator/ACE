
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class MetaSpell
    {
        [JsonPropertyName("sp_type")]
        public int Type { get; set; }

        [JsonPropertyName("spell")]
        public Spell Spell { get; set; }
    }
}
