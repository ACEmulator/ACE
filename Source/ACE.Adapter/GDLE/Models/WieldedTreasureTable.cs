
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class WieldedTreasureTable
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("value")]
        public List<WieldedTreasure> Value { get; set; }
    }
}
