
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Region
    {
        [JsonPropertyName("encounterMap")]
        public List<uint> EncounterMap { get; set; }

        [JsonPropertyName("encounters")]
        public Encounter[] Encounters { get; set; }

        [JsonPropertyName("tableCount")]
        public int TableCount { get; set; }

        [JsonPropertyName("tableSize")]
        public int TableSize { get; set; }
    }
}
