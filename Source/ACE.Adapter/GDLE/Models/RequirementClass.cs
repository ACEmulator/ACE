using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class RequirementClass
    {
        [JsonProperty("BoolRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<Requirement> BoolRequirements { get; set; }

        [JsonProperty("IntRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<Requirement> IntRequirements { get; set; }
    }
}
