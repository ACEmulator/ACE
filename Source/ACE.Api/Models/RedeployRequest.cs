using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Models
{
    /// <summary>
    ///
    /// </summary>
    public class RedeployRequest
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("force")]
        public bool ForceDeploy { get; set; } = false;
    }
}
