using System.Collections.Generic;
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class LandblockValue
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<LandblockLink> links { get; set; }
        public List<LandblockWeenie> weenies { get; set; }
    }
}
