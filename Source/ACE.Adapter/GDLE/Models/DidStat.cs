using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class DidStat
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("value")]
        public uint Value { get; set; }

        [JsonIgnore]
        public string PropertyIdBinder => ((DidPropertyId)Key).GetName();

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
