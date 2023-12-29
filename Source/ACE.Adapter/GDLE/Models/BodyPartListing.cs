using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class BodyPartListing
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonIgnore]
        public BodyPartType BodyPartType
        {
            get
            {
                return (BodyPartType)Key;
            }
            set
            {
                Key = (int)value;
            }
        }

        [JsonPropertyName("value")]
        public BodyPart BodyPart { get; set; } = new BodyPart();


        [JsonIgnore]
        public bool Deleted { get; set; }

        public BodyPartListing()
        {
            BodyPart = new BodyPart();
        }
    }
}
