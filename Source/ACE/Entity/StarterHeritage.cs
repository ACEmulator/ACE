using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class StarterHeritage
    {
        [JsonProperty("id")]
        public ushort HeritageId { get; set; }

        /// <summary>
        /// not used, but the file has it for readability
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gear")]
        public List<StarterItem> Gear { get; set; } = new List<StarterItem>();
    }
}
