using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellTable
    {
        [JsonPropertyName("spellBaseHash")]
        public List<SpellBaseHash> SpellBaseHash { get; set; }
    }
}
