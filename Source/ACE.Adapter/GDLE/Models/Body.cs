using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Body
    {
        [JsonPropertyName("body_part_table")]
        public List<BodyPartListing> BodyParts { get; set; } = new List<BodyPartListing>();

    }
}
