using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Common
{
    public class AuthResponse
    {
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }
    }
}
