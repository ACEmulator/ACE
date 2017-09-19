using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class StarterGearConfiguration
    {
        [JsonProperty("skills")]
        public List<StarterGearSkill> Skills { get; set; }
    }
}
