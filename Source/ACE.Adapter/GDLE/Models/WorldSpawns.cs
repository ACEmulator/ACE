using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class WorldSpawns
    {
        [JsonProperty("_version")]
        public string Version { get; set; }

        [JsonProperty("landblocks")]
        public List<Landblock> Landblocks { get; set; }
    }
}
