using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class RecipePrecursor
    {
        public uint Tool { get; set; }

        public uint Target { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? RecipeId { get; set; }
    }
}
