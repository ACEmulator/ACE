using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.AuthApi.Controllers
{
    public class RegisterResponse
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

        [JsonProperty("accountId")]
        public uint AccountId { get; set; }

        [JsonProperty("accountGuid")]
        public Guid AccountGuid { get; set; }
    }
}
