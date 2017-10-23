using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Models
{
    public class SimpleMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
