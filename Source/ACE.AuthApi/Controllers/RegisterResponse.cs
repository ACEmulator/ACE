using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.AuthApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("accountId")]
        public uint AccountId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("accountGuid")]
        public Guid AccountGuid { get; set; }
    }
}
