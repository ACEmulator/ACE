
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class MetaSpell
    {
        [JsonProperty("sp_type")]
        public int Type { get; set; }

        [JsonProperty("spell")]
        public Spell Spell { get; set; }
    }
}
