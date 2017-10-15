using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Controllers
{
    public class AuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
