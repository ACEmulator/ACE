﻿using System;
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
    public class Image
    {
        /// <summary>
        /// mime type for the image
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("data")]
        public byte[] Data { get; set; }
    }
}
