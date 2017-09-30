using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class StarterSpell
    {
        [JsonProperty("spellId")]
        public uint SpellId { get; set; }

        /// <summary>
        /// not used, but in the json file for readability
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
