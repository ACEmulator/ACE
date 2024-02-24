using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class StringStat
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonIgnore]
        public string PropertyIdBinder => ((StringPropertyId)Key).GetName();

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
