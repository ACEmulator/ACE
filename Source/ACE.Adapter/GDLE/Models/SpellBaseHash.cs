
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellBaseHash
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("value")]
        public SpellValue Value { get; set; }
    }
}
