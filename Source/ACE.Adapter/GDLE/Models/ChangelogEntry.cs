using System;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class ChangelogEntry
    {
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}
