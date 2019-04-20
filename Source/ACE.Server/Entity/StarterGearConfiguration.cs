using System.Collections.Generic;
using Newtonsoft.Json;

namespace ACE.Server.Entity
{
    public class StarterGearConfiguration
    {
        [JsonProperty("skills")]
        public List<StarterGearSkill> Skills { get; set; }
    }
}
