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
    public class SimpleMessage
    {
        /// <summary>
        ///
        /// </summary>
        public SimpleMessage() { }

        /// <summary>
        ///
        /// </summary>
        public SimpleMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
