using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellTable
    {
        [JsonProperty("spellBaseHash")]
        public List<SpellBaseHash> SpellBaseHash { get; set; }
    }
}
