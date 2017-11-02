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
    public class RegisterRequest
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }
}
