
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class Encounter
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("value")]
        public List<uint> Value { get; set; }
    }
}
