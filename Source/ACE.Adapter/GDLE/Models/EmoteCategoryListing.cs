using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class EmoteCategoryListing
    {
        [JsonPropertyName("key")]
        public int EmoteCategoryId { get; set; }

        [JsonPropertyName("value")]
        public List<Emote> Emotes { get; set; } = new List<Emote>();


        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
