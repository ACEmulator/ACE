
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Encounter
    {
        [JsonPropertyName("key")]
        public uint Key { get; set; }

        [JsonPropertyName("value")]
        public List<uint> Value { get; set; }
    }
}
