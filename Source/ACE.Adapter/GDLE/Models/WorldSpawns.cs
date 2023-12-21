using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class WorldSpawns
    {
        [JsonPropertyName("_version")]
        public string Version { get; set; }

        [JsonPropertyName("landblocks")]
        public List<Landblock> Landblocks { get; set; }
    }
}
