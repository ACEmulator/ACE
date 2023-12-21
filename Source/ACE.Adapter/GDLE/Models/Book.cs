using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Book
    {
        [JsonPropertyName("maxNumCharsPerPage")]
        public int MaxCharactersPerPage { get; set; }

        [JsonPropertyName("maxNumPages")]
        public int MaxNumberPages { get; set; }

        [JsonPropertyName("pages")]
        public List<Page> Pages { get; set; } = new List<Page>();

    }
}
