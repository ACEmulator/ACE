using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Api.Models
{
    public class AceProperty
    {
        [JsonProperty("id")]
        public uint PropertyId { get; set; }

        [JsonProperty("name")]
        public string PropertyName { get; set; }
    }
}
