
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Spells
    {
        [JsonProperty("table")]
        public SpellTable Table { get; set; }
    }
}
