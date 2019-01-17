
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Event
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public EventValue Value { get; set; }
    }
}
