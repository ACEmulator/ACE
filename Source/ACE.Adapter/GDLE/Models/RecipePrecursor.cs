using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class RecipePrecursor
    {
        [JsonProperty("Tool")]
        public uint Tool { get; set; }

        [JsonProperty("Target")]
        public uint Target { get; set; }

        [JsonProperty("RecipeID")]
        public uint RecipeId { get; set; }
    }
}
