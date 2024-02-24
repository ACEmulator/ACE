using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Int64Stat
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("value")]
        public long Value { get; set; }

        [JsonIgnore]
        public string PropertyIdBinder => ((Int64PropertyId)Key).GetName();

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
