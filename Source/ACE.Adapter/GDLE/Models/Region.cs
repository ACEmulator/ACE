
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class Region
    {
        [JsonProperty("encounterMap")]
        public List<uint> EncounterMap { get; set; }

        [JsonProperty("encounters")]
        public Encounter[] Encounters { get; set; }

        [JsonProperty("tableCount")]
        public int TableCount { get; set; }

        [JsonProperty("tableSize")]
        public int TableSize { get; set; }
    }
}
