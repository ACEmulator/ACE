
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class TerrainData
    {
        [JsonPropertyName("key")]
        public uint Key { get; set; }

        [JsonPropertyName("value")]
        public List<ushort> Value { get; set; }
    }
}
