using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class BoolStat
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonIgnore]
        public bool BoolValue
        {
            get
            {
                return Value != 0;
            }
            set
            {
                Value = (value ? 1 : 0);
            }
        }

        [JsonIgnore]
        public string PropertyIdBinder => ((BoolPropertyId)Key).GetName();

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
