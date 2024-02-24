
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Event
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public EventValue Value { get; set; }
    }
}
