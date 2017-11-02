using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Api.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AceProperty
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("id")]
        public uint PropertyId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("name")]
        public string PropertyName { get; set; }
    }
}
