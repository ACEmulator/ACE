using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class RecipeRequirements
    {
        [JsonProperty("BoolRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> BoolRequirements { get; set; }

        [JsonProperty("IntRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> IntRequirements { get; set; }
    }
}
