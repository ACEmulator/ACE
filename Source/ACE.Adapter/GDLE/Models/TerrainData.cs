
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class TerrainData
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("value")]
        public List<ushort> Value { get; set; }
    }
}
